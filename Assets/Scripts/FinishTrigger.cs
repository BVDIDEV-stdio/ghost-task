using System;
using Ashsvp;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public event Action<GameObject> OnTriggered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("someone crossed the finish line: " + other.tag);
        if (other.CompareTag("Player") || other.CompareTag("Ghost"))
        {
            OnTriggered?.Invoke(other.gameObject);
            GameObject otherParent = other.gameObject.transform.parent.gameObject;
            otherParent.GetComponent<SimcadeVehicleController>().CanDrive = false;
        }
    }
}
