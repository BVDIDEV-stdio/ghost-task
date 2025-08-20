using System;
using System.Collections;
using Ashsvp;
using Unity.VisualScripting;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public GameObject playerCarPrefab; //inputProcessor car
    public GameObject ghostCarPrefab; //input player car
    public Camera mainCamera;

    public Transform pos1;
    public Transform pos2;
    public FinishTrigger finishTrigger;
    public UIManager uIManager;

    [SerializeField]
    private InputRecorder inputRecorder;
    [SerializeField]
    private InputReplay ghostInputPlayer;

    private int round = 1;

    private bool playerFinished = false;
    private bool ghostFinished = false;

    private GameObject player;
    private GameObject ghost;

    private GameObject winnerGO;
    public IEnumerator RoundsCoroutine()
    {
        //ROUND 1
        uIManager.ShowPreRaceText("RACE RECORD", 1.5f);
        yield return new WaitForSeconds(1.5f);

        uIManager.SetupCountdown(3);
        yield return uIManager.CountdownCoroutine(3);

        StartRoundOne();
        yield return new WaitUntil(() => playerFinished);
        ShowWinner();
        yield return new WaitForSeconds(2.0f);
        uIManager.roundResultText.gameObject.SetActive(false);
        SceneReset();


        //ROUND 2 // TODO add softlock protection
        uIManager.ShowPreRaceText("GHOST RACE", 1.5f);
        yield return new WaitForSeconds(1.5f);

        uIManager.SetupCountdown(3);
        yield return uIManager.CountdownCoroutine(3);

        StartRoundTwo();
        yield return new WaitUntil(() => playerFinished && ghostFinished);
        ShowWinner();
        yield return new WaitForSeconds(4f);
        QuitScene();
    }
    void StartRoundOne()
    {
        round = 1;
        playerFinished = false;

        player = Instantiate(playerCarPrefab, pos1.position, pos1.rotation);

        inputRecorder = player.AddComponent<InputRecorder>();
        var inputProcessor = player.GetComponent<InputProcessor>();
        inputRecorder.Initialize(inputProcessor);

        finishTrigger.OnTriggered += OnPlayerFinish;
        Debug.Log("round one started");
    }
    void StartRoundTwo()
    {
        round = 2;
        playerFinished = false;
        ghostFinished = false;

        player = Instantiate(playerCarPrefab, pos2.position, pos2.rotation);
        ghost = Instantiate(ghostCarPrefab, pos1.position, pos1.rotation);

        ghostInputPlayer = ghost.GetComponent<InputReplay>();
        ghostInputPlayer.SetReplay(inputRecorder.RecordedInput);

        finishTrigger.OnTriggered += OnAnyFinish;
    }
    private void OnPlayerFinish(GameObject car)
    {
        Debug.Log(car.name);
        if (car.CompareTag("Player"))
        {
            playerFinished = true;
            finishTrigger.OnTriggered -= OnPlayerFinish;
        }
    }

    private void OnAnyFinish(GameObject car)
    {
        if (winnerGO == null)
        {
            winnerGO = car;
        }
        if (car.CompareTag("Player"))
            {
                playerFinished = true;
            }
            else if (car.CompareTag("Ghost"))
            {
                ghostFinished = true;
            }

        if (playerFinished && ghostFinished)
        {
            finishTrigger.OnTriggered -= OnAnyFinish;
        }
    }

    private void ShowWinner()
    {
        if (round == 1)
        {
            uIManager.DisplayRoundResult(false, "notimportant");
        }
        else if (round == 2)
        {
            string winner = winnerGO.CompareTag("Player") ? "player" :
                            winnerGO.CompareTag("Ghost") ? "ghost" :
                            "draw";
            Debug.Log("winner: " + winner);
            uIManager.DisplayRoundResult(true, winner);
        }
    }
    private void SceneReset()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
        }
        if (ghost != null)
        {
            Destroy(ghost.gameObject);
        }
        mainCamera.transform.position = new Vector3(0, 1.7f, -4f);
        mainCamera.transform.rotation = Quaternion.identity;
    }

    public void QuitScene()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL
                SceneManager.LoadScene("gtfo");
        #else
            Application.Quit();
        #endif
    }
    private void Start()
    {
        StartCoroutine(RoundsCoroutine());
    }
}
