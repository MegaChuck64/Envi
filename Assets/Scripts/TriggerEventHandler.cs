using UnityEngine;

public class TriggerEventHandler : MonoBehaviour
{
    public event System.Action<Collider2D> onTriggerEnterEvent;
    public event System.Action<Collider2D> onTriggerExitEvent;

    void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnterEvent?.Invoke(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        onTriggerExitEvent?.Invoke(other);
    }
        
}
