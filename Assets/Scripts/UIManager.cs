using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text preRaceText;
    public TMP_Text countdownText;
    public TMP_Text roundResultText;

    public void SetupCountdown(int seconds)
    {
        countdownText.gameObject.SetActive(true);
        roundResultText.gameObject.SetActive(false);
        StartCoroutine(CountdownCoroutine(seconds));
    }

    public IEnumerator CountdownCoroutine(int seconds)
    {
        for (int i = seconds; i > 0; --i)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }

    public void DisplayRoundResult(bool isCompetitiveRound, string result)
    {
        roundResultText.gameObject.SetActive(true);
        if (isCompetitiveRound)
        {
            if (result != "draw")
            roundResultText.text = result == "player" ? "YOU WON!" : "YOU LOST!";
        }
        else
        {
            roundResultText.text = "RACE RECORDED";
        }
    }
    public void ShowPreRaceText(string text, float duration)
    {
        StartCoroutine(PreRaceCoroutine(text, duration));
    }
    private IEnumerator PreRaceCoroutine(string text, float duration)
    {
        preRaceText.gameObject.SetActive(true);
        preRaceText.text = text;
        yield return new WaitForSeconds(duration);
        preRaceText.gameObject.SetActive(false);
    }
}
