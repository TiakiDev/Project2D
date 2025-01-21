using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class VendingTooltipManager : MonoBehaviour
{
    public static VendingTooltipManager instance;
    
    private Vector3 lastMousePosition;
    
    public CanvasGroup canvasGroup;
    
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemCost;
    public TMP_Text itemSlotNumber;

    private void Awake()
    {
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
        canvasGroup = GetComponent<CanvasGroup>();
        Cursor.visible = true;
        canvasGroup.alpha = 0;
    }

   

    private void Update()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            transform.position = Input.mousePosition;
            lastMousePosition = Input.mousePosition;
        }
    }


    public void ShowTooltip()
    {
        canvasGroup.alpha = 1;
    }
    public void HideTooltip()
    {
        canvasGroup.alpha = 0;
    }
}
