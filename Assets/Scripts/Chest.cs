using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer itemRenderer;
    private ClickInfoDisplay clickInfoDisplay;
    
    private bool playerIsInChestRange;
    private bool isOpened;
    [SerializeField] private Sprite openedChest;
    public List<ItemSO> itemsInChest = new List<ItemSO>();

    [SerializeField] private GameObject chestTop;
    [SerializeField] private GameObject itemOut;
    [SerializeField] private GameObject openEffect;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemRenderer = itemOut.GetComponent<SpriteRenderer>();
        clickInfoDisplay = GetComponent<ClickInfoDisplay>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInChestRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsInChestRange = false;  
        }
    }

    private void Update()
    {
        if (playerIsInChestRange && !isOpened)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                int randomIndex = UnityEngine.Random.Range(0, itemsInChest.Count);
                int randomAmount = UnityEngine.Random.Range(1, 4);
                ItemSO randomItem = itemsInChest[randomIndex];
                
                InventoryManager.instance.AddItem(randomItem, randomAmount);
                
                isOpened = true;
                spriteRenderer.sprite = openedChest;
                itemRenderer.sprite = randomItem.itemIcon;
                
                chestTop.SetActive(true);
                itemOut.SetActive(true);
                openEffect.SetActive(true);
                Destroy(clickInfoDisplay.clickInfo);
                
            }
        }
    }
}

