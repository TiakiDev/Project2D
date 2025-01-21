using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickInfoDisplay : MonoBehaviour
{
    [HideInInspector] public GameObject clickInfo;
    private TMP_Text infoText;
    
    [Header("Properties")]
    [SerializeField] private string infoString;
    [SerializeField] private float infoDisplayHeight;
    [SerializeField] private Vector2 infoScale;
    
    [Header("References")]
    [SerializeField] private GameObject prefab;
    
    private void Start()
    {
        clickInfo = Instantiate(prefab, transform.position + new Vector3(0, infoDisplayHeight, 0), Quaternion.identity, transform);

        if (infoScale != Vector2.zero)
        {
            clickInfo.transform.localScale = infoScale;
        }
        else
        {
            infoScale = new Vector2(1, 1);
        }
        clickInfo.SetActive(false);
        
        infoText = clickInfo.GetComponentInChildren<TMP_Text>();

        if (!string.IsNullOrEmpty(infoString))
        {
            infoText.text = infoString;
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && clickInfo != null)
        {
            clickInfo.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && clickInfo != null)
        {
            clickInfo.SetActive(false);
        }
    }
}
