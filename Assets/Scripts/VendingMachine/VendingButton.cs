using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VendingButton : MonoBehaviour
{
    public int slotNumber;
    public VendingMachine vendingMachine; // Referencja do automatu

    [SerializeField] TMP_Text slotNumberText;

    private void Start()
    {
        slotNumberText.text = slotNumber.ToString();
    }

    public void OnClick()
    {
        vendingMachine.BuyItem(slotNumber); // Użycie instancji automatu
    }
}

