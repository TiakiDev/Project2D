using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ArtifactSlot : MonoBehaviour
{
    private ItemSlot itemSlot;
    public bool hasArtifact;
    
    private int currentStatValue = 0;
    private ItemSO.StatType currentStatType = ItemSO.StatType.None;

    private void Start()
    {
        itemSlot = GetComponent<ItemSlot>();
    }

    private void Update()
    {
        if (itemSlot != null && itemSlot.itemSO != null && itemSlot.itemSO.itemType == ItemSO.ItemType.Artifact)
        {
            if (!hasArtifact) // Dodano artefakt
            {
                currentStatValue = itemSlot.itemSO.statValue;
                currentStatType = itemSlot.itemSO.statToChange;
    
                if (StatsManager.instance != null)
                {
                    switch (currentStatType)
                    {
                        case ItemSO.StatType.Speed:
                            StatsManager.instance.speed += currentStatValue;
                            break;
                        case ItemSO.StatType.Health:
                            StatsManager.instance.maxHealth += currentStatValue;
                            break;
                        default:
                            Debug.LogWarning("Stat type not handled");
                            break;
                    }
                    hasArtifact = true;
                    Debug.Log("Artifact added. " + currentStatType + " increased by " + currentStatValue);
                    StatsManager.instance.UpdateUI();
                }
            }
        }
        else
        {
            if (hasArtifact) // Usunięto artefakt
            {
                if (StatsManager.instance != null)
                {
                    // Odejmowanie statystyki na podstawie zapisanych wartości
                    switch (currentStatType)
                    {
                        case ItemSO.StatType.Speed:
                            StatsManager.instance.speed -= currentStatValue;
                            break;
                        case ItemSO.StatType.Health:
                            StatsManager.instance.maxHealth -= currentStatValue;
                            break;
                        default:
                            Debug.LogWarning("Stat type not handled");
                            break;
                    }
                    // Resetowanie zmiennych po usunięciu artefaktu
                    currentStatValue = 0;
                    currentStatType = ItemSO.StatType.None;
    
                    hasArtifact = false;
                    StatsManager.instance.UpdateUI();
                    Debug.Log("Artifact removed. " + currentStatType + " decreased by " + currentStatValue);
                }
            }
        }
    }
}