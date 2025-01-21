using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendingIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public ItemSO itemSO;

    private Image image;

    public int slotNumber;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = itemSO.itemIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        VendingTooltipManager.instance.ShowTooltip();
        if (itemSO != null)
        {
            VendingTooltipManager.instance.itemImage.sprite = itemSO.itemIcon;
            VendingTooltipManager.instance.itemName.text = itemSO.itemName;
            VendingTooltipManager.instance.itemCost.text = itemSO.vendingPrice.ToString();
            VendingTooltipManager.instance.itemSlotNumber.text = slotNumber.ToString();
        }
        else
        {
            Debug.Log("ItemSO is null");
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        VendingTooltipManager.instance.HideTooltip();
    }
}
