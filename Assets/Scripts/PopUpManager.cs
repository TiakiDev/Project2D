using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
    public GameObject popUpPrefab;
    public float popUpDuration = 1f;
    public int maxPopUps = 5;
    private readonly Queue<GameObject> activePopUps = new();
    
    public static PopUpManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ShowPopUp(string text, Sprite icon)
    {
        GameObject newPopUp = Instantiate(popUpPrefab, transform);

        TMP_Text itemText = newPopUp.GetComponentInChildren<TMP_Text>();
        Image itemImage = newPopUp.transform.Find("ItemIcon").GetComponent<Image>();
        
        itemImage.sprite = icon;
        itemText.text = text;

        activePopUps.Enqueue(newPopUp);
        if (activePopUps.Count > maxPopUps)
        {
            Destroy(activePopUps.Dequeue());
        }
        
        StartCoroutine(FadeOutAndDestroy(newPopUp));
    }

    private IEnumerator FadeOutAndDestroy(GameObject popUp)
    {
        yield return new WaitForSeconds(popUpDuration);
        if (popUp == null)
        {
            yield break;
        }
        CanvasGroup canvasGroup = popUp.GetComponent<CanvasGroup>();
        for(float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popUp == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }
        Destroy(popUp);
    }
}
