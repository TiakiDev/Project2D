using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TooltipGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string tooltipName;
    private string tooltipDescription;
    private Sprite tooltipIcon;
    
    private ItemSlot itemSlot;
    private void Start()
    {
        itemSlot = GetComponent<ItemSlot>();
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlot.itemSO != null)
        {
            tooltipName = itemSlot.itemSO.itemName;
            tooltipDescription = itemSlot.itemSO.description;
            tooltipIcon = itemSlot.itemSO.itemIcon;
            TooltipManager.instance.ShowTooltip(tooltipName, tooltipIcon, tooltipDescription);
            TooltipManager.instance.openInfo.SetActive(false);
        }
        else
        {
            TooltipManager.instance.HideTooltip();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("mouse exit");
        TooltipManager.instance.HideTooltip();
    }
    
}


