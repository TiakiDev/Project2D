using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;
    
    public TMP_Text tooltipName;
    public TMP_Text tooltipDescription;
    public Image tooltipIcon;
    public GameObject openInfo;
    
    void Awake()
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

    public void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    public void Update()
    {
        transform.position = Input.mousePosition;
    }
    
    public void ShowTooltip(string text, Sprite icon, string description)
    {
        tooltipName.text = text;
        tooltipDescription.text = description;
        tooltipIcon.sprite = icon;
        
        gameObject.SetActive(true);
    }
    
    public void HideTooltip()
    {
        tooltipName.text = String.Empty;
        tooltipDescription.text = String.Empty;
        
        gameObject.SetActive(false);
    }
}
