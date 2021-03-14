using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using System.Linq;
using Firebase.Extensions;
using UnityEngine.Networking;

public class SoundsOfTheMonthManager : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField]
    private string OnlineModeScene;
    [Header("SCREENS")]
    [SerializeField]
    private GameObject SettingScreen;
    [SerializeField]
    private GameObject SoundPlayScreen;
    [SerializeField]
    private GameObject RetryLevelScreen;
    [SerializeField]
    private Text CoinText;
    private int CoinCount;
    [SerializeField]
    private GameObject NotEnoughCoinScreen;
    private SoundsScript theSoundScript;
    private bool InGame;
    [Header("LEVEL BUTTONS")]
    [SerializeField]
    private Button[] SoundButtons;
    [SerializeField]
    private Text[] LeadingUserNameText;
    [SerializeField]
    private Text[] LeadingPointsText;
    [SerializeField]
    private Text[] SoundStatusText;
    [SerializeField]
    private GameObject[] SoundCheckMark;
    [SerializeField]
    private GameObject[] SoundCrossMark;
    private int CurrentSound;
    [Header("SEASON POINTS")]
    [SerializeField]
    private Text PointsText;
    private int CurrentSeasonPoints = -1;
    private bool ScreenRefreshed;
    [Header("SOUND PLAY SCREEN")]
    [SerializeField]
    private string[] PossibleAnswers;
    [SerializeField]
    private Text CurrentSoundNumberText;
    [SerializeField]
    private InputField AnswerInput;
    [SerializeField]
    private GameObject RightScreen;
    [SerializeField]
    private GameObject WrongScreen;
    [SerializeField]
    private Text YouGotAmountPointsText;
    private float PointsToGive;
    [SerializeField]
    private Text TimerText;
    private float TimeCounter;
    [SerializeField]
    private SoundOfTheSeasonItemList SeasonLevelItems;
    [SerializeField]
    private LoginScript TheLoginScript;
    private int Season1tem1Try;
    private int Season2tem1Try;
    private int Season3tem1Try;
    private int Season4tem1Try;
    private int Season5tem1Try;
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;
    public FirebaseStorage storage;

    private bool AudioClipDownloaded = false;
    private int AudioDownLoaded;
    private bool SeasonPointsChecked = false;
    
    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        ScreenRefreshed = false;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if(!ScreenRefreshed)
        {
            RefreshTheMainScreen();
        }
        if (SeasonPointsChecked && CurrentSeasonPoints < 0)
        {
            TheLoginScript.InitiateConnectionError();
            CurrentSeasonPoints = 0;
        }
        if (SoundPlayScreen.activeInHierarchy)
        {
            if(TimeCounter > 0)
            {
                TimeCounter -= 1 * Time.deltaTime;
                TimerText.text = "" + Mathf.Round(TimeCounter);
                PointsToGive -= 33 * Time.deltaTime;
            }
            else if (TimeCounter <= 0)
            {
                if(InGame)
                {
                    InGame = false;
                    //wronganswer
                    theSoundScript.PlayWrongSound();
                    switch (CurrentSound)
                    {
                        case 1:
                            PlayerPrefs.SetInt("SeasonSoundOneAnswered", 2); //0=null 1=right 2=wrong
                            break;
                        case 2:
                            PlayerPrefs.SetInt("SeasonSoundTwoAnswered", 2); //0=null 1=right 2=wrong
                            break;
                        case 3:
                            PlayerPrefs.SetInt("SeasonSoundThreeAnswered", 2); //0=null 1=right 2=wrong
                            break;
                        case 4:
                            PlayerPrefs.SetInt("SeasonSoundFourAnswered", 2); //0=null 1=right 2=wrong
                            break;
                        case 5:
                            PlayerPrefs.SetInt("SeasonSoundFiveAnswered", 2); //0=null 1=right 2=wrong
                            break;
                    }
                    StartCoroutine(UpdateSoundOfTheSeasonData(2));
                    WrongScreen.SetActive(true);
                }
            }
        }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        storage = FirebaseStorage.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            User = auth.CurrentUser;
        }
        Debug.Log("Firebase Initialized!");
    }
    public void OnClickLevel(int SoundNumber)
    {
        theSoundScript.PlaySelectSound();
        CurrentSound = SoundNumber;
        switch (SoundNumber)
        {
            case 1:
                if(Season1tem1Try == 2)
                {
                    RetryLevelScreen.SetActive(true);
                }
                else if (Season1tem1Try == 0)
                {
                    SetupLevel(SoundNumber);
                    SoundPlayScreen.SetActive(true);
                }
                break;
            case 2:
                if (Season2tem1Try == 2)
                {
                    RetryLevelScreen.SetActive(true);
                }
                else if (Season2tem1Try == 0)
                {
                    SetupLevel(SoundNumber);
                    SoundPlayScreen.SetActive(true);
                }
                break;
            case 3:
                if (Season3tem1Try == 2)
                {
                    RetryLevelScreen.SetActive(true);
                }
                else if (Season3tem1Try == 0)
                {
                    SetupLevel(SoundNumber);
                    SoundPlayScreen.SetActive(true);
                }
                break;
            case 4:
                if (Season4tem1Try == 2)
                {
                    RetryLevelScreen.SetActive(true);
                }
                else if (Season4tem1Try == 0)
                {
                    SetupLevel(SoundNumber);
                    SoundPlayScreen.SetActive(true);
                }
                break;
            case 5:
                if (Season5tem1Try == 2)
                {
                    RetryLevelScreen.SetActive(true);
                }
                else if (Season5tem1Try == 0)
                {
                    SetupLevel(SoundNumber);
                    SoundPlayScreen.SetActive(true);
                }
                break;
        }
    }
    public void Close_LevelScreen()
    {
        theSoundScript.PlaySelectSound();
        HideLevel();
    }
    public void PlayLevelSound()
    {
        theSoundScript.PlaySpecificFX(SeasonLevelItems.SoundsOfTheSeasonItemList[CurrentSound - 1].ItemSound);
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
            RightScreen.SetActive(true);
            YouGotAmountPointsText.text = "You got " + PointsToGive + " points!";
            PlayerPrefs.SetInt("Score", (int)(PlayerPrefs.GetInt("Score") + Mathf.Round(PointsToGive)));
            //Update the SeasonPoints
            StartCoroutine(UpdateSoundItemTryData(1));
            StartCoroutine(UpdateSeasonPoints());
            StartCoroutine(UpdateSoundOfTheSeasonData(1));
        }
        else
        {
            //wronganswer
            theSoundScript.PlayWrongSound();
            StartCoroutine(UpdateSoundItemTryData(2));
            StartCoroutine(UpdateSoundOfTheSeasonData(2));
            WrongScreen.SetActive(true);
        }
    }
    public void OnClick_CloseRightScreen()
    {
        theSoundScript.PlaySelectSound();
        RightScreen.SetActive(false);
        Close_LevelScreen();
    }
    public void OnClick_CloseWrongScreen()
    {
        theSoundScript.PlaySelectSound();
        WrongScreen.SetActive(false);
        Close_LevelScreen();
    }
    public void OnClickBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(OnlineModeScene);
    }
    public void OnClickSetting()
    {
        theSoundScript.PlaySelectSound();
        if (SettingScreen.activeInHierarchy)
        {
            ScreenRefreshed = false;
            SettingScreen.SetActive(false);
        }
        else
        {
            SettingScreen.SetActive(true);
        }
    }
    public void OnClick_CloseRetryScreen()
    {
        theSoundScript.PlaySelectSound();
        CurrentSound = 0;
        theSoundScript.PlaySelectSound();
        RetryLevelScreen.SetActive(false);
    }
    public void OnClick_RetryWithCoins()
    {
        theSoundScript.PlaySelectSound();
        //remove coins
        if (CoinCount >= 50)
        {
            PlayerPrefs.SetInt("Coins", CoinCount - 50);
            RetryLevelScreen.SetActive(false);
            StartCoroutine(UpdateSoundItemTryData(0));
            SetupLevel(CurrentSound);
            SoundPlayScreen.SetActive(true);
        }
        else
        {
            NotEnoughCoinScreen.SetActive(true);
        }
    }
    public void OnClick_CloseNotEnoughCoinScreen()
    {
        theSoundScript.PlaySelectSound();
        NotEnoughCoinScreen.SetActive(false);
    }
    private void HideLevel()
    {
        CurrentSound = 0;
        SoundPlayScreen.SetActive(false);
        PointsToGive = 0;
        TimeCounter = 0;
        AnswerInput.text = null;
        ScreenRefreshed = false;
    }
    private void SetupLevel(int level)
    {
        for (int i = 0; i < PossibleAnswers.Length; i++)
        {
            PossibleAnswers[i] = SeasonLevelItems.SoundsOfTheSeasonItemList[CurrentSound - 1].PossibleAnswers[i];
        }
        PointsToGive = SeasonLevelItems.SoundsOfTheSeasonItemList[CurrentSound - 1].PointsToGive;
        TimeCounter = 60;
        InGame = true;
        AnswerInput.text = null;
        PlayLevelSound();
    }
    [System.Obsolete]
    private void RefreshTheMainScreen()
    {
        ScreenRefreshed = true;
        if (User != null)
        {
            StartCoroutine(GetSoundOfTheSeasonData());
            StartCoroutine(GetSeasonPoints());
            StartCoroutine(CheckConnectionTime());
            if(!AudioClipDownloaded)
            {
                StartCoroutine(SetSounds());
            }
            else if (AudioClipDownloaded && AudioDownLoaded < 5)
            {
                StartCoroutine(SetSounds());
            }
        }
        else
        {
            SeasonPointsChecked = true;
        }
        CoinCount = PlayerPrefs.GetInt("Coins");
        CoinText.text = CoinCount.ToString();
    }
    IEnumerator GetSoundOfTheSeasonData()
    {
        var ATask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = ATask.Result;

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                int soundnum = int.Parse(childSnapshot.Key.ToString()) - 1;
                LeadingPointsText[soundnum].text = childSnapshot.Child("LeadingPoints").Value.ToString();
                SoundStatusText[soundnum].text = childSnapshot.Child("RightAnswers").Value.ToString() + " / " + childSnapshot.Child("TotalAnswers").Value.ToString();

                if(int.Parse(childSnapshot.Child("LeadingPoints").Value.ToString()) > 0)
                {
                    var PTask = DBreference.Child("users").Child(childSnapshot.Child("LeadingUserID").Value.ToString()).Child("username").GetValueAsync();
                    yield return new WaitUntil(predicate: () => PTask.IsCompleted);
                    if (PTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {PTask.Exception}");

                    }
                    else
                    {
                        DataSnapshot _usernm = PTask.Result;
                        LeadingUserNameText[soundnum].text = _usernm.Value.ToString();
                    }
                }
                else
                {
                    LeadingUserNameText[soundnum].text = childSnapshot.Child("LeadingUser").Value.ToString();
                }
                
            }

            //GET SOUND OF THE SEASON ITEM TRY DATA
            var KTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundOneAnswered").GetValueAsync();

            yield return new WaitUntil(predicate: () => KTask.IsCompleted);

            if (KTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {KTask.Exception}");

            }
            else
            {
                DataSnapshot _Item1Data = KTask.Result;
                Season1tem1Try = int.Parse(_Item1Data.Value.ToString());

            }
            var LTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundTwoAnswered").GetValueAsync();

            yield return new WaitUntil(predicate: () => LTask.IsCompleted);

            if (LTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {LTask.Exception}");
            }
            else
            {
                DataSnapshot _Item2Data = LTask.Result;
                Season2tem1Try = int.Parse(_Item2Data.Value.ToString());

            }
            var MTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundThreeAnswered").GetValueAsync();

            yield return new WaitUntil(predicate: () => MTask.IsCompleted);

            if (MTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {MTask.Exception}");
            }
            else
            {
                DataSnapshot _Item3Data = MTask.Result;
                Season3tem1Try = int.Parse(_Item3Data.Value.ToString());

            }

            var NTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundFourAnswered").GetValueAsync();

            yield return new WaitUntil(predicate: () => NTask.IsCompleted);

            if (NTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {NTask.Exception}");
            }
            else
            {
                DataSnapshot _Item4Data = NTask.Result;
                Season4tem1Try = int.Parse(_Item4Data.Value.ToString());

            }

            var OTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundFiveAnswered").GetValueAsync();

            yield return new WaitUntil(predicate: () => OTask.IsCompleted);

            if (OTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {OTask.Exception}");
            }
            else
            {
                DataSnapshot _Item5Data = OTask.Result;
                Season5tem1Try = int.Parse(_Item5Data.Value.ToString());

            }

            for (int i = 1; i < 6; i++)
            {
                switch (i)
                {
                    case 1:
                        switch (Season1tem1Try)
                        {
                            case 0:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 1:
                                SoundCheckMark[i - 1].SetActive(true);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 2:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(true);
                                break;
                        }
                        break;
                    case 2:
                        switch (Season2tem1Try)
                        {
                            case 0:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 1:
                                SoundCheckMark[i - 1].SetActive(true);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 2:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(true);
                                break;
                        }
                        break;
                    case 3:
                        switch (Season3tem1Try)
                        {
                            case 0:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 1:
                                SoundCheckMark[i - 1].SetActive(true);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 2:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(true);
                                break;
                        }
                        break;
                    case 4:
                        switch (Season4tem1Try)
                        {
                            case 0:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 1:
                                SoundCheckMark[i - 1].SetActive(true);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 2:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(true);
                                break;
                        }
                        break;
                    case 5:
                        switch (Season5tem1Try)
                        {
                            case 0:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 1:
                                SoundCheckMark[i - 1].SetActive(true);
                                SoundCrossMark[i - 1].SetActive(false);
                                break;
                            case 2:
                                SoundCheckMark[i - 1].SetActive(false);
                                SoundCrossMark[i - 1].SetActive(true);
                                break;
                        }
                        break;
                }
            }

            ScreenRefreshed = true;
        }
    }
    IEnumerator GetSeasonPoints()
    {
        var ATask = DBreference.Child("users").Child(User.UserId).Child("SeasonPoints").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = ATask.Result;
            int pointshaving = int.Parse(snapshot.Value.ToString());
            PointsText.text = pointshaving.ToString();
            CurrentSeasonPoints = pointshaving;
        }
        SeasonPointsChecked = true;
    }
    IEnumerator UpdateSoundItemTryData(int value)
    {
        switch(CurrentSound)
        {
            case 1:
                var LTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundOneAnswered").SetValueAsync(Season1tem1Try + value);

                yield return new WaitUntil(predicate: () => LTask.IsCompleted);

                if (LTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {LTask.Exception}");
                }
                else
                {

                }
                break;
            case 2:
                var MTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundTwoAnswered").SetValueAsync(Season2tem1Try + value);

                yield return new WaitUntil(predicate: () => MTask.IsCompleted);

                if (MTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {MTask.Exception}");
                }
                else
                {

                }
                break;
            case 3:
                var NTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundThreeAnswered").SetValueAsync(Season3tem1Try + value);

                yield return new WaitUntil(predicate: () => NTask.IsCompleted);

                if (NTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {NTask.Exception}");
                }
                else
                {

                }
                break;
            case 4:
                var OTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundFourAnswered").SetValueAsync(Season4tem1Try + value);

                yield return new WaitUntil(predicate: () => OTask.IsCompleted);

                if (OTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {OTask.Exception}");
                }
                else
                {

                }
                break;
            case 5:
                var PTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundFiveAnswered").SetValueAsync(Season5tem1Try + value);

                yield return new WaitUntil(predicate: () => PTask.IsCompleted);

                if (PTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {PTask.Exception}");
                }
                else
                {

                }
                break;
        }
       
    }
    IEnumerator UpdateSeasonPoints()
    {
        var ATask = DBreference.Child("users").Child(User.UserId).Child("SeasonPoints").SetValueAsync(CurrentSeasonPoints + Mathf.Round(PointsToGive));

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            StartCoroutine(CheckIfSoundMasterOfSeason());
        }
    }
    IEnumerator UpdateSoundOfTheSeasonData(int rightorwrong) //1=right answer 2=wronganswer
    {
        var ATask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").GetValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);
        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = ATask.Result;

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                int soundnum = int.Parse(childSnapshot.Key.ToString());

                if (soundnum == CurrentSound)
                {
                    switch (rightorwrong)
                    {
                        case 1://if right
                            if (int.Parse(childSnapshot.Child("LeadingPoints").Value.ToString()) < PointsToGive)
                            {
                                var BTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child(CurrentSound.ToString()).Child("LeadingPoints").SetValueAsync(Mathf.Round(PointsToGive));
                                yield return new WaitUntil(predicate: () => BTask.IsCompleted);

                                if (BTask.Exception != null)
                                {
                                    Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
                                }
                                else
                                {
                                    var CTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child(CurrentSound.ToString()).Child("LeadingUser").SetValueAsync(User.DisplayName);
                                    yield return new WaitUntil(predicate: () => CTask.IsCompleted);

                                    if (CTask.Exception != null)
                                    {
                                        Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
                                    }
                                    else
                                    {
                                        var GTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child(CurrentSound.ToString()).Child("LeadingUserID").SetValueAsync(User.UserId);
                                        yield return new WaitUntil(predicate: () => GTask.IsCompleted);

                                        if (GTask.Exception != null)
                                        {
                                            Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                            var DTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child(CurrentSound.ToString()).Child("RightAnswers").SetValueAsync(int.Parse(childSnapshot.Child("RightAnswers").Value.ToString()) + 1);
                            yield return new WaitUntil(predicate: () => DTask.IsCompleted);
                            if (DTask.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
                            }
                            else
                            {
                                var ETask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child(CurrentSound.ToString()).Child("TotalAnswers").SetValueAsync(int.Parse(childSnapshot.Child("TotalAnswers").Value.ToString()) + 1);
                                yield return new WaitUntil(predicate: () => ETask.IsCompleted);

                                if (ETask.Exception != null)
                                {
                                    Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
                                }
                                else
                                {

                                }
                            }
                            break;
                        case 2://if wrong
                            var FTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child(CurrentSound.ToString()).Child("TotalAnswers").SetValueAsync(int.Parse(childSnapshot.Child("TotalAnswers").Value.ToString()) + 1);
                            yield return new WaitUntil(predicate: () => FTask.IsCompleted);
                            if (FTask.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
                            }
                            else
                            {

                            }

                            var HTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child(CurrentSound.ToString()).Child("PointsToGive").SetValueAsync(int.Parse(childSnapshot.Child("PointsToGive").Value.ToString()) + 50);
                            yield return new WaitUntil(predicate: () => HTask.IsCompleted);
                            if (HTask.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task with {HTask.Exception}");
                            }
                            else
                            {

                            }
                            break;
                    }
                }
            }
        }
    }
    IEnumerator CheckIfSoundMasterOfSeason()
    {
        var ATask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("Season Points").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = ATask.Result;
            int highestSeasonPoint = int.Parse(snapshot.Value.ToString());

            if(CurrentSeasonPoints > highestSeasonPoint)
            {
                var BTask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("Season Points").SetValueAsync(CurrentSeasonPoints);

                yield return new WaitUntil(predicate: () => BTask.IsCompleted);

                if (BTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
                }
                else
                {

                }

                var CTask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("Name").SetValueAsync(User.DisplayName);

                yield return new WaitUntil(predicate: () => CTask.IsCompleted);

                if (CTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
                }
                else
                {

                }

                var DTask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("UserID").SetValueAsync(User.UserId);

                yield return new WaitUntil(predicate: () => DTask.IsCompleted);

                if (DTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
                }
                else
                {

                }
            }
        }
    }
    IEnumerator CheckConnectionTime()
    {
        TheLoginScript.ShowLoadingScreen("Retrieving datas from the server.");
        if(!SeasonPointsChecked)
        {
            yield return new WaitForSeconds(1);
            if (!SeasonPointsChecked)
            {
                yield return new WaitForSeconds(1);
                if (!SeasonPointsChecked)
                {
                    yield return new WaitForSeconds(1);
                    if (!SeasonPointsChecked)
                    {
                        yield return new WaitForSeconds(1);
                        if (!SeasonPointsChecked)
                        {
                            yield return new WaitForSeconds(1);
                            if (!SeasonPointsChecked)
                            {
                                yield return new WaitForSeconds(1);
                                if (!SeasonPointsChecked)
                                {
                                    yield return new WaitForSeconds(1);
                                    if (!SeasonPointsChecked)
                                    {
                                        yield return new WaitForSeconds(1);
                                        if (!SeasonPointsChecked)
                                        {
                                            yield return new WaitForSeconds(1);
                                            if (!SeasonPointsChecked)
                                            {
                                                yield return new WaitForSeconds(1);
                                                SeasonPointsChecked = true;
                                            }
                                            else
                                            {
                                                TheLoginScript.HideLoadingScreen();
                                            }
                                        }
                                        else
                                        {
                                            TheLoginScript.HideLoadingScreen();
                                        }
                                    }
                                    else
                                    {
                                        TheLoginScript.HideLoadingScreen();
                                    }
                                }
                                else
                                {
                                    TheLoginScript.HideLoadingScreen();
                                }
                            }
                            else
                            {
                                TheLoginScript.HideLoadingScreen();
                            }
                        }
                        else
                        {
                            TheLoginScript.HideLoadingScreen();
                        }
                    }
                    else
                    {
                        TheLoginScript.HideLoadingScreen();
                    }
                }
                else
                {
                    TheLoginScript.HideLoadingScreen();
                }
            }
            else
            {
                TheLoginScript.HideLoadingScreen();
            }
        }
        else
        {
            SeasonPointsChecked = true;
            TheLoginScript.HideLoadingScreen();
        }
    }
    [System.Obsolete]
    IEnumerator SetSounds()
    {
        AudioClipDownloaded = true;
        StorageReference Sound1 = storage.GetReferenceFromUrl("gs://sounds-like-26713874.appspot.com/OnlineSounds/Sounds of the Season/1/sound.mp3");
        // Fetch the download URL
        Sound1.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                //Debug.Log("Download URL: " + task.Result);
                StartCoroutine(DownloadSound(task.Result,1));
            }
        });
        var ATask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("1").Child("ItemNames").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemNames = ATask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemNames.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[0].PossibleAnswers[_itemNum] = childSnapshot.Key.ToString();
                _itemNum += 1;
            }
        }
        var BTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("1").Child("PointsToGive").GetValueAsync();

        yield return new WaitUntil(predicate: () => BTask.IsCompleted);

        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemPointtogive = BTask.Result;
            SeasonLevelItems.SoundsOfTheSeasonItemList[0].PointsToGive = int.Parse(SoundItemPointtogive.Value.ToString());
        }
        var CTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("1").Child("Gifts").GetValueAsync();

        yield return new WaitUntil(predicate: () => CTask.IsCompleted);

        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemGifts = CTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemGifts.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[0].Gifts[_itemNum] = int.Parse(childSnapshot.Key.ToString());
                _itemNum += 1;
            }
        }


        StorageReference Sound2 = storage.GetReferenceFromUrl("gs://sounds-like-26713874.appspot.com/OnlineSounds/Sounds of the Season/2/sound.mp3");
        // Fetch the download URL
        Sound2.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                //Debug.Log("Download URL: " + task.Result);
                StartCoroutine(DownloadSound(task.Result, 2));
            }
        });
        var DTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("2").Child("ItemNames").GetValueAsync();

        yield return new WaitUntil(predicate: () => DTask.IsCompleted);

        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemNames = DTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemNames.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[1].PossibleAnswers[_itemNum] = childSnapshot.Key.ToString();
                _itemNum += 1;
            }
        }
        var ETask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("2").Child("PointsToGive").GetValueAsync();

        yield return new WaitUntil(predicate: () => ETask.IsCompleted);

        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemPointtogive = ETask.Result;
            SeasonLevelItems.SoundsOfTheSeasonItemList[1].PointsToGive = int.Parse(SoundItemPointtogive.Value.ToString());
        }
        var FTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("2").Child("Gifts").GetValueAsync();

        yield return new WaitUntil(predicate: () => FTask.IsCompleted);

        if (FTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemGifts = FTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemGifts.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[1].Gifts[_itemNum] = int.Parse(childSnapshot.Key.ToString());
                _itemNum += 1;
            }
        }


        StorageReference Sound3 = storage.GetReferenceFromUrl("gs://sounds-like-26713874.appspot.com/OnlineSounds/Sounds of the Season/3/sound.mp3");
        // Fetch the download URL
        Sound3.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                //Debug.Log("Download URL: " + task.Result);
                StartCoroutine(DownloadSound(task.Result, 3));
            }
        });
        var GTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("3").Child("ItemNames").GetValueAsync();

        yield return new WaitUntil(predicate: () => GTask.IsCompleted);

        if (GTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemNames = GTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemNames.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[2].PossibleAnswers[_itemNum] = childSnapshot.Key.ToString();
                _itemNum += 1;
            }
        }
        var HTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("3").Child("PointsToGive").GetValueAsync();

        yield return new WaitUntil(predicate: () => HTask.IsCompleted);

        if (HTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {HTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemPointtogive = HTask.Result;
            SeasonLevelItems.SoundsOfTheSeasonItemList[2].PointsToGive = int.Parse(SoundItemPointtogive.Value.ToString());
        }
        var ITask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("3").Child("Gifts").GetValueAsync();

        yield return new WaitUntil(predicate: () => ITask.IsCompleted);

        if (ITask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ITask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemGifts = ITask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemGifts.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[2].Gifts[_itemNum] = int.Parse(childSnapshot.Key.ToString());
                _itemNum += 1;
            }
        }

        StorageReference Sound4 = storage.GetReferenceFromUrl("gs://sounds-like-26713874.appspot.com/OnlineSounds/Sounds of the Season/4/sound.mp3");
        // Fetch the download URL
        Sound4.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                //Debug.Log("Download URL: " + task.Result);
                StartCoroutine(DownloadSound(task.Result, 4));
            }
        });
        var JTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("4").Child("ItemNames").GetValueAsync();

        yield return new WaitUntil(predicate: () => JTask.IsCompleted);

        if (JTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {JTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemNames = JTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemNames.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[3].PossibleAnswers[_itemNum] = childSnapshot.Key.ToString();
                _itemNum += 1;
            }
        }
        var KTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("4").Child("PointsToGive").GetValueAsync();

        yield return new WaitUntil(predicate: () => KTask.IsCompleted);

        if (KTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {KTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemPointtogive = KTask.Result;
            SeasonLevelItems.SoundsOfTheSeasonItemList[3].PointsToGive = int.Parse(SoundItemPointtogive.Value.ToString());
        }
        var LTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("4").Child("Gifts").GetValueAsync();

        yield return new WaitUntil(predicate: () => LTask.IsCompleted);

        if (LTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemGifts = LTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemGifts.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[3].Gifts[_itemNum] = int.Parse(childSnapshot.Key.ToString());
                _itemNum += 1;
            }
        }


        StorageReference Sound5 = storage.GetReferenceFromUrl("gs://sounds-like-26713874.appspot.com/OnlineSounds/Sounds of the Season/5/sound.mp3");
        // Fetch the download URL
        Sound5.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                //Debug.Log("Download URL: " + task.Result);
                StartCoroutine(DownloadSound(task.Result, 5));
            }
        });
        var MTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("5").Child("ItemNames").GetValueAsync();

        yield return new WaitUntil(predicate: () => MTask.IsCompleted);

        if (MTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {MTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemNames = MTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemNames.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[4].PossibleAnswers[_itemNum] = childSnapshot.Key.ToString();
                _itemNum += 1;
            }
        }
        var NTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("5").Child("PointsToGive").GetValueAsync();

        yield return new WaitUntil(predicate: () => NTask.IsCompleted);

        if (NTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {NTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemPointtogive = NTask.Result;
            SeasonLevelItems.SoundsOfTheSeasonItemList[4].PointsToGive = int.Parse(SoundItemPointtogive.Value.ToString());
        }
        var OTask = DBreference.Child("SoundsOfTheSeason").Child("SoundItems").Child("5").Child("Gifts").GetValueAsync();

        yield return new WaitUntil(predicate: () => OTask.IsCompleted);

        if (OTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {OTask.Exception}");
        }
        else
        {
            DataSnapshot SoundItemGifts = OTask.Result;
            int _itemNum = 0;
            foreach (DataSnapshot childSnapshot in SoundItemGifts.Children.Reverse<DataSnapshot>())
            {
                SeasonLevelItems.SoundsOfTheSeasonItemList[4].Gifts[_itemNum] = int.Parse(childSnapshot.Key.ToString());
                _itemNum += 1;
            }
        }
    }
    IEnumerator DownloadSound(System.Uri uri, int soundnumber)
    {
        switch(soundnumber)
        {
            case 1:
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
                {
                    yield return www.SendWebRequest();

                    if (www.isHttpError || www.isNetworkError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        SeasonLevelItems.SoundsOfTheSeasonItemList[0].ItemSound = DownloadHandlerAudioClip.GetContent(www);
                        AudioDownLoaded += 1;
                    }
                }
                break;
            case 2:
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
                {
                    yield return www.SendWebRequest();

                    if (www.isHttpError || www.isNetworkError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        SeasonLevelItems.SoundsOfTheSeasonItemList[1].ItemSound = DownloadHandlerAudioClip.GetContent(www);
                        AudioDownLoaded += 1;
                    }
                }
                break;
            case 3:
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
                {
                    yield return www.SendWebRequest();

                    if (www.isHttpError || www.isNetworkError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        SeasonLevelItems.SoundsOfTheSeasonItemList[2].ItemSound = DownloadHandlerAudioClip.GetContent(www);
                        AudioDownLoaded += 1;
                    }
                }
                break;
            case 4:
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
                {
                    yield return www.SendWebRequest();

                    if (www.isHttpError || www.isNetworkError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        SeasonLevelItems.SoundsOfTheSeasonItemList[3].ItemSound = DownloadHandlerAudioClip.GetContent(www);
                        AudioDownLoaded += 1;
                    }
                }
                break;
            case 5:
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
                {
                    yield return www.SendWebRequest();

                    if (www.isHttpError || www.isNetworkError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        SeasonLevelItems.SoundsOfTheSeasonItemList[4].ItemSound = DownloadHandlerAudioClip.GetContent(www);
                        AudioDownLoaded += 1;
                    }
                }
                break;
        }
    }

}
