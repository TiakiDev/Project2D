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
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateUI();
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //jak ma max życia to nie leczy więcej
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
            default:
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
        coinsText.text = coins.ToString();
    }
}
