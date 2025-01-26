using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text healthText;
    public Slider speedBar;
    public TMP_Text speedText;
    public TMP_Text coinsText;
    public List<TMP_Text> coinsTextList;
    
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float speed;

    public int coins;
    
    public static StatsManager instance;
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
        speed = PlayerMovement.instance.speed;
        UpdateUI();
    }
    
    public void ChangeStat(float amount, ItemSO.StatType statType)
    {
        switch (statType)
        {
            case ItemSO.StatType.Health:
                currentHealth += amount;
                break;
            case ItemSO.StatType.Speed:
                speed += amount;
                PlayerMovement.instance.speed = speed;
                break;
            case ItemSO.StatType.Defense:
                //tutaj se wpisujesz co się będzie działo kiedy użyje przedmiotu z tym typem statType
                break;
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthBar.value = currentHealth;
        healthBar.maxValue = maxHealth;
        healthText.text = currentHealth + "/" + maxHealth;
        
        speedBar.value = speed;
        speedText.text = speed.ToString();
        foreach (var text in coinsTextList)
        {
            text.text = coins.ToString();
        }
    }
}
