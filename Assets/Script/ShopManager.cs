using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("BUTTONS")]
    [SerializeField]
    private GameObject LoginButton;
    private int Currency;
    [Header("SCREENS")]
    [SerializeField]
    private GameObject ShopScreen;
    [SerializeField]
    private GameObject CoinPackScreen;
    [SerializeField]
    private GameObject SoundCardScreen;
    [SerializeField]
    private GameObject AvatarScreen;
    [SerializeField]
    private GameObject EnterCodeScreen;
    [SerializeField]
    private InputField CodeInputField;
    [SerializeField]
    private GameObject ConfirmPaymentScreen;
    private int ProductBuying = 0;
    [Header("TEXTS")]
    [SerializeField]
    private Text CoinText;
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text WarningMessageText;
    private SoundsScript theSoundScript;
    [SerializeField]
    private GameObject PuchasingItemScreen;
    [SerializeField]
    private Text PurchasingItemMessageText;
    [SerializeField]
    private Text ConfirmPaymentText;
    [SerializeField]
    private AvatarDatabaseScriptableObject AvatarDatabase;
    [SerializeField]
    private Button[] AvatarBuyButtons;
    [SerializeField]
    private LoginScript theLoginScript;


    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }
    void Update()
    {
        if(ShopScreen.activeInHierarchy)
        {
            CoinText.text = "" + PlayerPrefs.GetInt("Coins");
            ScoreText.text = "Score: " + PlayerPrefs.GetInt("Score");
        }
    }
    public void OnClick_Shop()
    {
        theSoundScript.PlaySelectSound();
        if (ShopScreen.activeInHierarchy)
        {
            ShopScreen.SetActive(false);
            LoginButton.SetActive(true);
        }
        else
        {
            ShopScreen.SetActive(true);
            OnClick_CoinPacks();
            LoginButton.SetActive(false);
        }
    }
    public void OnClick_CoinPacks()
    {
        theSoundScript.PlaySelectSound(); 
        CoinPackScreen.SetActive(true);
        SoundCardScreen.SetActive(false);
        AvatarScreen.SetActive(false);
    }
    public void OnClick_SoundCards()
    {
        theSoundScript.PlaySelectSound();
        CoinPackScreen.SetActive(false);
        SoundCardScreen.SetActive(true);
        AvatarScreen.SetActive(false);
    }
    public void OnClick_Avatar()
    {
        theSoundScript.PlaySelectSound();
        CoinPackScreen.SetActive(false);
        SoundCardScreen.SetActive(false);
        AvatarScreen.SetActive(true);
        for (int i = 0; i < AvatarBuyButtons.Length; i++)
        {
            int check = i + 1;
            switch(check)
            {
                case 1:
                    if (AvatarDatabase.EyesLocked[4])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
                case 2:
                    if (AvatarDatabase.EyesLocked[5])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
                case 3:
                    if (AvatarDatabase.EyesLocked[6])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
                case 4:
                    if (AvatarDatabase.NoseLocked[5])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
                case 5:
                    if (AvatarDatabase.NoseLocked[7])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
                case 6:
                    if (AvatarDatabase.NoseLocked[9])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
                case 7:
                    if (AvatarDatabase.MouthLocked[8])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
                case 8:
                    if (AvatarDatabase.MouthLocked[5])
                    {
                        AvatarBuyButtons[i].interactable = true;
                    }
                    else
                    {
                        AvatarBuyButtons[i].interactable = false;
                    }
                    break;
            }
        }
    }
    public void ShowWarningMessage(string message, Color textcolor)
    {
        StartCoroutine(ShowWarningMessageSequence(message, textcolor));
    }
    public void ShowConfirmPayment(int ProductID, float price, int currency)
    {
        theSoundScript.PlaySelectSound();
        Currency = currency;
        ConfirmPaymentScreen.SetActive(true);
        switch(currency)
        {
            case 1:
                ConfirmPaymentText.text = "You are spending $" + price + " . Click confirm to continue.";
                ProductBuying = ProductID;
                break;
            case 2:
                ConfirmPaymentText.text = "You are spending " + price + " points. Click confirm to continue.";
                break;
            case 3:
                ConfirmPaymentText.text = "You are spending " + price + " coins. Click confirm to continue.";
                break;
        }
    }
    public void HideConfirmPayment()
    {
        theSoundScript.PlaySelectSound();
        ConfirmPaymentText.text = "";
        ProductBuying = 0;
        Currency = 0;
        ConfirmPaymentScreen.SetActive(false);
    }
    public void ConfirmPayment()
    {
        theSoundScript.PlaySelectSound();
        switch (Currency)
        {
            case 1:
                switch (ProductBuying)
                {
                    case 50:
                        IAPManager.instance.ConfirmedCoin50();
                        break;
                    case 300:
                        IAPManager.instance.ConfirmedCoin300();
                        break;
                    case 500:
                        IAPManager.instance.ConfirmedCoin500();
                        break;
                    case 1000:
                        IAPManager.instance.ConfirmedCoin1000();
                        break;
                }
                break;
            case 2:
                int price = 0;
                switch (ProductBuying)
                {
                    case 50:
                        price = 5000;
                        break;
                    case 300:
                        price = 7500;
                        break;
                    case 500:
                        price = 12500;
                        break;
                    case 1000:
                        price = 25000;
                        break;
                }
                if (PlayerPrefs.GetInt("Score") >= price)
                {
                    PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") - price);
                    PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + ProductBuying);

                    StartCoroutine(ShowWarningMessageSequence("Success. You got " + ProductBuying + " coins.", Color.white));
                }
                else
                {
                    StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough score. ", Color.red));
                }
                break;
            case 3:
                int price2 = 0;
                switch (ProductBuying)
                {
                    case 1:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.EyesLocked[4] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR EYES # 5.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                    case 2:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.EyesLocked[5] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR EYES # 6.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                    case 3:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.EyesLocked[6] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR EYES # 7.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                    case 4:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.NoseLocked[5] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR NOSE # 6.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                    case 5:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.NoseLocked[7] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR NOSE # 8.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                    case 6:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.NoseLocked[9] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR NOSE # 10.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                    case 7:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.MouthLocked[8] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR MOUTH # 9.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                    case 8:
                        price2 = 20;
                        if (PlayerPrefs.GetInt("Coins") >= price2)
                        {
                            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price2);
                            AvatarDatabase.MouthLocked[5] = false;
                            StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR MOUTH # 6.", Color.white));
                        }
                        else
                        {
                            StartCoroutine(ShowWarningMessageSequence("Purchase Failed. You do not have enough coins. ", Color.red));
                        }
                        break;
                }
                break;
        }
    }
    public void ShowPurchasingItemScreen(string message, Color textcolor)
    {
        PuchasingItemScreen.SetActive(true);
        PurchasingItemMessageText.color = textcolor;
        PurchasingItemMessageText.text = message;
    }
    public void HideShowPurchasingItemScreen()
    {
        PuchasingItemScreen.SetActive(false);
        PurchasingItemMessageText.color = Color.white;
        PurchasingItemMessageText.text = "";
    }
    public void PurchasedWithMoney(string message, Color textcolor)
    {
        StartCoroutine(ShowWarningMessageSequence(message, textcolor));
    }
    public void OnClick_BuyCoin(int value)
    {
        theSoundScript.PlaySelectSound();
        int price = 0;
        ProductBuying = value;
        switch (value)
        {
            case 50:
                price = 5000;
                break;
            case 300:
                price = 7500;
                break;
            case 500:
                price = 12500;
                break;
            case 1000:
                price = 25000;
                break;
        }
        ShowConfirmPayment(value, price,2);
    }
    public void OnClick_BuyAvatar(int ID)
    {
        theSoundScript.PlaySelectSound();
        int price = 0;
        ProductBuying = ID;
        switch (ID)
        {
            case 1:
                price = 20;
                break;
            case 2:
                price = 20;
                break;
            case 3:
                price = 20;
                break;
            case 4:
                price = 20;
                break;
            case 5:
                price = 20;
                break;
            case 6:
                price = 20;
                break;
            case 7:
                price = 20;
                break;
            case 8:
                price = 20;
                break;
        }
        ShowConfirmPayment(ID, price, 3);
    }
    public void OnClick_EnterCode()
    {
        theSoundScript.PlaySelectSound();
        if (EnterCodeScreen.activeInHierarchy)
        {
            EnterCodeScreen.SetActive(false);
            CodeInputField.text = "";
        }
        else
        {
            EnterCodeScreen.SetActive(true);
            CodeInputField.text = "";
        }
    }
    public void Confirm_Code()
    {
        theSoundScript.PlaySelectSound();
        StartCoroutine(CheckCode(CodeInputField.text));
    }
    IEnumerator ShowWarningMessageSequence(string message, Color textcolor)
    {
        ShowPurchasingItemScreen(message, textcolor);
        yield return new WaitForSeconds(1.5f);
        HideShowPurchasingItemScreen();
        HideConfirmPayment();
        EnterCodeScreen.SetActive(false);
    }
    IEnumerator CheckCode(string Code)
    {
        ShowPurchasingItemScreen("Checking your code.", Color.white);

        var ATask = theLoginScript.DBreference.Child("Main").Child("InAppCodes").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
            ShowWarningMessage("Checking user name failed.",Color.red);
        }
        else
        {
            DataSnapshot codesSnapshot = ATask.Result;

            int keyFound = 0;
            int IDValueFound = 0;

            foreach (DataSnapshot childSnapshot in codesSnapshot.Children.Reverse<DataSnapshot>())
            {
                if(Code == childSnapshot.Key)
                {
                    keyFound += 1;
                    IDValueFound =int.Parse(childSnapshot.Value.ToString());
                }
            }

            if(keyFound > 0)
            {
                switch(IDValueFound)
                {
                    case 0:
                        StartCoroutine(ShowWarningMessageSequence("This code has already been redeemed.", Color.red));
                        break;
                    case 1:
                        AvatarDatabase.EyesLocked[4] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR EYES # 5.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 2:
                        AvatarDatabase.EyesLocked[5] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR EYES # 6.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 3:
                        AvatarDatabase.EyesLocked[6] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR EYES # 7.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 4:
                        AvatarDatabase.NoseLocked[5] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR NOSE # 6.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 5:
                        AvatarDatabase.NoseLocked[7] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR NOSE # 8.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 6:
                        AvatarDatabase.NoseLocked[9] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR NOSE # 10.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 7:
                        AvatarDatabase.MouthLocked[8] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR MOUTH # 9.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 8:
                        AvatarDatabase.MouthLocked[5] = false;
                        StartCoroutine(ShowWarningMessageSequence("Success. You unlocked AVATAR MOUTH # 6.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 50:
                        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + IDValueFound);
                        StartCoroutine(ShowWarningMessageSequence("Success. You got " + IDValueFound + " coins.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 300:
                        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + IDValueFound);
                        StartCoroutine(ShowWarningMessageSequence("Success. You got " + IDValueFound + " coins.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 500:
                        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + IDValueFound);
                        StartCoroutine(ShowWarningMessageSequence("Success. You got " + IDValueFound + " coins.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 1000:
                        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + IDValueFound);
                        StartCoroutine(ShowWarningMessageSequence("Success. You got " + IDValueFound + " coins.", Color.white));
                        StartCoroutine(UpdateCode(Code));
                        break;
                    case 101:
                        //soundcard
                        break;
                    case 102:
                        //soundcard
                        break;
                    case 103:
                        //soundcard
                        break;
                    case 104:
                        //soundcard
                        break;
                    case 105:
                        //soundcard
                        break;
                    case 106:
                        //soundcard
                        break;
                    case 107:
                        //soundcard
                        break;
                    case 108:
                        //soundcard
                        break;
                    case 109:
                        //soundcard
                        break;
                    case 110:
                        //soundcard
                        break;
                }
            }
            else
            {
                StartCoroutine(ShowWarningMessageSequence("This code is invalid.", Color.red));
            }
        }
    }
    IEnumerator UpdateCode(string Code)
    {

        var ATask = theLoginScript.DBreference.Child("Main").Child("InAppCodes").Child(Code).SetValueAsync(0);

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {

        }
    }
}
