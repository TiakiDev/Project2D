using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    
    private bool canUse;
    private bool menuActivated;
    private int itemCount;
    private GameObject playerInRange;
    
    [Header("Properties")]
    [SerializeField] private int maxItems;
    [SerializeField] private int minItems;
    
    [Header("References")]
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject vendingCanvas;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject vendingItemsParent;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private GameObject outOfStockInfo;
    
    [Header("Lists")]
    public List<ItemSO> allItems = new List<ItemSO>();
    public List<GameObject> itemsInVendingMachine = new List<GameObject>();

    private void Start()
    {
        
        
        itemCount = Mathf.Clamp(UnityEngine.Random.Range(minItems, maxItems), 1, allItems.Count);


        for (int i = 0; i < itemCount; i++)
        {
            if (allItems.Count == 0) break; // Zakończ pętlę, jeśli lista stanie się pusta

            int randomIndex = UnityEngine.Random.Range(0, allItems.Count);
            ItemSO randomItem = allItems[randomIndex];
            GameObject item = Instantiate(itemPrefab, vendingItemsParent.transform);
            item.GetComponent<VendingIcon>().itemSO = randomItem;
            item.GetComponent<VendingIcon>().slotNumber = i + 1;
            allItems.Remove(randomItem); // Usuń wykorzystany element
            itemsInVendingMachine.Add(item);
        }

        for (int i = 0; i < 7; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonsParent.transform);
            VendingButton buttonScript = button.GetComponent<VendingButton>();
            buttonScript.slotNumber = i + 1;
            buttonScript.vendingMachine = this;

        }
        vendingCanvas.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = collision.gameObject; // Ustawiamy referencję do gracza
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject == playerInRange)
        {
            playerInRange = null; // Usuwamy referencję, gdy gracz opuszcza zasięg
            menuActivated = false;
            vendingCanvas.SetActive(false);
            VendingTooltipManager.instance.canvasGroup.alpha = 0;
        }
    }
    
    private void Update()
    {
        if (playerInRange != null && Input.GetKeyDown(KeyCode.E) && !menuActivated)
        {
            menuActivated = true;
            vendingCanvas.SetActive(true);
            VendingTooltipManager.instance.canvasGroup.alpha = 0;
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape) && menuActivated || Input.GetKeyDown(KeyCode.Tab) && menuActivated)
        {
            menuActivated = false;
            vendingCanvas.SetActive(false);
            if (VendingTooltipManager.instance != null)
            {
                VendingTooltipManager.instance.canvasGroup.alpha = 0;
            }
        }

        if (itemsInVendingMachine.All(GameObject => GameObject == null))
        {
            Debug.Log("Vending machine is empty.");
            outOfStockInfo.SetActive(true);
        }
    }

    public void BuyItem(int slotNumber)
    {
        int index = slotNumber - 1;
        if (index < 0 || index >= itemsInVendingMachine.Count || itemsInVendingMachine[index] == null)
        {
            return;
        }
    
        VendingIcon vendingIcon = itemsInVendingMachine[index].GetComponent<VendingIcon>();
        
        if (StatsManager.instance.coins >= vendingIcon.itemSO.vendingPrice)
        {
            StatsManager.instance.coins -= vendingIcon.itemSO.vendingPrice;
            StatsManager.instance.UpdateUI();
        
            // Usuwamy referencję w liście, zanim obiekt zostanie zniszczony
            itemsInVendingMachine[index] = null;
        
            // Zniszczenie obiektu
            Destroy(vendingIcon.gameObject);
            
            InventoryManager.instance.AddItem(vendingIcon.itemSO, 1);
            
            if (gridLayoutGroup.enabled)
            {
                gridLayoutGroup.enabled = false;
            }
        }
    }

    private void DestroyOtherVendingMachines()
    {
        VendingMachine[] vendingMachines = FindObjectsOfType<VendingMachine>();

        // Przejdź przez wszystkie znalezione maszyny vendingowe
        foreach (VendingMachine vendingMachine in vendingMachines)
        {
            // Sprawdź, czy maszyna vendingowa nie jest tą, która wywołuje ten kod
            if (vendingMachine != this)
            {
                // Zniszcz inną maszynę vendingową
                Destroy(vendingMachine.gameObject);
                Debug.Log("[VendingMachine] Zniszczono inną maszynę vendingową.");
            }
        }
    }
}
