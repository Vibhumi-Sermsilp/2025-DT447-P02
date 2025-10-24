using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 1f;
    [SerializeField] private LayerMask interactionLayerMask;

    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TextMeshProUGUI interactionText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetInteractable()?.Interact(this);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetInteractable()?.AltInteract(this);
        }

        if (GetInteractable() != null)
        {
            ShowUI(GetInteractable());
        }
        else
        {
            HideUI();
        }
    }

    private Interactable GetInteractable()
    {
        List<Interactable> interactables = new List<Interactable>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactionLayerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                interactables.Add(interactable);
            }
        }

        Interactable closest = null;
        foreach (Interactable interactable in interactables)
        {
            if (closest == null)
                closest = interactable;
            else
            {
                if (Vector3.Distance(transform.position, interactable.transform.position) <
                    Vector3.Distance(transform.position, closest.transform.position))
                {
                    closest = interactable;
                }
            }
        }

        return closest;
    }

    private void ShowUI(Interactable interactable)
    {
        if (interactable.GetInstructionText() == null)
        {
            HideUI();
            return;
        }

        interactionText.SetText(interactable.GetInstructionText());
        interactionUI.SetActive(true);
    }

    private void HideUI()
    {
        interactionUI.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
#endif
}
