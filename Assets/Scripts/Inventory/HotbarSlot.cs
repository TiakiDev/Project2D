using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : MonoBehaviour, IPointerClickHandler
{
    private SpriteRenderer weaponSpriteSR;
    private ItemSlot itemSlot;
    public bool isSelected;
    
    public GameObject selectedShader;
    public GameObject weaponSprite;
    
    private void Start()
    {
        itemSlot = GetComponent<ItemSlot>();
        weaponSpriteSR = weaponSprite.GetComponent<SpriteRenderer>();
        
        selectedShader.SetActive(false);
        isSelected = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.instance.DeselectAllSlots();
        selectedShader.SetActive(true);
        isSelected = true;
    }

    private void Update()
    {
        if (isSelected && itemSlot.itemSO != null && itemSlot.itemSO.itemType == ItemSO.ItemType.Weapon)
        {
            weaponSprite.SetActive(true);
            weaponSpriteSR.sprite = itemSlot.itemSO.itemIcon;
            PlayerAttack.instance.canAttack = true;
        }
        
        if(isSelected && itemSlot.itemSO == null)
        {
            weaponSprite.SetActive(false);
            weaponSpriteSR.sprite = itemSlot.nullImage;
            PlayerAttack.instance.canAttack = false;
        }
    }
}
