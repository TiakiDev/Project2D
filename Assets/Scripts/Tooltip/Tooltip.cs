using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private Item item;
    
    private string tooltipText;
    private string tooltipDescription;
    private Sprite icon;
    private void Start()
    {
        item = GetComponent<Item>();
    }
    

    private void OnMouseEnter()
    {
        tooltipText = item.itemName;
        tooltipDescription = item.description;
        icon = item.icon;
        
        TooltipManager.instance.ShowTooltip(tooltipText, icon, tooltipDescription);
        TooltipManager.instance.openInfo.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        TooltipManager.instance.HideTooltip();
    } 
}
