using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int quantity = 1;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StatsManager.instance.coins += quantity;
            StatsManager.instance.UpdateUI();
            Destroy(gameObject);
        }
    }
}
