using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{
    private GameObject inventorycanvas;
    private InventoryManager inventoryManager;
    
    public ItemSO item;

    public int quantity = 1;
    
    public Sprite icon;
    public string itemName;
    public string description;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        itemName = item.itemName;
        description = item.description;
        icon = item.itemIcon;
        
        Transform itemIcon = transform.Find("Icon");
        
        inventorycanvas = GameObject.Find("InventoryCanvas");
        spriteRenderer = itemIcon.GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component fron the childreen
        inventoryManager = inventorycanvas.GetComponent<InventoryManager>();
        
        spriteRenderer.sprite = icon;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inventoryManager.AddItem(item, quantity);
            Destroy(gameObject);
            TooltipManager.instance.HideTooltip();
        }
    }
}
