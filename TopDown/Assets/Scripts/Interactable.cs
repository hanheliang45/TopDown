using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material highlightMaterial;
    private Material defaultMaterial;
    
    public void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = this.GetComponent<MeshRenderer>();
        }

        defaultMaterial = meshRenderer.material;
    }

    public virtual void Interact(PlayerInteraction interaction)
    {
        
    }

    public void HighlightActive(bool isActive)
    {
        meshRenderer.material = isActive ? highlightMaterial : defaultMaterial;
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
        if (interaction != null)
        {
            interaction.AddInteractable(this);
            interaction.UploadClosestInteractable();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
        if (interaction != null)
        {
            interaction.RemoveInteractable(this);
            interaction.UploadClosestInteractable();
        }
    }
}
