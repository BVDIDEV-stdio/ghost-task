using System;
using System.Collections.Generic;
using Ashsvp;
using UnityEngine;

public class InputReplay : MonoBehaviour, IVehicleInput // as I mentioned in InputProcessor: 
// this is a contract needed for SimCadeController supporting both InputProcessor and Replay 
// AND to integrate with those utilizing the very same logic
{
    public event Action<Vector2> OnMoveInput;
    public event Action<bool> OnHandBrakePressed;

    private List<InputEvent> replayEvents;
    private int currentIndex = 0;
    private float startTime;

    private Vector2 currentMove = Vector2.zero;
    private bool currentHandBrake = false;

    public void SetReplay(List<InputEvent> events)
    {
        replayEvents = events;
        currentIndex = 0;
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (replayEvents == null || currentIndex >= replayEvents.Count)
            return;
        float elapsedTime = Time.time - startTime;
        while (currentIndex < replayEvents.Count && replayEvents[currentIndex].timestamp <= elapsedTime)
        {
            var inputEvent = replayEvents[currentIndex];
            ApplyInputEvent(inputEvent);
            currentIndex++;
        }
    }

    private void ApplyInputEvent(InputEvent inputEvent)
    {
        switch (inputEvent.actionType)
        {
            case ActionType.Move:
                // as you remember typeof(val) is object (to provide versatility in terms of value storage (bool and vector2))
                currentMove = (Vector2)inputEvent.val; // could shove those values right into invoke braces but need to implement intrfc
                OnMoveInput?.Invoke(currentMove);
                break;
            case ActionType.HandBrake:
                currentHandBrake = (bool)inputEvent.val;
                OnHandBrakePressed?.Invoke(currentHandBrake);
                break;
        }
    }
}
