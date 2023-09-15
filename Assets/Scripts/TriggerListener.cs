using System;
using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    public event Action<Collider> onTriggerEntered;
    public event Action<Collider> onTriggerExited;
    
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEntered?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExited?.Invoke(other);
    }
}
