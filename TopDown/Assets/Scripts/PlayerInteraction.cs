using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactableList;
    private Interactable closestInteractable;
    void Start()
    {
        interactableList = new List<Interactable>();
    }

    public void Interact()
    {
        closestInteractable?.Interact(this);

        UploadClosestInteractable();
    }

    public void AddInteractable(Interactable interactable)
    {
        interactableList.Add(interactable);
    }

    public void RemoveInteractable(Interactable interactable)
    {
        interactableList.Remove(interactable);
    }

    public void UploadClosestInteractable()
    {
        closestInteractable?.HighlightActive(false);
        
        closestInteractable = null;
        float maxDistance = Mathf.Infinity;
        foreach (var interactable in this.interactableList)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < maxDistance)
            {
                maxDistance = distance;
                closestInteractable = interactable;
            }
        }
        closestInteractable?.HighlightActive(true);
    }
}
