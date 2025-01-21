using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [Header("General")]
    public string itemName;
    public Sprite itemIcon;
    public int maxStackSize;
    [TextArea (3, 10)]
    public string description;


    public enum ItemType
    {
        Weapon,
        Consumable,
        Artifact,
    }
    [Header("Item types")]
    public ItemType itemType;
    
    // Statystyki
    public enum StatType
    {
        None,
        Health,
        Attack,
        Speed,
        Defense
    }
    [Header("Stat changes")]
    public StatType statToChange; // Możliwość wyboru statystyki do zmiany

    // Dodanie wartości dla każdej zmiany statystyki
    public int statValue;
    
    [Header("Vending Machine")]
    public int vendingPrice;
}
