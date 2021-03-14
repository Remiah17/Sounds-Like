using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignModeManager : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField]
    private string OfflineModeScene;
    [Header("SCREENS")]
    [SerializeField]
    private GameObject SettingScreen;
    [SerializeField]
    private GameObject LevelScreen;
    private SoundsScript theSoundScript;
    [SerializeField]
    private Text CoinText;
    [SerializeField]
    private GameObject[] CoinsInButton;
    private int CoinUnlocked;
    [Header("LEVEL BUTTONS")]
    [SerializeField]
    private Button[] LevelButtons;
    [SerializeField]
    private GameObject[] LevelDoneMark;
    private int LevelUnlocked;
    private int CurrentLevel;
    [Header("SCORE")]
    [SerializeField]
    private Text ScoreText;
    private bool LevelChecked;
    [Header("LEVEL SCREEN")]
    [SerializeField]
    private string[] PossibleAnswers;
    [SerializeField]
    private Text CurrentLevelText;
    [SerializeField]
    private InputField AnswerInput;
    [SerializeField]
    private GameObject RightScreen;
    [SerializeField]
    private GameObject WrongScreen;
    [SerializeField]
    private Text YouGotAmountPointsText;
    private int PointsToGive;
    [SerializeField]
    private CampaignItemList FirstLevelItems;
    [SerializeField]
    private AvatarDatabaseScriptableObject AvatarDatabase;
    [SerializeField]
    private GameObject[] AvatarInButtons;



    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        LevelChecked = false;
        LevelUnlocked = PlayerPrefs.GetInt("CampaignLevelUnlocked");
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelUnlocked != PlayerPrefs.GetInt("CampaignLevelUnlocked"))
        {
            LevelUnlocked = PlayerPrefs.GetInt("CampaignLevelUnlocked");
            LevelChecked = false;
        }
        ScoreText.text = Mathf.Round(PlayerPrefs.GetInt("Score")).ToString();
        if (!LevelChecked)
        {
            for (int i = 0; i < LevelButtons.Length; i++)
            {
                if (LevelUnlocked != i)
                {
                    LevelButtons[i].interactable = false;
                }
                else
                {
                    LevelButtons[i].interactable = true;
                }

                switch (i)
                {
                    case 14:
                        if (i >= LevelUnlocked - 2)
                        {
                            AvatarInButtons[0].SetActive(true);
                        }
                        else
                        {
                            AvatarInButtons[0].SetActive(false);
                        }
                        break;
                    case 24:
                        if (i >= LevelUnlocked - 2)
                        {
                            AvatarInButtons[1].SetActive(true);
                        }
                        else
                        {
                            AvatarInButtons[1].SetActive(false);
                        }
                        break;
                    case 34:
                        if (i >= LevelUnlocked - 2)
                        {
                            AvatarInButtons[2].SetActive(true);
                        }
                        else
                        {
                            AvatarInButtons[2].SetActive(false);
                        }
                        break;
                }
            }
            for (int i = 0; i < LevelDoneMark.Length; i++)
            {
                if (LevelUnlocked < i + 1)
                {
                    LevelDoneMark[i].SetActive(false);
                }
                else
                {
                    LevelDoneMark[i].SetActive(true);
                }
            }
            CoinUnlocked = PlayerPrefs.GetInt("CampaignCoinUnlocked");
            for (int i = 1; i < CoinsInButton.Length; i++)
            {
                if(i > CoinUnlocked)
                {
                    CoinsInButton[i].SetActive(true);
                }
                else
                {
                    CoinsInButton[i].SetActive(false);
                }
            }
            CoinText.text = "" + PlayerPrefs.GetInt("Coins");
            LevelChecked = true;
        }

        if(LevelScreen.activeInHierarchy) {
            CurrentLevelText.text = "LEVEL " + CurrentLevel;
        }
    }

    public void OnClickLevel(int LevelNumber)
    {
        theSoundScript.PlaySelectSound();
        CurrentLevel = LevelNumber;
        SetupLevel(LevelNumber);
        LevelScreen.SetActive(true);
    }
    public void Close_LevelScreen()
    {
        theSoundScript.PlaySelectSound();
        HideLevel();
    }
    public void PlayLevelSound()
    {
        //play sound
        theSoundScript.PlaySpecificFX(FirstLevelItems.LevelItemList[CurrentLevel - 1].ItemSound);
    }
    public void On_ClickAnswer()
    {
        theSoundScript.PlaySelectSound();
        int rightaswer = 0;
        //play sound base on current level
        for (int i = 0; i < PossibleAnswers.Length; i++)
        {
            if (AnswerInput.text == PossibleAnswers[i])
            {
                rightaswer += 1;
            }
        }

        if (rightaswer > 0)
        {
            //right aswer
            theSoundScript.PlayCorrectSound();
            int givingscore = PlayerPrefs.GetInt("Score") + PointsToGive;
            PlayerPrefs.SetInt("CampaignLevelUnlocked", PlayerPrefs.GetInt("CampaignLevelUnlocked") + 1);
            PlayerPrefs.SetInt("Score", givingscore);
            RightScreen.SetActive(true);
          
            if(CurrentLevel == 10 || CurrentLevel == 20 || CurrentLevel == 30 || CurrentLevel == 40)
            {
                YouGotAmountPointsText.text = "You got " + Mathf.Round(PointsToGive) + " points and 10 coins";
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 10);
                PlayerPrefs.SetInt("CampaignCoinUnlocked", PlayerPrefs.GetInt("CampaignCoinUnlocked") + 1);
            }
            else if (CurrentLevel == 15 || CurrentLevel == 25 || CurrentLevel == 35)
            {
                switch(CurrentLevel)
                {
                    case 15:
                        AvatarDatabase.EyesLocked[7] = false;
                        YouGotAmountPointsText.text = "You got " + Mathf.Round(PointsToGive) + " points and unlocked Avatar Eyes # 8";
                        break;
                    case 25:
                        AvatarDatabase.NoseLocked[4] = false;
                        YouGotAmountPointsText.text = "You got " + Mathf.Round(PointsToGive) + " points and unlockedv Avatar Nose # 5";
                        break;
                    case 35:
                        AvatarDatabase.MouthLocked[4] = false;
                        YouGotAmountPointsText.text = "You got " + Mathf.Round(PointsToGive) + " points and unlocked Avatar Mouth # 5";
                        break;
                }
            }
            else
            {
                YouGotAmountPointsText.text = "You got " + Mathf.Round(PointsToGive) + " points!";
            }
        }
        else
        {
            //wronganswer
            theSoundScript.PlayWrongSound();
            WrongScreen.SetActive(true);
        }
    }
    public void OnClick_CloseRightScreen(){
        theSoundScript.PlaySelectSound();
        RightScreen.SetActive(false);
        Close_LevelScreen();
    }
    public void OnClick_CloseWrongScreen(){
        theSoundScript.PlaySelectSound();
        WrongScreen.SetActive(false);
        Close_LevelScreen();
    }
    public void OnClick_RetryLevel() {
        theSoundScript.PlaySelectSound();
        WrongScreen.SetActive(false);
        LevelScreen.SetActive(false);
        LevelChecked = false;
        OnClickLevel(CurrentLevel);
    }
    public void OnClickBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(OfflineModeScene);
    }
    public void OnClickSetting()
    {
        theSoundScript.PlaySelectSound();
        if (SettingScreen.activeInHierarchy)
        {
            LevelChecked = false;
            SettingScreen.SetActive(false);
        }
        else
        {
            SettingScreen.SetActive(true);
        }
    }
    private void HideLevel()
    {
        CurrentLevel = 0;
        LevelScreen.SetActive(false);
        LevelChecked = false;
    }
    private void SetupLevel(int level)
    {
        for (int i = 0; i < PossibleAnswers.Length; i++)
        {
            PossibleAnswers[i] = FirstLevelItems.LevelItemList[level - 1].PossibleAnswers[i];
        }
        PointsToGive = FirstLevelItems.LevelItemList[level - 1].PointsToGive;
        AnswerInput.text = null;
    }

}
