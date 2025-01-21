//************************
//* Author : Tiaki
//************************
using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    //For the drag and drop system
    private Transform originalParent;
    private GameObject dragIcon;
    
    //slot info
    public ItemSO itemSO;
    public String itemName;
    public int quantity;
    public Image image;
    public TMP_Text quantityText;
    
    public Sprite nullImage;
    
    public void AddItem(ItemSO item, int amount)
    {
        itemSO = item;
        quantity += amount;
        
        quantityText.text = quantity.ToString();
        image.sprite = item.itemIcon;
        
        UpdateQuantityText();
    }
    
    private void RemoveItem(int amount)
    {
        this.quantity -= amount;
        if (quantity <= 0)
        {
            ClearSlot();
            UpdateQuantityText();
        }
    }

    private void ClearSlot()
    {
        image.sprite = nullImage;
        itemSO = null;
        quantity = 0;
        UpdateQuantityText();
    }
    private void UpdateQuantityText()
    {
        if (quantity > 1)
        {
            quantityText.text = quantity.ToString();
        }
        else
        {
            quantityText.text = "";
        }
    }


    private void Update()
    {
        UpdateQuantityText();
        
        if (quantity <= 0)
        {
            ClearSlot();
        }
    }

    //*-----------------------
    //* Actions with items   |
    //*-----------------------
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && itemSO != null)
        {
            DropItem();
            ClearSlot();
            UpdateQuantityText();
        }
        else if (eventData.button == PointerEventData.InputButton.Left && itemSO != null && ItemSO.StatType.None != itemSO.statToChange && itemSO.itemType == ItemSO.ItemType.Consumable)
        {
            StatsManager.instance.ChangeStat(itemSO.statValue, itemSO.statToChange);
            RemoveItem(1);
            Destroy(dragIcon);
            if (quantity <= 0)
            {
                TooltipManager.instance.HideTooltip();
            }
        }
    }

    //*-----------------------
    //* Dropping item system |
    //*-----------------------
    private void DropItem()
    {
        
        GameObject itemToDrop;
        Item itemToDropScript;
        SpriteRenderer itemToDropSR;
        
        //szukanie gracza
        GameObject player = GameObject.Find("Player");
        GameObject parent = GameObject.Find("Items");

        // Dodaj przesunięcie
        float dropDistance = 1.8f; // Dystans przed graczem
        Vector3 lastMovement = PlayerMovement.instance.GetLastMovementDirection();
        Vector3 dropPosition = player.transform.position + lastMovement * dropDistance;

        //item do wyrzucenia to prefab z inventorymanager
        itemToDrop = InventoryManager.instance.itemPrefab;
        
        //pobieranie childa z prefaba
        Transform itemIcon = itemToDrop.transform.Find("Icon");
        
        //pobieranie komponentow
        itemToDropScript = itemToDrop.GetComponent<Item>();
        itemToDropSR = itemIcon.GetComponent<SpriteRenderer>();
        
        //modyfikacja wszystkich komponentow
        itemToDropScript.quantity = quantity;
        itemToDropScript.item = itemSO;
        itemToDropSR.sprite = itemSO.itemIcon;

        //spawnowanie przededmiotu
        GameObject newItem = Instantiate(itemToDrop, dropPosition, Quaternion.identity, parent.transform);
        newItem.name = itemSO.itemName + " [" + quantity + "]";
        
        Destroy(dragIcon);
        UpdateQuantityText();
    }
    
    //*-------------------
    //* Dragging system  |
    //*-------------------

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemSO == null) return;
        
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(transform.root);
        dragIcon.transform.SetAsLastSibling();
        Image dragImage = dragIcon.AddComponent<Image>();
        dragImage.sprite = image.sprite;
        dragImage.raycastTarget = false;
        
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            image.sprite = nullImage;
            quantityText.enabled = false; 
        }
        else if (eventData.button == PointerEventData.InputButton.Right && quantity == 1)
        {
            image.sprite = nullImage;
            quantityText.enabled = false; 
        }

    }
    
    public void OnDrag(PointerEventData eventData)
    {
        TooltipManager.instance.HideTooltip();
        
        if (dragIcon != null)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
        else
        {
            Debug.LogWarning("Drag icon is null.");
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            quantityText.enabled = true;
            if (itemSO == null) return;
            image.sprite = itemSO.itemIcon;
            UpdateQuantityText();

            if (dragIcon != null)
            {
                Destroy(dragIcon);
            }

            if (eventData.pointerEnter != null)
            {
                // Przechodzenie w górę hierarchii, aby znaleźć ItemSlot
                Transform current = eventData.pointerEnter.transform;
                while (current != null)
                {
                    if (current.TryGetComponent<ItemSlot>(out ItemSlot itemSlot))
                    {
                        
                        Debug.Log("Znaleziono slot: " + itemSlot.name);
                    
                        //*poniżej znajduje się cała logia dodania przedmiotu do slotu
                    
                        if (itemSlot == this)
                        {
                            Debug.Log("Przenoszony przedmiot na ten sam slot.");
                            return;
                        }   
                        
                        //sprawdzenie czy slot akceptuje ten przedmiot
                        if (itemSlot.TryGetComponent<ArtifactSlot>(out _) && itemSO.itemType != ItemSO.ItemType.Artifact)
                        {
                            Debug.LogWarning("Ten slot akceptuje tylko przedmioty typu Artifact!");
                            return;
                        }
                    
                        if (itemSlot.itemSO == null)
                        {
                            if (eventData.button == PointerEventData.InputButton.Left)
                            {
                                itemSlot.AddItem(itemSO, quantity);
                                ClearSlot();
                            }
                            if (eventData.button == PointerEventData.InputButton.Right)
                            {
                                if (quantity <= 1)
                                {
                                    itemSlot.AddItem(itemSO, quantity);
                                    ClearSlot();
                                    
                                }
                                else
                                {
                                    itemSlot.AddItem(itemSO, quantity / 2);
                                    quantity -= quantity / 2;
                                }
                                return;
                            }

                        }
                        if(itemSlot.itemSO != null && itemSlot.itemSO == itemSO)
                        {
                            itemSlot.AddItem(itemSO, quantity);
                            ClearSlot();
                            return;
                        }
                        if (itemSlot.itemSO != null && itemSlot.itemSO != itemSO)
                        {
                            SwapItems(itemSlot);
                            return;
                        }
                    
                    }
                    current = current.parent;
                }

                Debug.Log("Obiekt pod myszką nie jest slotem lub nie ma w hierarchii rodzica typu ItemSlot.");
            }
            else
            {
                Debug.Log("Brak obiektu pod myszką.");
            }
    }
    private void SwapItems(ItemSlot otherSlot)
    {
        if (itemSO == null || otherSlot.itemSO == null) return; // Sprawdzenie na istnienie przedmiotu w obu slotach

        // Kopiowanie danych do innego slotu
        ItemSO tempItem = itemSO;
        int tempQuantity = quantity;

        ClearSlot();  // Wyczyszczenie aktualnego slotu
        AddItem(otherSlot.itemSO, otherSlot.quantity);  // Dodanie przedmiotu z drugiego slotu

        otherSlot.ClearSlot();  // Wyczyszczenie drugiego slotu
        otherSlot.AddItem(tempItem, tempQuantity);  // Dodanie tymczasowych danych z pierwszego slotu

        UpdateQuantityText();
    }
}

