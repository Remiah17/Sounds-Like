using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftElement : MonoBehaviour
{
    public Text GiftNameText;
    public int GiftID;
    public Image GiftIcon;
    public Sprite[] ItemSprites;
    [SerializeField]
    private AvatarDatabaseScriptableObject AvatarDatabase;
    private SoundsScript theSoundScript;
    private GiftManager theGiftScript;

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theGiftScript = FindObjectOfType<GiftManager>().GetComponent<GiftManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theGiftScript = FindObjectOfType<GiftManager>().GetComponent<GiftManager>();
    }
   
    public void SetupGiftButton(int giftId)
    {
        GiftID = giftId;
        switch(giftId)
        {
            case 50:
                GiftNameText.text = "Coin 50";
                GiftIcon.sprite = ItemSprites[0];
                break;
            case 300:
                GiftNameText.text = "Coin 300";
                GiftIcon.sprite = ItemSprites[0];
                break;
            case 500:
                GiftNameText.text = "Coin 500";
                GiftIcon.sprite = ItemSprites[0];
                break;
            case 1000:
                GiftNameText.text = "Coin 1000";
                GiftIcon.sprite = ItemSprites[0];
                break;
            case 1:
                GiftNameText.text = "EYES # 5";
                GiftIcon.sprite = ItemSprites[1];
                break;
            case 2:
                GiftNameText.text = "EYES # 6";
                GiftIcon.sprite = ItemSprites[1];
                break;
            case 3:
                GiftNameText.text = "EYES # 7";
                GiftIcon.sprite = ItemSprites[1];
                break;
            case 4:
                GiftNameText.text = "NOSE # 6";
                GiftIcon.sprite = ItemSprites[2];
                break;
            case 5:
                GiftNameText.text = "NOSE # 8";
                GiftIcon.sprite = ItemSprites[2];
                break;
            case 6:
                GiftNameText.text = "NOSE # 10";
                GiftIcon.sprite = ItemSprites[2];
                break;
            case 7:
                GiftNameText.text = "MOUTH # 9";
                GiftIcon.sprite = ItemSprites[3];
                break;
            case 8:
                GiftNameText.text = "MOUTH # 6";
                GiftIcon.sprite = ItemSprites[3];
                break;
        }
    }

    public void OnClick_RedeemGift()
    {
        theSoundScript.PlaySelectSound();
        switch (GiftID)
        {
            case 50:
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + GiftID);
                break;
            case 300:
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + GiftID);
                break;
            case 500:
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + GiftID);
                break;
            case 1000:
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + GiftID);
                break;
            case 1:
                AvatarDatabase.EyesLocked[4] = false;
                break;
            case 2:
                AvatarDatabase.EyesLocked[5] = false;
                break;
            case 3:
                AvatarDatabase.EyesLocked[6] = false;
                break;
            case 4:
                AvatarDatabase.NoseLocked[5] = false;
                break;
            case 5:
                AvatarDatabase.NoseLocked[7] = false;
                break;
            case 6:
                AvatarDatabase.NoseLocked[9] = false;
                break;
            case 7:
                AvatarDatabase.MouthLocked[8] = false;
                break;
            case 8:
                AvatarDatabase.MouthLocked[5] = false;
                break;
        }
        StartCoroutine(theGiftScript.UpdateGifts(GiftID));
    }

    
}
