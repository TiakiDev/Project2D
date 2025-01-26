using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public GameObject itemPrefab;
    
    public GameObject inventoryMenu;
    public GameObject statsCanvas;
    public bool menuActivated;
    
    
    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public List<HotbarSlot> hotbarSlots = new List<HotbarSlot>();
    
    public void AddItem(ItemSO item, int amount = 1)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].itemSO == item)
            {
                itemSlots[i].AddItem(item, amount);
                PopUpManager.instance.ShowPopUp( "+" + amount + " " + item.itemName, item.itemIcon);
                return;
            }
        }
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].itemSO == null)
            {
                itemSlots[i].AddItem(item, amount);
                PopUpManager.instance.ShowPopUp( "+" + amount + " " + item.itemName, item.itemIcon);
                return;
            }
        }
    }
    
    public void DeselectAllSlots()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            hotbarSlots[i].selectedShader.SetActive(false);
            hotbarSlots[i].isSelected = false;
        }
    }

    private void Awake()
    {
        inventoryMenu.SetActive(false);
        menuActivated = false;
        
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    

    private void Start()
    {
        //* wybiera pierwszy slot na starcie gry aby zpobiec błędom
        //* idk moze potem wymyslisz lepsze rozwiązanie
        
        StartCoroutine(SelectSlot());
    }

    private IEnumerator SelectSlot()
    {
        yield return new WaitForSeconds(0.1f);
        hotbarSlots[0].SelectSlot();
    }


    
    


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !menuActivated)
        {
            inventoryMenu.SetActive(true);
            statsCanvas.SetActive(false);
            menuActivated = true;
            PlayerAttack.instance.canAttack = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape) && menuActivated)
        {
            inventoryMenu.SetActive(false);
            statsCanvas.SetActive(true);
            menuActivated = false;
            PlayerAttack.instance.canAttack = true;
            TooltipManager.instance.HideTooltip();
        }
        
        //pozwala przełączać się miedzy slotami używając 12345
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                hotbarSlots[i].OnPointerClick(null);
                break; // Opcjonalne, przerywa pętlę po znalezieniu pasującego klawisza
            }
        }

    }
    

}
