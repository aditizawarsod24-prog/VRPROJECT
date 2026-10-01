using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnTag : MonoBehaviour
{
    [Tooltip("Tag that the colliding object must have to fire the event")]
    [SerializeField] private string targetTag = "Player";

    [Tooltip("Event fired when a matching object enters the trigger")]
    public UnityEvent onTargetEnter;

    [Tooltip("Optional: fire only once, then disable further triggers")]
    [SerializeField] private bool triggerOnce = false;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && hasTriggered) return;

        if (other.CompareTag(targetTag))
        {
            hasTriggered = true;
            onTargetEnter.Invoke();
        }
    }
}