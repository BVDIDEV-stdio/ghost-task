using System.Collections.Generic;
using UnityEngine;

public class InputRecorder : MonoBehaviour
{
    public struct InputFrame
    {
        public float timestamp;
        public Vector2 move;
        public bool handBrake;
    }


    public List<InputFrame> RecordedInput { get; private set; } = new List<InputFrame>();

    private float startTime;

    private Vector2 currentMove = Vector2.zero;
    private bool currentHandBrake = false;

    private InputProcessor inputProcessor;
    public void Initialize(InputProcessor processor)
    {
        inputProcessor = processor;

        inputProcessor.OnMoveInput += OnMoveInput;
        inputProcessor.OnHandBrakePressed += OnHandBrakePressed;
    }

    private void OnMoveInput(Vector2 move)
    {
        currentMove = move;
    }
    private void OnHandBrakePressed(bool isPressed)
    {
        currentHandBrake = isPressed;
    }


    private void OnDestroy()
    {
        if (inputProcessor != null)
        {
            inputProcessor.OnMoveInput -= OnMoveInput;
            inputProcessor.OnHandBrakePressed -= OnHandBrakePressed;
        }
    }
    void Start()
    {
        startTime = Time.time;
    }

    void Update() // naive approach poll listing
    {
        InputFrame frame = new InputFrame
        {
            timestamp = Time.time - startTime,
            move = currentMove,
            handBrake = currentHandBrake
        };
        RecordedInput.Add(frame);
    }
}
