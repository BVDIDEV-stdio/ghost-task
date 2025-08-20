// public class InputRecorder : MonoBehaviour
// {
//     public struct InputFrame
//     {
//         public float timestamp;
//         public Vector2 move;
//         public bool handBrake;
//     }


//     public List<InputFrame> RecordedInput = new List<InputFrame>();

//     private float startTime;

//     private Vector2 currentMove = Vector2.zero;
//     private bool currentHandBrake = false;

//     private InputProcessor inputProcessor;
//     public void Initialize(InputProcessor processor)
//     {
//         inputProcessor = processor;

//         inputProcessor.OnMoveInput += OnMoveInput;
//         inputProcessor.OnHandBrakePressed += OnHandBrakePressed;
//     }

//     private void OnMoveInput(Vector2 move)
//     {
//         currentMove = move;
//     }
//     private void OnHandBrakePressed(bool isPressed)
//     {
//         currentHandBrake = isPressed;
//     }


//     private void OnDestroy()
//     {
//         if (inputProcessor != null)
//         {
//             inputProcessor.OnMoveInput -= OnMoveInput;
//             inputProcessor.OnHandBrakePressed -= OnHandBrakePressed;
//         }
//     }
//     void Start()
//     {
//         startTime = Time.time;
//     }

//     void Update() // naive approach poll listing
//     {
//         InputFrame frame = new InputFrame
//         {
//             timestamp = Time.time - startTime,
//             move = currentMove,
//             handBrake = currentHandBrake
//         };
//         RecordedInput.Add(frame);
//     }
// }
// preserved in case of foxtrot uniform
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionType {Move, HandBrake}
public struct InputEvent
{
    public float timestamp;
    public ActionType actionType;
    public object val; // to support both vector2 and bool
}

public class InputRecorder : MonoBehaviour
{
    private float startTime;
    public List<InputEvent> RecordedInput = new List<InputEvent>();

    // Входные переменные
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

    private void OnHandBrakePressed(bool pressed)
    {
        currentHandBrake = pressed;
    }

    private void Start()
    {
        startTime = Time.time;
    }
    // Allow me to explain:  
    // Adding input records in every `FixedUpdate` has proven to maintain more precise 
    // inputs for InputReplay to replay than the event-recording method.  
    
    // One might assume that event-triggered input recording would be sufficient if 
    // the controlled pawn (car) does not use a Rigidbody 
    // as Rigidbody physics is prone to error accumulation.  

    // That - however - is not my case.
    private void FixedUpdate()
    {
        InputEvent frame = new InputEvent
        {
            timestamp = Time.time - startTime,
            actionType = ActionType.Move,
            val = currentMove
        };
        RecordedInput.Add(frame);

        InputEvent brakeFrame = new InputEvent
        {
            timestamp = Time.time - startTime,
            actionType = ActionType.HandBrake,
            val = currentHandBrake
        };
        RecordedInput.Add(brakeFrame);
    }

    private void OnDestroy()
    {
        if (inputProcessor != null)
        {
            inputProcessor.OnMoveInput -= OnMoveInput;
            inputProcessor.OnHandBrakePressed -= OnHandBrakePressed;
        }
    }
}
