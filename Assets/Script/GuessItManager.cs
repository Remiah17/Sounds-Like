using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GuessItManager : MonoBehaviour
{
    public string ArcadeModeScene;
    [Space(20)]
    public GameObject SettingScreen;
    [Space(20)]
    public SoundsScript theSoundScript;
    [Space(20)]
    public int Difficulty;
    public Text DifficultyText;
    [Space(20)]
    //public GuessItLevel[] ArcadeItemList;
    [Space(20)]
    //public GuessItLevel[] GuessItItemsList1;
    //public GuessItLevel[] GuessItItemsList2;
    //public GuessItLevel[] GuessItItemsList3;
    //public GuessItLevel[] GuessItItemsList4;
    //public GuessItLevel[] GuessItItemsList5;
    //public GuessItLevel[] GuessItItemsList6;
    //public GuessItLevel[] GuessItItemsList7;
    //public GuessItLevel[] GuessItItemsList8;
    //public GuessItLevel[] GuessItItemsList9;
    //public GuessItLevel[] GuessItItemsList10;
    //public bool[] ArcadeItemListTaken;
    [Space(20)]
    public GameObject[] ChoicesButtons;
    public Image[] ChoiceSprite;
    public int CorrectAnswerNumber;
    public int ChoiceToGenerate;
    public int ChoiceGenerated;
    public bool[] EasyChoicesTaken;
    public bool[] NormalChoicesTaken;
    public bool[] HardChoicesTaken;
    public bool[] ExtremeChoicesTaken;
    //public bool[] ItemShown;
    [Space(20)]
    public bool LevelStarted;
    public int CurrentLevel;
    [Space(20)]
    public Text TimerText;
    public Text LevelNameText;
    public float LevelStartingTime;
    private float CountdownTime;
    private bool PauseTheTimer;
    [Space(20)]
    private int WrongCount;
    public int Life;
    public GameObject[] LifeMeter;  
    public GameObject CheckOBJs;
    public GameObject ExOBJs;
    [Space(20)]
    public AudioSource SFX;
    public AudioSource BGM;
    public float BGMCurrentVolume;
    private bool ItemSoundIsPlaying;
    [Space(20)]
    public GameObject GameStar1;
    public GameObject GameStar2;
    public GameObject GameStar3;
    [Space]
    public GameObject MascotMessageTab;
    public Text MascotMessageText;
    [Space]
    public GameObject GameOverScreen;
    public GameObject YouWinScreen;
    public GameObject LoadingScreen;
    public Text LoadingMessageText;
    [Space(20)]
    public bool Preparing;
    public bool DoneAssigningItemList;
    public bool DoneAssigningFirstLevelItems;
    public bool ClearItemList;
    public bool GeneratingItemChoice;
    public bool[] ArcadeChoiceTaken;
    //public GuessItLevel[] EasyChoiceToShow;
    //public GuessItLevel[] NormalChoiceToShow;
    //public GuessItLevel[] HardChoiceToShow;
    //public GuessItLevel[] ExtremeChoiceToShow;
    public bool DoneAssigningCorrectChoice;
    public bool DoneAssigningAllChoices;
    public bool FirstChoiceGeneration;
    // Start is called before the first frame update
    void Start()
    {
        Difficulty = PlayerPrefs.GetInt("GuessItDifficulty"); 
        GameStar1.SetActive(false);
        GameStar2.SetActive(false);
        GameStar3.SetActive(false);
        CurrentLevel = 1;
        WrongCount = 0;
        PauseTheTimer = true;
        Life = 3;
        LevelStarted = false;
        LoadingScreen.SetActive(true);
        Preparing = true;
        //StartCoroutine(StartSequence());
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Preparing)
        {
            if (!DoneAssigningItemList)
            {
                LoadingMessageText.text = "ASSIGNING ITEM LIST...";
                int r = Random.Range(0, 10);
                if(!ArcadeItemListTaken[r])
                {
                   switch(r)
                    {
                        case 1:
                            ArcadeItemList = GuessItItemsList1;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 2:
                            ArcadeItemList = GuessItItemsList2;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 3:
                            ArcadeItemList = GuessItItemsList3;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 4:
                            ArcadeItemList = GuessItItemsList4;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 5:
                            ArcadeItemList = GuessItItemsList5;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 6:
                            ArcadeItemList = GuessItItemsList6;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 7:
                            ArcadeItemList = GuessItItemsList7;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 8:
                            ArcadeItemList = GuessItItemsList8;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 9:
                            ArcadeItemList = GuessItItemsList9;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                        case 10:
                            ArcadeItemList = GuessItItemsList10;
                            ArcadeItemListTaken[r] = true;
                            DoneAssigningItemList = true;
                            break;
                    }
                }
            }
            else
            {
                if(!DoneAssigningFirstLevelItems)
                {
                    LoadingMessageText.text = "GENERATING CHOICES...";
                    switch (Difficulty)
                    {
                        case 1:
                            DifficultyText.text = "EASY";
                            ChoiceToGenerate = 5;
                            break;
                        case 2:
                            DifficultyText.text = "NORMAL";
                            ChoiceToGenerate = 10;
                            break;
                        case 3:
                            DifficultyText.text = "HARD";
                            ChoiceToGenerate = 15;
                            break;
                        case 4:
                            DifficultyText.text = "EXTREME";
                            ChoiceToGenerate = 20;
                            break;

                    }
                    if(!FirstChoiceGeneration)
                    {
                        GeneratingItemChoice = true;
                    }
                }
                else
                {

                }
            }
        }

        if(GeneratingItemChoice)
        {
            PauseTheTimer = true;
            CheckOBJs.SetActive(false);
            ExOBJs.SetActive(false);
            if (!ClearItemList)
            {
                switch(Difficulty)
                {
                    case 1:
                        for (int i = 0; i < EasyChoiceToShow.Length; i++)
                        {
                            EasyChoiceToShow[i] = null;
                        }
                        break;
                    case 2:
                        for (int i = 0; i < NormalChoiceToShow.Length; i++)
                        {
                            NormalChoiceToShow[i] = null;
                        }
                        break;
                    case 3:
                        for (int i = 0; i < HardChoiceToShow.Length; i++)
                        {
                            HardChoiceToShow[i] = null;
                        }
                        break;
                    case 4:
                        for (int i = 0; i < ExtremeChoiceToShow.Length; i++)
                        {
                            ExtremeChoiceToShow[i] = null;
                        }
                        break;
                }
                for (int i = 0; i < ArcadeChoiceTaken.Length; i++)
                {
                    ArcadeChoiceTaken[i] = false;

                    if(i == ArcadeChoiceTaken.Length - 1)
                    {
                        ClearItemList = true;
                    }
                }
            }
            else
            {
                if (!DoneAssigningCorrectChoice)
                {
                    CorrectAnswerNumber = Random.Range(0, ChoiceToGenerate);
                    //the correct answer will be assigned to the choice list
                    switch (Difficulty)
                    {
                        case 1:
                            for (int i = 0; i < EasyChoiceToShow.Length; i++)
                            {
                                if(i == CorrectAnswerNumber)
                                {
                                    EasyChoiceToShow[i] = ArcadeItemList[CurrentLevel - 1];
                                    EasyChoicesTaken[i] = true;
                                    ArcadeChoiceTaken[CorrectAnswerNumber] = true;
                                    DoneAssigningCorrectChoice = true;
                                    ChoiceGenerated += 1;
                                }
                            }
                            break;
                        case 2:
                            for (int i = 0; i < NormalChoiceToShow.Length; i++)
                            {
                                if (i == CorrectAnswerNumber)
                                {
                                    NormalChoiceToShow[i] = ArcadeItemList[CurrentLevel];
                                    NormalChoicesTaken[i] = true;
                                    ArcadeChoiceTaken[CorrectAnswerNumber] = true;
                                    DoneAssigningCorrectChoice = true;
                                    ChoiceGenerated += 1;
                                }
                            }
                            break;
                        case 3:
                            for (int i = 0; i < HardChoiceToShow.Length; i++)
                            {
                                if (i == CorrectAnswerNumber)
                                {
                                    HardChoiceToShow[i] = ArcadeItemList[CurrentLevel];
                                    HardChoicesTaken[i] = true;
                                    ArcadeChoiceTaken[CorrectAnswerNumber] = true;
                                    DoneAssigningCorrectChoice = true;
                                    ChoiceGenerated += 1;
                                }
                            }
                            break;
                        case 4:
                            for (int i = 0; i < ExtremeChoiceToShow.Length; i++)
                            {
                                if (i == CorrectAnswerNumber)
                                {
                                    ExtremeChoiceToShow[i] = ArcadeItemList[CurrentLevel];
                                    ExtremeChoicesTaken[i] = true;
                                    ArcadeChoiceTaken[CorrectAnswerNumber] = true;
                                    DoneAssigningCorrectChoice = true;
                                    ChoiceGenerated += 1;
                                }
                            }
                            break;

                    }
                }
                else
                {
                    if(!DoneAssigningAllChoices)
                    {
                        if (ChoiceGenerated < ChoiceToGenerate)
                        {
                            switch (Difficulty)
                            {
                                case 1:
                                    for (int i = 0; i < EasyChoiceToShow.Length; i++)
                                    {
                                        if (!EasyChoicesTaken[i])
                                        {
                                            int randomitemchoice = Random.Range(0, ChoiceToGenerate);

                                            if (!ArcadeChoiceTaken[randomitemchoice])
                                            {
                                                EasyChoiceToShow[i] = ArcadeItemList[randomitemchoice];
                                                EasyChoicesTaken[i] = true;
                                                ArcadeChoiceTaken[randomitemchoice] = true;
                                                ChoiceGenerated += 1;
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    for (int i = 0; i < NormalChoiceToShow.Length; i++)
                                    {
                                        if (!NormalChoicesTaken[i])
                                        {
                                            int randomitemchoice = Random.Range(0, ChoiceToGenerate);

                                            if (!ArcadeChoiceTaken[randomitemchoice])
                                            {
                                                NormalChoiceToShow[i] = ArcadeItemList[randomitemchoice];
                                                NormalChoicesTaken[i] = true;
                                                ArcadeChoiceTaken[randomitemchoice] = true;
                                                ChoiceGenerated += 1;
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    for (int i = 0; i < HardChoiceToShow.Length; i++)
                                    {
                                        if (!HardChoicesTaken[i])
                                        {
                                            int randomitemchoice = Random.Range(0, ChoiceToGenerate);

                                            if (!ArcadeChoiceTaken[randomitemchoice])
                                            {
                                                HardChoiceToShow[i] = ArcadeItemList[randomitemchoice];
                                                HardChoicesTaken[i] = true;
                                                ArcadeChoiceTaken[randomitemchoice] = true;
                                                ChoiceGenerated += 1;
                                            }
                                        }
                                    }
                                    break;
                                case 4:
                                    for (int i = 0; i < ExtremeChoiceToShow.Length; i++)
                                    {
                                        if (!ExtremeChoicesTaken[i])
                                        {
                                            int randomitemchoice = Random.Range(0, ChoiceToGenerate);

                                            if (!ArcadeChoiceTaken[randomitemchoice])
                                            {
                                                ExtremeChoiceToShow[i] = ArcadeItemList[randomitemchoice];
                                                ExtremeChoicesTaken[i] = true;
                                                ArcadeChoiceTaken[randomitemchoice] = true;
                                                ChoiceGenerated += 1;
                                            }
                                        }
                                    }
                                    break;

                            }
                        }
                        else
                        {
                            DoneAssigningAllChoices = true;
                        }
                    }
                    else
                    {
                        if(!FirstChoiceGeneration)
                        {
                            FirstChoiceGeneration = true;
                            DoneAssigningFirstLevelItems = true;
                        }
                        else
                        {
                            GeneratingItemChoice = false;
                            StartCoroutine(ReadytoStartSequence());
                        }
                    }
                }
                
            }
        }



        if(LevelStarted)
        {
            if(CountdownTime > 0 && !PauseTheTimer)
            {
                CountdownTime -= 1 * Time.deltaTime;
            }
            TimerText.text = "" + Mathf.Round(CountdownTime);

            //SHOW RIGHT AMOUNT OF BUTTON AND HIDE THE REST DEPENDING ON DIFFICULTY
            for (int i = 0; i < ChoicesButtons.Length; i++)
            {
                if(i < ChoiceToGenerate && ClearItemList)
                {
                    ChoicesButtons[i].SetActive(true);
                    switch(Difficulty)
                    {
                        case 1:
                            if(EasyChoiceToShow[i] != null)
                            {
                                ChoiceSprite[i].sprite = EasyChoiceToShow[i].ItemSprite;
                            }
                            break;
                        case 2:
                            if (NormalChoiceToShow[i] != null)
                            {
                                ChoiceSprite[i].sprite = NormalChoiceToShow[i].ItemSprite;
                            }
                            break;
                        case 3:
                            if (HardChoiceToShow[i] != null)
                            {
                                ChoiceSprite[i].sprite = HardChoiceToShow[i].ItemSprite;
                            }
                            break;
                        case 4:
                            if (ExtremeChoiceToShow[i] != null)
                            {
                                ChoiceSprite[i].sprite = ExtremeChoiceToShow[i].ItemSprite;
                            }
                            break;
                    }
                   
                }
                else
                {
                    ChoicesButtons[i].SetActive(false);
                }
            }

        }

        LevelNameText.text = "LEVEL " + CurrentLevel;

        for (int i = 0; i < LifeMeter.Length; i++)
        {
            if(i < Life)
            {
                LifeMeter[i].SetActive(true);
            }
            else
            {
                LifeMeter[i].SetActive(false);
            }
        }

        if(Input.GetKey(KeyCode.Space))
        {
            CurrentLevel = 20;
        }
    }

    public IEnumerator ReadytoStartSequence()
    {
        LoadingMessageText.text = "STARTING IN 3...";
        yield return new WaitForSeconds(1);
        LoadingMessageText.text = "STARTING IN 2...";
        yield return new WaitForSeconds(1);
        LoadingMessageText.text = "STARTING IN 1...";
        yield return new WaitForSeconds(1);
        LevelStartingTime = 300;
        CountdownTime = LevelStartingTime;
        LoadingScreen.SetActive(false);
        LevelStarted = true;
        PauseTheTimer = false;
        Preparing = false;
    }


    public void OnClickPlaySound()
    {
        if(!ItemSoundIsPlaying)
        {
            StartCoroutine(PlayItemSoundSequence());
        }
        else
        {
            SFX.Stop();
            StartCoroutine(PlayItemSoundSequence());
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
        }
        else
        {
            SettingScreen.SetActive(true);
        }
    }

    public void OnClickAnswer(int _choiceNumber)
    {
        if(_choiceNumber == CorrectAnswerNumber)
        {
            StartCoroutine(CorrectAswerSequence());
            theSoundScript.PlayCorrectSound();
            //ChoicesButtons[_choiceNumber].SetActive(false);
        }
        else
        {
            StartCoroutine(WrongAswerSequence());
            theSoundScript.PlayWrongSound();
        }
    }

    IEnumerator CorrectAswerSequence()
    {
        PauseTheTimer = true;
        CheckOBJs.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if(CurrentLevel == 20)
        {
            YouWinScreen.SetActive(true);
            if (CountdownTime > LevelStartingTime / 4 && WrongCount < 2)
            {
                if (!GameStar1.activeInHierarchy)
                {
                    GameStar1.SetActive(true);
                }
                theSoundScript.PlayStarSound();
            }
            else
            {
                if (GameStar1.activeInHierarchy)
                {
                    GameStar1.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.5f);
            if (CountdownTime > LevelStartingTime / 3 && WrongCount < 1)
            {
                if (!GameStar2.activeInHierarchy)
                {
                    GameStar2.SetActive(true);
                }

                theSoundScript.PlayStarSound();
            }
            else
            {
                if (GameStar2.activeInHierarchy)
                {
                    GameStar2.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.5f);
            if (CountdownTime > 1.5f && WrongCount < 2)
            {
                if (!GameStar3.activeInHierarchy)
                {
                    GameStar3.SetActive(true);
                }
                theSoundScript.PlayStarSound();
            }
            else
            {
                if (GameStar3.activeInHierarchy)
                {
                    GameStar3.SetActive(false);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        CheckOBJs.SetActive(false);
        ClearItemList = false;
        DoneAssigningCorrectChoice = false;
        DoneAssigningAllChoices = false;
        ChoiceGenerated = 0;
        yield return new WaitForSeconds(0.5f);
        if (CurrentLevel != 20)
        {
            PauseTheTimer = false;
            CurrentLevel += 1;
            theSoundScript.PlaySelectSound();
            GeneratingItemChoice = true;
        }
        else
        {
            switch(Difficulty)
            {
                case 1:
                    if (PlayerPrefs.GetInt("EasyLevelComplete") != 1)
                    {
                        StartCoroutine(FirstTimeUnlockingEasySequence());
                    }
                    else
                    {
                        OnClickBack();
                    }
                    break;
                case 2:
                    if (PlayerPrefs.GetInt("NormalLevelComplete") != 1)
                    {
                        StartCoroutine(FirstTimeUnlockingNormalSequence());
                    }
                    else
                    {
                        OnClickBack();
                    }
                    break;
                case 3:
                    if (PlayerPrefs.GetInt("HardLevelComplete") != 1)
                    {
                        StartCoroutine(FirstTimeUnlockingHardSequence());
                    }
                    else
                    {
                        OnClickBack();
                    }
                    break;
                case 4:
                    if (PlayerPrefs.GetInt("ExtremeLevelComplete") != 1)
                    {
                        StartCoroutine(FirstTimeUnlockingExtremeSequence());
                    }
                    else
                    {
                        OnClickBack();
                    }
                    break;
            }
        }*/
    }

    IEnumerator WrongAswerSequence()
    {
        PauseTheTimer = true;
        ExOBJs.SetActive(true);
        WrongCount += 1;
        Life -= 1;
        yield return new WaitForSeconds(0.5f);
        ExOBJs.SetActive(false);
        PauseTheTimer = false;
        if(Life <= 0)
        {
            GameOverScreen.SetActive(true);
        }
    }
    /*
    IEnumerator PlayItemSoundSequence()
    {
        
        ItemSoundIsPlaying = true;
        switch(Difficulty)
        {
            case 1:
                SFX.clip = ArcadeItemList[CurrentLevel - 1].ItemSound;
                break;
            case 2:
                SFX.clip = ArcadeItemList[CurrentLevel - 1].ItemSound;
                break;
            case 3:
                SFX.clip = ArcadeItemList[CurrentLevel - 1].ItemSound;
                break;
            case 4:
                SFX.clip = ArcadeItemList[CurrentLevel - 1].ItemSound;
                break;
        }
        if (SFX.isPlaying)
        {
            SFX.Stop();
        }
        BGMCurrentVolume = BGM.volume;
        BGM.volume = 0;
        SFX.Play();
        yield return new WaitForSeconds(4f);
        BGM.volume = BGMCurrentVolume;
        ItemSoundIsPlaying = false;
    }

    IEnumerator FirstTimeUnlockingEasySequence()
    {
        PlayerPrefs.SetInt("EasyLevelComplete", 1);
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Contratulation! You unlocked NORMAL MODE!";
        //Show message you unlocked next!
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        OnClickBack();

    }

    IEnumerator FirstTimeUnlockingNormalSequence()
    {
        PlayerPrefs.SetInt("NormalLevelComplete", 1);
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Contratulation! You unlocked HARD MODE!";
        //Show message you unlocked next!
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        OnClickBack();
    }

    IEnumerator FirstTimeUnlockingHardSequence()
    {
        PlayerPrefs.SetInt("HardLevelComplete", 1);
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Contratulation! You unlocked EXTREME MODE!";
        //Show message you unlocked next!
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        OnClickBack();
    }

    IEnumerator FirstTimeUnlockingExtremeSequence()
    {
        PlayerPrefs.SetInt("ExtremeLevelComplete", 1);
        MascotMessageTab.SetActive(true);
        MascotMessageText.text = "Contratulation! You unlocked MATCH MODE!";
        //Show message you unlocked next!
        yield return new WaitForSeconds(3);
        MascotMessageTab.SetActive(false);
        OnClickBack();
    }*/
}
