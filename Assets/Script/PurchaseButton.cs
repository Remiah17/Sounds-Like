using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public enum PurchaseType { Coin50, Coin300, Coin500, Coin1000 };
    public PurchaseType purchaseType;
    private SoundsScript theSoundScript;

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    public void ClickPurchaseButton()
    {
        theSoundScript.PlaySelectSound();
        switch (purchaseType)
        {
            case PurchaseType.Coin50:
                IAPManager.instance.BuyCoin50();
                break;
            case PurchaseType.Coin300:
                IAPManager.instance.BuyCoin300();
                break;
            case PurchaseType.Coin500:
                IAPManager.instance.BuyCoin500();
                break;
            case PurchaseType.Coin1000:
                IAPManager.instance.BuyCoin1000();
                break;
        }
    }
}
