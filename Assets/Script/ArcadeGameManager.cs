using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcadeGameManager : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField]
    private string ArcadeModeScene;
    [Header("SETTINGS SCREEN")]
    [SerializeField]
    private GameObject SettingScreen;
    [Header("GAME OVER SCREEN")]
    [SerializeField]
    private GameObject GameOverScreen;
    [Header("YOU WIN SCREEN")]
    [SerializeField]
    private GameObject YouWinScreen;
    [SerializeField]
    private GameObject GameStar1;
    [SerializeField]
    private GameObject GameStar2;
    [SerializeField]
    private GameObject GameStar3;
    private int StarGot;
    [Header("LOADING SCREEN")]
    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private Text LoadingMessageText;
    [Header("GAME SCREEN")]
    [SerializeField]
    private Text LevelNameText;
    private int Difficulty;
    private int CurrentLevel;
    private int CorrectAnswer;
    [Header("ITEM LIST DATA")]
    [SerializeField]
    private ArcadeItemLevel[] ArcadeItemLevelsInUse;
    [SerializeField]
    private ArcadeItemList[] AllItemList;
    [SerializeField]
    private GameObject[] ButtonList;
    [Header("MASCOT")]
    [SerializeField]
    private GameObject MascotMessageTab;
    [SerializeField]
    private Text MascotMessageText;
    [Header("SCORE")]
    [SerializeField]
    private Text ScoreText;
    private int PointsToGive;
    private int Score;
    [Header("HINT")]
    private float UpressedTime;
    [SerializeField]
    private GameObject PressMeHint;
    [Header("FEEDBACK SIGNS")]
    [SerializeField]
    private GameObject CorrectSign;
    [SerializeField]
    private GameObject WrongSign;
    [Header("TIMER")]
    [SerializeField]
    private Text TimerText;
    private bool TimerPaused;
    private float TimerCounter;
    [Header("LIFE")]
    [SerializeField]
    private GameObject[] LifeMeter;
    private int Life;
    private SoundsScript theSoundScript;

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theSoundScript.PlayBGM(2);
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        Difficulty = PlayerPrefs.GetInt("GuessItDifficulty");
        CurrentLevel = 1;
        SetupLevel(Random.Range(0, 5));
    }

    // Update is called once per frame
    void Update()
    {
        LevelNameText.text = "LEVEL " + (CurrentLevel);
        if(!TimerPaused)
        {
            if(TimerCounter > 0)
            {
                TimerCounter -= 1 * Time.deltaTime;
                TimerText.text = "" + Mathf.Round(TimerCounter);
            }
            else
            {
                TimerText.text = "" + 0;
                GameOverScreen.SetActive(true);
            }

            if (UpressedTime > 0)
            {
                UpressedTime -= 1 * Time.deltaTime;
            }
            else if (UpressedTime <= 0)
            {
                PressMeHint.SetActive(true);
            }
        }
       
    }

    public void OnClickBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(ArcadeModeScene);
    }
    public void OnClickSetting()
    {
        theSoundScript.PlaySelectSound();
        if (SettingScreen.activeInHierarchy)
        {
            SettingScreen.SetActive(false);
            TimerPaused = false;
        }
        else
        {
            SettingScreen.SetActive(true);
            TimerPaused = true;
        }
    }
    private void SetupLevel(int LevelListToUse)
    {
        TimerPaused = true;
        for (int i = 0; i < ArcadeItemLevelsInUse.Length; i++)
        {
            switch (Difficulty)
            {
                case 1:
                    ArcadeItemLevelsInUse[i] = AllItemList[LevelListToUse].ItemLevel[i];
                    break;
                case 2:
                    ArcadeItemLevelsInUse[i] = AllItemList[LevelListToUse].NormalItemLevel[i];
                    break;
                case 3:
                    ArcadeItemLevelsInUse[i] = AllItemList[LevelListToUse].HardItemLevel[i];
                    break;
                case 4:
                    ArcadeItemLevelsInUse[i] = AllItemList[LevelListToUse].ExtremeItemLevel[i];
                    break;
            }
        }
        
        TimerCounter = 99;
        Life = 3;
        Score = 0;
        SetupItemList();
    }
    private void SetupItemList()
    {
        TimerPaused = true;
        CorrectAnswer = Random.Range(1, 6);
        int chckans = CorrectAnswer - 1;
        for (int i = 0; i < ButtonList.Length; i++)
        {
            switch (Difficulty)
            {
                case 1:
                    if(i < 5)
                    {
                        ButtonList[i].SetActive(true);
                        if(chckans == i)
                        {
                            ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(1, ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.ItemSprite);
                            PointsToGive = ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.PointsToGive;
                        }
                        else
                        {
                            ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(0, ArcadeItemLevelsInUse[CurrentLevel-1].Distractors[i].ItemSprite);
                        }
                    }
                    else
                    {
                        ButtonList[i].SetActive(false);
                    }
                    break;
                case 2:
                    if (i < 10)
                    {
                        ButtonList[i].SetActive(true);
                        if (chckans == i)
                        {
                            ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(1, ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.ItemSprite);
                            PointsToGive = ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.PointsToGive;
                        }
                        else
                        {
                            ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(0, ArcadeItemLevelsInUse[CurrentLevel-1].Distractors[i].ItemSprite);
                        }
                    }
                    else
                    {
                        ButtonList[i].SetActive(false);
                    }
                    break;
                case 3:
                    if (i < 15)
                    {
                        ButtonList[i].SetActive(true);
                        if (chckans == i)
                        {
                            ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(1, ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.ItemSprite);
                            PointsToGive = ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.PointsToGive;
                        }
                        else
                        {
                            ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(0, ArcadeItemLevelsInUse[CurrentLevel-1].Distractors[i].ItemSprite);
                        }
                    }
                    else
                    {
                        ButtonList[i].SetActive(false);
                    }
                    break;
                case 4:
                    ButtonList[i].SetActive(true);
                    if (chckans == i)
                    {
                        ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(1, ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.ItemSprite);
                        PointsToGive = ArcadeItemLevelsInUse[CurrentLevel-1].TargetItem.PointsToGive;
                    }
                    else
                    {
                        ButtonList[i].GetComponent<ShowItemListScript>().SetupButton(0, ArcadeItemLevelsInUse[CurrentLevel-1].Distractors[i].ItemSprite);
                    }
                    break;
            }
        }
        OnClickPlaySound();
        TimerPaused = false;
    }
    public void OnClickPlaySound()
    {
        theSoundScript.PlaySpecificFX(ArcadeItemLevelsInUse[CurrentLevel - 1].TargetItem.ItemSound);
        ButtonIsPressed();
    }
    public void ClickAnswer(int numberClicked)
    {
        ButtonIsPressed();
        if (numberClicked == CorrectAnswer)
        {
            if(CurrentLevel != 10)
            {
                StartCoroutine(CorrectAnswerSequence());
            }
            else
            {
                StartCoroutine(LevelCompleteSequence());
            }
        }
        else
        {
            StartCoroutine(WrongAnswerSequence());
        }
    }
    private void UpdateScore(int BonusPoints)
    {
        Score = Score + PointsToGive + BonusPoints;
    }
    private void UpdateLife(int value)
    {
        Life += value;
        for (int i = 0; i < LifeMeter.Length; i++)
        {
            if (i < Life)
            {
                LifeMeter[i].SetActive(true);
            }
            else
            {
                LifeMeter[i].SetActive(false);
            }
        }
    }
    private void ButtonIsPressed()
    {
        UpressedTime = 10;
        PressMeHint.SetActive(false);
    }
    IEnumerator CorrectAnswerSequence()
    {
        TimerPaused = true;
        yield return new WaitForSeconds(0.1f);
        theSoundScript.PlayCorrectSound();
        CorrectSign.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        CorrectSign.SetActive(false);
        LoadingScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        int BonusToGive = (int)((TimerCounter / 2) * CurrentLevel) + (10 * Life);
        UpdateScore(BonusToGive);
        yield return new WaitForSeconds(0.5f);
        CurrentLevel += 1;
        yield return new WaitForSeconds(0.1f);
        SetupItemList();
        yield return new WaitForSeconds(0.1f);
        LoadingScreen.SetActive(false);
        TimerPaused = false;
    }
    IEnumerator WrongAnswerSequence()
    {
        TimerPaused = true;
        yield return new WaitForSeconds(0.1f);
        theSoundScript.PlayWrongSound();
        WrongSign.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        WrongSign.SetActive(false);
        UpdateLife(- 1);
        yield return new WaitForSeconds(0.1f);
        if (Life <= 0)
        {
            GameOverScreen.SetActive(true);
        }
        else
        {
            TimerPaused = false;
        }
    }
    IEnumerator LevelCompleteSequence()
    {
        theSoundScript.VolumeBackBGM();
        TimerPaused = true;
        YouWinScreen.SetActive(true);
        ScoreText.text = "" + Score;
        yield return new WaitForSeconds(0.5f);
        if (TimerCounter >= 20 && Life > 0)
        {
            if (!GameStar1.activeInHierarchy)
            {
                GameStar1.SetActive(true);
                StarGot += 1;
            }
            theSoundScript.PlayStarSound();
            ScoreText.text = "" + Score + " + 100";
            yield return new WaitForSeconds(0.5f);
            Score += 100;
            ScoreText.text = "" + Score;
        }
        else
        {
            if (GameStar1.activeInHierarchy)
            {
                GameStar1.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (TimerCounter >= 30 && Life > 1)
        {
            if (!GameStar2.activeInHierarchy)
            {
                GameStar2.SetActive(true);
                StarGot += 1;
            }

            theSoundScript.PlayStarSound();
            ScoreText.text = "" + Score + " + 100";
            yield return new WaitForSeconds(0.5f);
            Score += 100;
            ScoreText.text = "" + Score;
        }
        else
        {
            if (GameStar2.activeInHierarchy)
            {
                GameStar2.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (TimerCounter >= 60 && Life > 2)
        {
            if (!GameStar3.activeInHierarchy)
            {
                GameStar3.SetActive(true);
                StarGot += 1;
            }
            theSoundScript.PlayStarSound();
            ScoreText.text = "" + Score + " + 100";
            yield return new WaitForSeconds(0.5f);
            Score += 100;
            ScoreText.text = "" + Score;
        }
        else
        {
            if (GameStar3.activeInHierarchy)
            {
                GameStar3.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.5f);
        switch (Difficulty)
        {
            case 1:
                if (PlayerPrefs.GetInt("EasyLevelComplete") < 1)
                {
                    StartCoroutine(FirstTimeUnlockingEasySequence());
                    ScoreText.text = "" + Score + " + 1000";
                    yield return new WaitForSeconds(0.5f);
                    Score += 1000;
                    ScoreText.text = "" + Score;
                    PlayerPrefs.SetInt("EasyLevelComplete", StarGot);
                }
                else
                {
                    PlayerPrefs.SetInt("EasyLevelComplete", StarGot);
                }
                break;
            case 2:
                if (PlayerPrefs.GetInt("NormalLevelComplete") < 1)
                {
                    StartCoroutine(FirstTimeUnlockingNormalSequence());
                    ScoreText.text = "" + Score + " + 1000";
                    yield return new WaitForSeconds(0.5f);
                    Score += 1000;
                    ScoreText.text = "" + Score;
                    PlayerPrefs.SetInt("NormalLevelComplete", StarGot);
                }
                else
                {
                    PlayerPrefs.SetInt("NormalLevelComplete", StarGot);
                }
                break;
            case 3:
                if (PlayerPrefs.GetInt("HardLevelComplete") < 1)
                {
                    StartCoroutine(FirstTimeUnlockingHardSequence());
                    ScoreText.text = "" + Score + " + 1000";
                    yield return new WaitForSeconds(0.5f);
                    Score += 1000;
                    ScoreText.text = "" + Score;
                    PlayerPrefs.SetInt("HardLevelComplete", StarGot);
                }
                else
                {
                    PlayerPrefs.SetInt("HardLevelComplete", StarGot);
                }
                break;
            case 4:
                if (PlayerPrefs.GetInt("ExtremeLevelComplete") < 1)
                {
                    StartCoroutine(FirstTimeUnlockingExtremeSequence());
                    ScoreText.text = "" + Score + " + 1000";
                    yield return new WaitForSeconds(0.5f);
                    Score += 1000;
                    ScoreText.text = "" + Score;
                    PlayerPrefs.SetInt("ExtremeLevelComplete", StarGot);
                }
                else
                {
                    PlayerPrefs.SetInt("ExtremeLevelComplete", StarGot);
                }
                break;
        }
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + Score);
        theSoundScript.VolumeBackBGM();
    }
    IEnumerator FirstTimeUnlockingEasySequence()
    {
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Well done! You unlocked NORMAL MODE and you got 5 coins!";
        //Show message you unlocked next!
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 5);
    }
    IEnumerator FirstTimeUnlockingNormalSequence()
    {
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Well done! You unlocked HARD MODE and you got 10 coins!";
        //Show message you unlocked next!
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 10);
    }
    IEnumerator FirstTimeUnlockingHardSequence()
    {
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Well done! You unlocked EXTREME MODE and you got 15 coins!";
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 15);
    }
    IEnumerator FirstTimeUnlockingExtremeSequence()
    {
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Well done! You got 20 coins! Complete all three stars to get more points!";
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 20);
    }
}
