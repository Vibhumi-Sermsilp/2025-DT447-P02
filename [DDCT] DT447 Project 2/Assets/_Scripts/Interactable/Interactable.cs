using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool Active = true;

    [SerializeField] private string instructionText;

    [Header("Event")]
    [Space]
    public UnityEvent OnInteract;
    public UnityEvent OnAltInteract;

    public virtual void Interact(PlayerInteraction player)
    {
        if (!Active) return;
        OnInteract?.Invoke();
    }

    public virtual void AltInteract(PlayerInteraction player)
    {
        if (!Active) return;
        OnAltInteract?.Invoke();
    }

    public string GetInstructionText()
    {
        if (Active)
            return instructionText;
        else
            return null;
    }

    public void Toggle(bool toggle)
    {
        Active = toggle;
    }
}
