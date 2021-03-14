using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundCardButtonElement : MonoBehaviour
{
    public Text SoundCardNameText;
    public Text SoundCardIDNumberText;
    public int SoundCardID;
    public Image SoundCardImage;
    public Text SoundCardArtistText;
    public Image SoundCardDifficultyImage;
    public Sprite[] SoundCardDifficultySprites;
    public Text SoundCardPointsToGiveText;
    public Text Amount;
    public SoundCardDatabaseScriptableObject SoundCardDatabase;
    private SoundsScript theSoundScript;
    private EliminationGameManager TheEliminationManager;

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        TheEliminationManager = FindObjectOfType<EliminationGameManager>().GetComponent<EliminationGameManager>();
    }

    public void NewSoundCardElementButton(int _SCID)
    {
        SoundCardNameText.text = "Item: " + SoundCardDatabase.SoundCardList[_SCID].CampaignItemData.ItemName;
        SoundCardIDNumberText.text = SoundCardDatabase.SoundCardList[_SCID].SoundCardID.ToString();
        SoundCardID = _SCID;
        SoundCardImage.sprite = SoundCardDatabase.SoundCardList[_SCID].CampaignItemData.ItemSprite;
        SoundCardArtistText.text = "Music by " + SoundCardDatabase.SoundCardList[_SCID].ArtistName;
        SoundCardDifficultyImage.sprite = SoundCardDifficultySprites[SoundCardDatabase.SoundCardList[_SCID].Difficulty - 1];
        SoundCardPointsToGiveText.text = "Points: " + SoundCardDatabase.SoundCardList[_SCID].PointsToGive.ToString();
        int ShowingAmount = SoundCardDatabase.SoundCardAmount[_SCID] - SoundCardDatabase.AmountBeingUsed[_SCID];
        Amount.text = "" + ShowingAmount;
    }

    public void ChooseThisSoundCard()
    {
        Debug.Log("Selected " + SoundCardID);
        theSoundScript.PlaySelectSound();
        TheEliminationManager.Show_SelectSoundCard(SoundCardID);
    }
}
