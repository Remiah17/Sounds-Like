using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    private string Coin50 = "soundslike_coin_50";
    private string Coin300 = "soundslike_coin_300";
    private string Coin500 = "soundslike_coin_500";
    private string Coin1000 = "soundslike_coin_1000";

    private ShopManager TheShopManager;

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(Coin50, ProductType.Consumable);
        builder.AddProduct(Coin300, ProductType.Consumable);
        builder.AddProduct(Coin500, ProductType.Consumable);
        builder.AddProduct(Coin1000, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void BuyCoin50()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowConfirmPayment(50, 0.99f, 1);
    }
    public void ConfirmedCoin50()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowPurchasingItemScreen("Initializing purchase.", Color.white);
        BuyProductID(Coin50);
    }
    public void BuyCoin300()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowConfirmPayment(300, 2.99f, 1);
    }
    public void ConfirmedCoin300()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowPurchasingItemScreen("Initializing purchase.", Color.white);
        BuyProductID(Coin300);
    }
    public void BuyCoin500()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowConfirmPayment(500, 4.99f, 1);
    }
    public void ConfirmedCoin500()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowPurchasingItemScreen("Initializing purchase.", Color.white);
        BuyProductID(Coin500);
    }
    public void BuyCoin1000()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowConfirmPayment(1000, 9.99f, 1);
    }
    public void ConfirmedCoin1000()
    {
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.ShowPurchasingItemScreen("Initializing purchase.", Color.white);
        BuyProductID(Coin1000);
    }


    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, Coin50, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 50);
            TheShopManager.PurchasedWithMoney("Success! You got 50 coins.", Color.white);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Coin300, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 300);
            TheShopManager.PurchasedWithMoney("Success! You got 300 coins.", Color.white);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Coin500, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 500);
            TheShopManager.PurchasedWithMoney("Success! You got 500 coins.", Color.white);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, Coin1000, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1000);
            TheShopManager.PurchasedWithMoney("Success! You got 1000 coins.", Color.white);
        }
        else
        {
            TheShopManager.PurchasedWithMoney("Purchase Failed", Color.red);
        }
        return PurchaseProcessingResult.Complete;
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
                TheShopManager.PurchasedWithMoney("Purchase Failed", Color.red);
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
            TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
            TheShopManager.PurchasedWithMoney("Purchase Failed", Color.red);
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
            TheShopManager.ShowWarningMessage("Restore Failed", Color.red);
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
                TheShopManager.ShowWarningMessage("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.", Color.white);
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
            TheShopManager.ShowWarningMessage("Restore Failed", Color.red);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        TheShopManager = FindObjectOfType<ShopManager>().GetComponent<ShopManager>();
        TheShopManager.PurchasedWithMoney("Purchase Failed", Color.red);
    }
}