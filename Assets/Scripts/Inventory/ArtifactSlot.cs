using System;
using UnityEngine;

public class ArtifactSlot : MonoBehaviour
{
    private ItemSlot itemSlot;
    public bool hasArtifact;
    private float currentStatValue; // Aktualna wartość dodana do statystyki gracza

    private void Start()
    {
        itemSlot = GetComponent<ItemSlot>();
    }

    private void Update()
    {
        if (itemSlot.itemSO != null && itemSlot.itemSO.itemType == ItemSO.ItemType.Artifact)
        {
            if (!hasArtifact) // Dodano artefakt
            {
                currentStatValue = itemSlot.itemSO.statValue;
                PlayerMovement.instance.speed += currentStatValue;
                hasArtifact = true;
                Debug.Log("Artifact added. Speed increased by " + currentStatValue);
            }
        }
        else
        {
            if (hasArtifact) // Usunięto artefakt
            {
                PlayerMovement.instance.speed -= currentStatValue;
                hasArtifact = false;
                Debug.Log("Artifact removed. Speed decreased by " + currentStatValue);
                currentStatValue = 0; // Wyczyszczenie wartości po usunięciu artefaktu
            }
        }
    }
}