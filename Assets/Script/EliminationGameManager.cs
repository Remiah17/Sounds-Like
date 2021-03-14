using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EliminationGameManager : MonoBehaviour
{
    [Header("SCRIPTS")]
    [SerializeField]
    private LoginScript theLoginScript;
    [SerializeField]
    private FirebaseOnlineChecker TheOnlineChecker;
    private SoundsScript theSoundScript;
    [Space(20)]
    [Header("SCENES")]
    [SerializeField]
    private string OnlineModeScene;
    [Space(20)]
    [Header("LOADING SCREEN")]
    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private Text LoadingScreenMessageText;
    [Space(20)]
    [Header("WELCOME SCREEN")]
    [SerializeField]
    private GameObject WelcomeScreen;
    [SerializeField]
    private Text CardToSetupNumberText;
    [SerializeField]
    private Text PhaseText;
    [SerializeField]
    private Text PhaseTimeLeftText;
    [SerializeField]
    private Text PlayerNumberText;
    [SerializeField]
    private GameObject[] SoundCardSetupIcon;
    [SerializeField]
    private GameObject[] SoundCardCheckIcon;
    [SerializeField]
    private Button WelcomeEnterButton;
    [SerializeField]
    private Text CoinRewardAmount;
    [SerializeField]
    private Text AvatarRewardAmount;
    [SerializeField]
    private Text SoundCardRewardAmount;
    [SerializeField]
    private Button[] ChooseASoundCardButton;
    private int ActiveRoom = 0;
    [Space(20)]
    [Header("SOUND CARDS")]
    [SerializeField]
    private GameObject SoundCardScreen;
    [SerializeField]
    private SoundCardDatabaseScriptableObject SoundCardDatabase;
    [SerializeField]
    private Transform SoundCardContent;
    [SerializeField]
    private GameObject SoundCardButtonElement;
    private int WelcomeSoundCardBeinSetup = 0;
    [SerializeField]
    private GameObject UseSoundCardScreen;
    private int ShowingSoundCardID;
    [Space(20)]
    [Header("SOUND CARD VIEW")]
    public Text SoundCardNameText;
    public Text SoundCardIDNumberText;
    public Image SoundCardImage;
    public Text SoundCardArtistText;
    public Image SoundCardDifficultyImage;
    public Text SoundCardPointsToGiveText;
    public Sprite[] SoundCardDifficultySprites;
    [Space(20)]
    [Header("WELCOME SCREEN")]
    [SerializeField]
    private GameObject WaitingScreen;
    [SerializeField]
    private Text WaitingPhaseText;
    [SerializeField]
    private Text WaitingPhaseTimeLeftText;
    [SerializeField]
    private Text WaitingPlayerNumberText;
    [SerializeField]
    private Text WaitingCoinRewardAmount;
    [SerializeField]
    private Text WaitingAvatarRewardAmount;
    [SerializeField]
    private Text WaitingSoundCardRewardAmount;
    [SerializeField]
    private Transform WaitingSoundCardContent;
    [SerializeField]
    private GameObject DoyouWantToQuitScreen;
    [SerializeField]
    private GameObject NotEnoughCoinWarningScreen;
    [Space(20)]
    [Header("GAME SCREEN")]
    [SerializeField]
    private GameObject GameScreen;
    [SerializeField]
    private Text GamePhaseText;
    [SerializeField]
    private Text GamePhaseTimeLeftText;
    [SerializeField]
    private Text GameTotalScoreText;
    [SerializeField]
    private int[] GameActiveSoundCardsID;
    [SerializeField]
    private string[] GameActiveSoundCardsOwner;
    private int PlayingSoundCards;
    private int ActiveCardNumber;
    [SerializeField]
    private GameObject GameGuessSoundCardScreen;
    [SerializeField]
    private InputField GameGuessSoundCardAnswerInput;
    [SerializeField]
    private GameObject GameCorrectAnswerScreen;
    [SerializeField]
    private GameObject GameWrongAnswerScreen;
    [SerializeField]
    private Button[] SoundCardsGuessButtons;
    [SerializeField]
    private GameObject[] SoundCardGuessButtonsPlayStatus;
    [SerializeField]
    private GameObject[] SoundCardGuessButtonsCheckStatus;
    [SerializeField]
    private GameObject[] SoundCardGuessButtonsWrongStatus;
    private float TimerBonus = 100;
    [Space(20)]
    [Header("GAME SCREEN")]
    [SerializeField]
    private GameObject ResultScreen;
    [SerializeField]
    private Text ResultTimeLeftText;
    [SerializeField]
    private PlayersAvatarElement FirstPlayerAvatar;
    [SerializeField]
    private PlayersAvatarElement SecondPlayerAvatar;
    [SerializeField]
    private PlayersAvatarElement ThirdPlayerAvatar;
    [Space(20)]
    [Header("SETTING SCREEN")]
    [SerializeField]
    private GameObject SettingScreen;


    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        StartCoroutine(EnterRoomSequence());
    }

    private void Update()
    {
        if(GameGuessSoundCardScreen.activeInHierarchy)
        {
            TimerBonus -= 1 * Time.deltaTime;
        }
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
            SettingScreen.SetActive(false);
        }
        else
        {
            SettingScreen.SetActive(true);
        }
    }

    private void InitializeWelcomeRoom(int RoomID)
    {
        WelcomeScreen.SetActive(true);
        WaitingScreen.SetActive(false);
        GameScreen.SetActive(false);
        ResultScreen.SetActive(false);
        StartCoroutine(WelcomeRoomRefresh(RoomID));
    }

    public void OnClick_WelcomeRoom_Back()
    {
        theSoundScript.PlaySelectSound();
        for (int i = 1; i < SoundCardDatabase.AmountBeingUsed.Length; i++)
        {
            SoundCardDatabase.AmountBeingUsed[i] = 0;
        }
        StartCoroutine(RemovePlayerToRoom(ActiveRoom));
    }

    public void OnClick_WelcomeRoom_ChooseSoundCard(int CardNumber)
    {
        theSoundScript.PlaySelectSound();
        if (SoundCardScreen.activeInHierarchy)
        {
            SoundCardScreen.SetActive(false);
            WelcomeSoundCardBeinSetup = 0;
        }
        else
        {
            SoundCardScreen.SetActive(true);
            WelcomeSoundCardBeinSetup = CardNumber;
            //destroy any existing scoreboard elements
            foreach (Transform child in SoundCardContent.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 1; i < SoundCardDatabase.SoundCardList.Length; i++)
            {
                if(SoundCardDatabase.SoundCardUnlocked[i] && SoundCardDatabase.SoundCardAmount[i] > SoundCardDatabase.AmountBeingUsed[i])
                {
                    GameObject soundcardelem = Instantiate(SoundCardButtonElement, SoundCardContent);
                    soundcardelem.GetComponent<SoundCardButtonElement>().NewSoundCardElementButton(i);
                }
            }
        }
    }

    public void Show_SelectSoundCard(int soundcardid)
    {
        theSoundScript.PlaySelectSound();
        SoundCardNameText.text = "Item: " + SoundCardDatabase.SoundCardList[soundcardid].CampaignItemData.ItemName;
        SoundCardIDNumberText.text = SoundCardDatabase.SoundCardList[soundcardid].SoundCardID.ToString();
        SoundCardImage.sprite = SoundCardDatabase.SoundCardList[soundcardid].CampaignItemData.ItemSprite;
        SoundCardArtistText.text = "Music by " + SoundCardDatabase.SoundCardList[soundcardid].ArtistName;
        SoundCardDifficultyImage.sprite = SoundCardDifficultySprites[SoundCardDatabase.SoundCardList[soundcardid].Difficulty - 1];
        SoundCardPointsToGiveText.text = "Points: " + SoundCardDatabase.SoundCardList[soundcardid].PointsToGive.ToString();
        UseSoundCardScreen.SetActive(true);
        ShowingSoundCardID = soundcardid;
    }

    public void Hide_SelectSoundCard(int soundcardid)
    {
        theSoundScript.PlaySelectSound();
        SoundCardNameText.text = "";
        SoundCardIDNumberText.text = "";
        SoundCardImage.sprite = null;
        SoundCardArtistText.text = "";
        SoundCardDifficultyImage.sprite = null;
        SoundCardPointsToGiveText.text = "";
        UseSoundCardScreen.SetActive(false);
        ShowingSoundCardID = 0;
    }

    public void OnClick_UseSoundCard()
    {
        theSoundScript.PlaySelectSound();
        SoundCardDatabase.AmountBeingUsed[ShowingSoundCardID] += 1;
        StartCoroutine(UpdatePlayerSoundCard(WelcomeSoundCardBeinSetup, ShowingSoundCardID));
    }

    public void OnClick_WelcomeRoom_EnterRoom()
    {
        theSoundScript.PlaySelectSound();
        StartCoroutine(UpdatePlayerInGame());
    }

    private void InitializeWaitingRoom(int RoomID)
    {
        WaitingScreen.SetActive(true);
        WelcomeScreen.SetActive(false);
        GameScreen.SetActive(false);
        ResultScreen.SetActive(false);
        StartCoroutine(WaitingRoomRefresh(RoomID));
    }

    public void OnClick_WaitingRoom_Quit()
    {
        theSoundScript.PlaySelectSound();
        DoyouWantToQuitScreen.SetActive(true);
    }
    public void OnClick_WaitingRoom_NoQuitFor50Coins()
    {
        theSoundScript.PlaySelectSound();
        DoyouWantToQuitScreen.SetActive(false);
    }
    public void OnClick_WaitingRoom_YesQuitFor50Coins()
    {
        theSoundScript.PlaySelectSound();
        if (PlayerPrefs.GetInt("Coins") >= 50)
        {
            for (int i = 1; i < SoundCardDatabase.AmountBeingUsed.Length; i++)
            {
                SoundCardDatabase.AmountBeingUsed[i] = 0;
            }
            StartCoroutine(RemovePlayerToRoom(ActiveRoom));
        }
        else
        {
            NotEnoughCoinWarningScreen.SetActive(true);
            DoyouWantToQuitScreen.SetActive(false);
        }
    }
    public void OnClick_WaitingRoom_OkayNotEnoughCoinsWarning()
    {
        theSoundScript.PlaySelectSound();
        NotEnoughCoinWarningScreen.SetActive(false);
        DoyouWantToQuitScreen.SetActive(false);
    }

    private void InitializeGameRoom(int RoomID)
    {
        WaitingScreen.SetActive(false);
        WelcomeScreen.SetActive(false);
        GameScreen.SetActive(true);
        ResultScreen.SetActive(false);
        StartCoroutine(GameRoomRefresh(RoomID));
    }

    public void OnClick_GameRoomGuessSound(int cardnumber)
    {
        theSoundScript.PlaySelectSound();
        if (GameGuessSoundCardScreen.activeInHierarchy)
        {
            ActiveCardNumber = 0;
            PlayingSoundCards = 0;
            GameGuessSoundCardAnswerInput.text = "";
            GameGuessSoundCardScreen.SetActive(false);
            StartCoroutine(GameRoomRefresh(ActiveRoom));
        }
        else
        {
            GameGuessSoundCardAnswerInput.text = "";
            ActiveCardNumber = cardnumber;
            PlayingSoundCards = GameActiveSoundCardsID[cardnumber];
            GameGuessSoundCardScreen.SetActive(true);
            TimerBonus = 100;
            OnClick_GameRoomPlaySound();
        }
    }

    public void OnClick_GameRoomPlaySound()
    {
        theSoundScript.PlaySpecificFX(SoundCardDatabase.SoundCardList[PlayingSoundCards].CampaignItemData.ItemSound);
    }

    public void OnClic_GameRoomConfirmAnswer()
    {
        theSoundScript.PlaySelectSound();
        string Answer = GameGuessSoundCardAnswerInput.text;
        int correctAswer = 0;
        for (int i = 0; i < SoundCardDatabase.SoundCardList[PlayingSoundCards].CampaignItemData.PossibleAnswers.Length; i++)
        {
            if(Answer == SoundCardDatabase.SoundCardList[PlayingSoundCards].CampaignItemData.PossibleAnswers[i])
            {
                correctAswer += 1;
            }
        }

        if(correctAswer > 0)
        {
            StartCoroutine(UpdatePlayerScore(SoundCardDatabase.SoundCardList[PlayingSoundCards].PointsToGive, theLoginScript.User.UserId.ToString()));
            StartCoroutine(UpdatePlayerSoundCardsRecord(1));
            ShowHide_CorrectAnswerScreen();
        }
        else
        {
            StartCoroutine(UpdatePlayerScore(SoundCardDatabase.SoundCardList[PlayingSoundCards].PointsToGive, GameActiveSoundCardsOwner[ActiveCardNumber]));
            StartCoroutine(UpdatePlayerSoundCardsRecord(2));
            ShowHide_WrongAnswerScreen();
        }
    }

    public void ShowHide_CorrectAnswerScreen()
    {
        if (GameCorrectAnswerScreen.activeInHierarchy)
        {
            theSoundScript.PlaySelectSound();
            GameCorrectAnswerScreen.SetActive(false);
            ActiveCardNumber = 0;
            PlayingSoundCards = 0;
            GameGuessSoundCardScreen.SetActive(false);
            StartCoroutine(GameRoomRefresh(ActiveRoom));
        }
        else
        {
            GameCorrectAnswerScreen.SetActive(true);
            theSoundScript.PlayCorrectSound();
        }
    }

    public void ShowHide_WrongAnswerScreen()
    {
        if (GameWrongAnswerScreen.activeInHierarchy)
        {
            theSoundScript.PlaySelectSound();
            GameWrongAnswerScreen.SetActive(false);
            ActiveCardNumber = 0;
            PlayingSoundCards = 0;
            GameGuessSoundCardScreen.SetActive(false);
            StartCoroutine(GameRoomRefresh(ActiveRoom));
        }
        else
        {
            GameWrongAnswerScreen.SetActive(true);
            theSoundScript.PlayWrongSound();
        }
    }

    private void InitializeResultRoom(int RoomID)
    {
        WaitingScreen.SetActive(false);
        WelcomeScreen.SetActive(false);
        GameScreen.SetActive(false);
        ResultScreen.SetActive(true);
        StartCoroutine(ResultRoomRefresh(RoomID));
    }

    private void ShowLoadingScreen()
    {
        LoadingScreen.SetActive(true);
    }

    private void HideLoadingScreen()
    {
        LoadingScreen.SetActive(false);
    }

    IEnumerator EnterRoomSequence()
    {
        ShowLoadingScreen();
        //CHECK IF PLAYER IS IN ANY ROOM
        int playerRoomLocation = 0;
        bool playerisinaroom = false;
        int thisPlayerEnteredTheRoom = 0;
        string AllReward = "";
        string ThirdCoinReward = "";
        string ThirdAvatarReward = "";
        string ThirdSoundCardReward = "";
        string SecondCoinReward = "";
        string SecondAvatarReward = "";
        string SecondSoundCardReward = "";
        string FirstCoinReward = "";
        string FirstAvatarReward = "";
        string FirstSoundCardReward = "";
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").GetValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);
        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot roomssnapshot = ATask.Result;
            foreach (DataSnapshot roomSnapshot in roomssnapshot.Children.Reverse<DataSnapshot>())
            {
                if(roomSnapshot.Key != 0.ToString())
                {
                    var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(roomSnapshot.Key.ToString()).Child("Players").GetValueAsync();

                    yield return new WaitUntil(predicate: () => BTask.IsCompleted);

                    if (BTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
                    }
                    else
                    {
                        DataSnapshot playerssnapshot = BTask.Result;
                        foreach (DataSnapshot playerSnapshot in playerssnapshot.Children.Reverse<DataSnapshot>())
                        {
                            if(playerSnapshot.Key != 0.ToString())
                            {
                                string checkinguserid = playerSnapshot.Key.ToString();
                                string thisuserid = theLoginScript.User.UserId.ToString();
                                if (checkinguserid == thisuserid)
                                {
                                    playerisinaroom = true;
                                    playerRoomLocation = int.Parse(roomSnapshot.Key.ToString());
                                    thisPlayerEnteredTheRoom = int.Parse(playerSnapshot.Child("InGame").Value.ToString());
                                }
                            }
                        }
                    }
                }
            }

            var GTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rewards").GetValueAsync();
            yield return new WaitUntil(predicate: () => GTask.IsCompleted);
            if (GTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
            }
            else
            {
                DataSnapshot rewardssnapshot = GTask.Result;
                foreach(DataSnapshot rewardsnapshot in rewardssnapshot.Children)
                {
                    string checkingreward = rewardsnapshot.Key.ToString();
                    switch(checkingreward)
                    {
                        case "All":
                            AllReward = rewardsnapshot.Child("CoinReward").Value.ToString();
                            break;
                        case "First":
                            FirstCoinReward = rewardsnapshot.Child("CoinReward").Value.ToString();
                            FirstAvatarReward = rewardsnapshot.Child("AvatarReward").Value.ToString();
                            FirstSoundCardReward = rewardsnapshot.Child("SoundCardReward").Value.ToString();
                            break;
                        case "Second":
                            SecondCoinReward = rewardsnapshot.Child("CoinReward").Value.ToString();
                            SecondAvatarReward = rewardsnapshot.Child("AvatarReward").Value.ToString();
                            SecondSoundCardReward = rewardsnapshot.Child("SoundCardReward").Value.ToString();
                            break;
                        case "Third":
                            ThirdCoinReward = rewardsnapshot.Child("CoinReward").Value.ToString();
                            ThirdAvatarReward = rewardsnapshot.Child("AvatarReward").Value.ToString();
                            ThirdSoundCardReward = rewardsnapshot.Child("SoundCardReward").Value.ToString();
                            break;
                    }
                }
            }

            //IF IN A ROOM
            if (playerisinaroom)
            {
                Debug.Log("Player is in room" + playerRoomLocation);
                //CHECK ROOM PHASE
                var CTask = theLoginScript.DBreference.Child("EliminationGame").Child("Phase").GetValueAsync();
                yield return new WaitUntil(predicate: () => CTask.IsCompleted);
                if (CTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
                }
                else
                {
                    DataSnapshot roomphasesnapshot = CTask.Result;
                    string CurrentPhase = roomphasesnapshot.Value.ToString();

                    switch (CurrentPhase)
                    {
                        case "Setup":
                            ActiveRoom = playerRoomLocation;
                            if(thisPlayerEnteredTheRoom > 0)
                            {
                                InitializeWaitingRoom(playerRoomLocation);
                            }
                            else
                            {
                                InitializeWelcomeRoom(playerRoomLocation);
                            }
                            break;
                        case "Round1":
                            ActiveRoom = playerRoomLocation;
                            if(thisPlayerEnteredTheRoom > 0)
                            {
                                InitializeGameRoom(playerRoomLocation);
                            }
                            else
                            {
                                LoadingScreenMessageText.gameObject.SetActive(true);
                                LoadingScreenMessageText.text = "Sorry, Elimination game is in progress. Wait until the next SETUP day.";
                                yield return new WaitForSeconds(2);
                                HideLoadingScreen();
                                OnClickBack();
                            }
                            break;
                        case "Round2":
                            if (thisPlayerEnteredTheRoom > 1)
                            {
                                InitializeGameRoom(playerRoomLocation);
                            }
                            else
                            {
                                LoadingScreenMessageText.gameObject.SetActive(true);
                                LoadingScreenMessageText.text = "Sorry, You did not make it to the next round. You got " + AllReward + " coins.";
                                yield return new WaitForSeconds(2);
                                HideLoadingScreen();
                                OnClickBack();
                            }
                            break;
                        case "Round3":
                            if (thisPlayerEnteredTheRoom > 2)
                            {
                                InitializeGameRoom(playerRoomLocation);
                            }
                            else
                            {
                                LoadingScreenMessageText.gameObject.SetActive(true);
                                LoadingScreenMessageText.text = "Sorry, You did not make it to the next round. You got " + AllReward + " coins.";
                                yield return new WaitForSeconds(2);
                                HideLoadingScreen();
                                OnClickBack();
                            }
                            break;
                        case "Round4":
                            if (thisPlayerEnteredTheRoom > 3)
                            {
                                InitializeGameRoom(playerRoomLocation);
                            }
                            else
                            {
                                LoadingScreenMessageText.gameObject.SetActive(true);
                                LoadingScreenMessageText.text = "Sorry, You did not make it to the next round. You got " + AllReward + " coins.";
                                yield return new WaitForSeconds(2);
                                HideLoadingScreen();
                                OnClickBack();
                            }
                            break;
                        case "Round5":
                            if (thisPlayerEnteredTheRoom > 4)
                            {
                                InitializeGameRoom(playerRoomLocation);
                            }
                            else if (thisPlayerEnteredTheRoom == 4)
                            {
                                LoadingScreenMessageText.gameObject.SetActive(true);
                                LoadingScreenMessageText.text = "Sorry, You did not make it to the next round. You got " + ThirdCoinReward + " coins, " + ThirdAvatarReward + " avatar, and " + ThirdSoundCardReward + " sound cards.";
                                yield return new WaitForSeconds(2);
                                HideLoadingScreen();
                                OnClickBack();
                            }
                            else
                            {
                                LoadingScreenMessageText.gameObject.SetActive(true);
                                LoadingScreenMessageText.text = "Sorry, You did not make it to the next round. You got " + AllReward + " coins.";
                                yield return new WaitForSeconds(2);
                                HideLoadingScreen();
                                OnClickBack();
                            }
                            break;
                        case "Result":
                            InitializeResultRoom(playerRoomLocation);
                            break;
                    }

                }
            }
            //IF NOT IN A ROOM
            else
            {
                //LOOK FOR AVAILABLE ROOM
                int LastRoomChecked = 0;
                bool roomfound = false;
                int roomfoundID = 0;
                string CurrentPhase = "";
                var DTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").GetValueAsync();
                yield return new WaitUntil(predicate: () => DTask.IsCompleted);

                if (DTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
                }
                else
                {
                    DataSnapshot rooms2snapshot = DTask.Result;
                    foreach (DataSnapshot room2Snapshot in rooms2snapshot.Children.Reverse<DataSnapshot>())
                    {
                        if (room2Snapshot.Key != 0.ToString())
                        {
                            Debug.Log("Checking" + room2Snapshot.Key);
                            var ETask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(room2Snapshot.Key.ToString()).Child("Players").GetValueAsync();

                            yield return new WaitUntil(predicate: () => ETask.IsCompleted);

                            if (ETask.Exception != null)
                            {
                                Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
                            }
                            else
                            {
                                DataSnapshot playerssnapshot = ETask.Result;
                                int numberofplayers = 0;
                                foreach (DataSnapshot playerSnapshot in playerssnapshot.Children.Reverse<DataSnapshot>())
                                {
                                    if (playerSnapshot.Key != 0.ToString())
                                    {
                                        numberofplayers += 1;
                                    }
                                }

                                if (numberofplayers < 5)
                                {
                                    Debug.Log("number of players in room is " + numberofplayers);
                                    roomfound = true;
                                    roomfoundID = int.Parse(room2Snapshot.Key.ToString());
                                    break;
                                }
                                else
                                {
                                    Debug.Log("number of players in room is " + numberofplayers);
                                    LastRoomChecked = int.Parse(rooms2snapshot.Key.ToString());
                                }
                            }
                        }
                    }

                    var FTask = theLoginScript.DBreference.Child("EliminationGame").Child("Phase").GetValueAsync();

                    yield return new WaitUntil(predicate: () => FTask.IsCompleted);

                    if (FTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
                    }
                    else
                    {
                        DataSnapshot phasesnapshot = FTask.Result;
                        CurrentPhase = phasesnapshot.Value.ToString();
                        Debug.Log("The current phase is " + CurrentPhase);
                    }
                    string SetupName = "Setup";
                    //IF THERE IS AVAILABLE ROOM
                    if (roomfound)
                    {
                        Debug.Log("Players is found in room");
                        if (CurrentPhase == SetupName)
                        {
                            StartCoroutine(AddPlayerToRoom(roomfoundID));
                            Debug.Log("Adding player to the room");
                        }
                        else
                        {
                            LoadingScreenMessageText.gameObject.SetActive(true);
                            LoadingScreenMessageText.text = "Sorry, Elimination game is in progress. Wait until the next SETUP day.";
                            yield return new WaitForSeconds(2);
                            HideLoadingScreen();
                            OnClickBack();
                        }
                    }
                    //IF ALL ROOM IS FULL
                    else
                    {
                        Debug.Log("Players is not in a room");
                        if (CurrentPhase == SetupName)
                        {
                            Debug.Log("Creating room");
                            StartCoroutine(CreateRoom(LastRoomChecked + 1, CurrentPhase));
                        }
                        else
                        {
                            LoadingScreenMessageText.text = "Sorry, Elimination game is in progress. Wait until the next SETUP day.";
                            yield return new WaitForSeconds(2);
                            HideLoadingScreen();
                            OnClickBack();
                        }

                    }
                }
            }
        }
    }

    IEnumerator WelcomeRoomRefresh(int RoomID)
    {
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Phase").GetValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot phasesnapshot = ATask.Result;
            PhaseText.text = "SETUP TIME ENDS IN:";
        }
        var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("PhaseTimeLeft").GetValueAsync();
        yield return new WaitUntil(predicate: () => BTask.IsCompleted);

        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {
            DataSnapshot phasetimeleftsnapshot = BTask.Result;
            float timeleftinminutes = float.Parse(phasetimeleftsnapshot.Value.ToString());
            float timeleftinhours = timeleftinminutes / 60;
            if(timeleftinminutes > 60)
            {
                PhaseTimeLeftText.text = timeleftinhours + " HOUR/S";
            }
            else
            {
                PhaseTimeLeftText.text = timeleftinminutes + " MINUTE/S";
            }
        }
        var CTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").GetValueAsync();
        yield return new WaitUntil(predicate: () => CTask.IsCompleted);

        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {
            DataSnapshot soundcardssnapshot = CTask.Result;
            int unsetcards = 0;
            int checkingcard = 0;
            foreach (DataSnapshot soundcardSnapshot in soundcardssnapshot.Children)
            {
                int cardID = int.Parse(soundcardSnapshot.Value.ToString());

                if (cardID == 0)
                {
                    SoundCardSetupIcon[checkingcard].SetActive(true);
                    SoundCardCheckIcon[checkingcard].SetActive(false);
                    ChooseASoundCardButton[checkingcard].interactable = true;
                    unsetcards += 1;
                }
                else
                {
                    SoundCardCheckIcon[checkingcard].SetActive(true);
                    SoundCardSetupIcon[checkingcard].SetActive(false);
                    ChooseASoundCardButton[checkingcard].interactable = false;
                }
                checkingcard += 1;
            }

            CardToSetupNumberText.text = "" + unsetcards;

            if(unsetcards > 0)
            {
                WelcomeEnterButton.interactable = false;
            }
            else
            {
                WelcomeEnterButton.interactable = true;
            }
        }
        var DTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").GetValueAsync();
        yield return new WaitUntil(predicate: () => DTask.IsCompleted);

        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {
            DataSnapshot playerssnapshot = DTask.Result;
            int numberofplayers = 0;
            foreach (DataSnapshot playerSnapshot in playerssnapshot.Children.Reverse<DataSnapshot>())
            {
                if(playerSnapshot.Key != 0.ToString() && playerSnapshot.Key != theLoginScript.User.UserId)
                {
                    numberofplayers += 1;
                }
            }

            PlayerNumberText.text = numberofplayers + " / 5";
        }

        var ETask = theLoginScript.DBreference.Child("EliminationGame").Child("Rewards").Child("First").Child("CoinReward").GetValueAsync();
        yield return new WaitUntil(predicate: () => ETask.IsCompleted);

        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {
            DataSnapshot coinsnapshot = ETask.Result;
            int coinreward = int.Parse(coinsnapshot.Value.ToString());
            CoinRewardAmount.text = "" + coinreward;
        }
        var FTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rewards").Child("First").Child("AvatarReward").GetValueAsync();
        yield return new WaitUntil(predicate: () => FTask.IsCompleted);

        if (FTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
        }
        else
        {
            DataSnapshot avatarsnapshot = FTask.Result;
            int avatarreward = int.Parse(avatarsnapshot.Value.ToString());
            AvatarRewardAmount.text = "" + avatarreward;
        }
        var GTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rewards").Child("First").Child("SoundCardReward").GetValueAsync();
        yield return new WaitUntil(predicate: () => GTask.IsCompleted);

        if (GTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
        }
        else
        {
            DataSnapshot soundcarsnapshot = GTask.Result;
            int screward = int.Parse(soundcarsnapshot.Value.ToString());
            SoundCardRewardAmount.text = "" + screward;
        }
        HideLoadingScreen();
    }

    IEnumerator WaitingRoomRefresh(int RoomID)
    {
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Phase").GetValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot phasesnapshot = ATask.Result;
            WaitingPhaseText.text = "SETUP TIME ENDS IN:";
        }
        var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("PhaseTimeLeft").GetValueAsync();
        yield return new WaitUntil(predicate: () => BTask.IsCompleted);

        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {
            DataSnapshot phasetimeleftsnapshot = BTask.Result;
            float timeleftinminutes = float.Parse(phasetimeleftsnapshot.Value.ToString());
            float timeleftinhours = timeleftinminutes / 60;
            if (timeleftinminutes > 60)
            {
                WaitingPhaseTimeLeftText.text = timeleftinhours + " HOUR/S";
            }
            else
            {
                WaitingPhaseTimeLeftText.text = timeleftinminutes + " MINUTE/S";
            }
        }
        var CTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").GetValueAsync();
        yield return new WaitUntil(predicate: () => CTask.IsCompleted);

        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {
            DataSnapshot soundcardssnapshot = CTask.Result;
            foreach (Transform child in WaitingSoundCardContent.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (DataSnapshot soundcardSnapshot in soundcardssnapshot.Children)
            {
                int cardID = int.Parse(soundcardSnapshot.Value.ToString());
                GameObject soundcardelem = Instantiate(SoundCardButtonElement, WaitingSoundCardContent);
                soundcardelem.GetComponent<SoundCardButtonElement>().NewSoundCardElementButton(cardID);
                soundcardelem.GetComponent<Button>().interactable = false;
            }
        }
        var DTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").GetValueAsync();
        yield return new WaitUntil(predicate: () => DTask.IsCompleted);

        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {
            DataSnapshot playerssnapshot = DTask.Result;
            int numberofplayers = 0;
            foreach (DataSnapshot playerSnapshot in playerssnapshot.Children.Reverse<DataSnapshot>())
            {
                if (playerSnapshot.Key != 0.ToString())
                {
                    numberofplayers += 1;
                }
            }

            WaitingPlayerNumberText.text = numberofplayers + " / 5";
        }

        var ETask = theLoginScript.DBreference.Child("EliminationGame").Child("Rewards").Child("First").Child("CoinReward").GetValueAsync();
        yield return new WaitUntil(predicate: () => ETask.IsCompleted);

        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {
            DataSnapshot coinsnapshot = ETask.Result;
            int coinreward = int.Parse(coinsnapshot.Value.ToString());
            WaitingCoinRewardAmount.text = "" + coinreward;
        }
        var FTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rewards").Child("First").Child("AvatarReward").GetValueAsync();
        yield return new WaitUntil(predicate: () => FTask.IsCompleted);

        if (FTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
        }
        else
        {
            DataSnapshot avatarsnapshot = FTask.Result;
            int avatarreward = int.Parse(avatarsnapshot.Value.ToString());
            WaitingAvatarRewardAmount.text = "" + avatarreward;
        }
        var GTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rewards").Child("First").Child("SoundCardReward").GetValueAsync();
        yield return new WaitUntil(predicate: () => GTask.IsCompleted);

        if (GTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
        }
        else
        {
            DataSnapshot soundcarsnapshot = GTask.Result;
            int screward = int.Parse(soundcarsnapshot.Value.ToString());
            WaitingSoundCardRewardAmount.text = "" + screward;
        }
        HideLoadingScreen();
    }

    IEnumerator GameRoomRefresh(int RoomID)
    {
        string CurrentRound = "";
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Phase").GetValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot phasesnapshot = ATask.Result;
            GamePhaseText.text = phasesnapshot.Value + " ENDS IN:";
            CurrentRound = phasesnapshot.Value.ToString();
        }
        var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("PhaseTimeLeft").GetValueAsync();
        yield return new WaitUntil(predicate: () => BTask.IsCompleted);
        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {
            DataSnapshot phasetimeleftsnapshot = BTask.Result;
            float timeleftinminutes = float.Parse(phasetimeleftsnapshot.Value.ToString());
            float timeleftinhours = timeleftinminutes / 60;
            if (timeleftinminutes > 60)
            {
                GamePhaseTimeLeftText.text = timeleftinhours + " HOUR/S";
            }
            else
            {
                GamePhaseTimeLeftText.text = timeleftinminutes + " MINUTE/S";
            }
        }
        var CTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId.ToString()).Child("TotalPoints").GetValueAsync();
        yield return new WaitUntil(predicate: () => CTask.IsCompleted);
        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {
            DataSnapshot totalPointssnapshot = CTask.Result;

            GameTotalScoreText.text = totalPointssnapshot.Value.ToString();
        }

        var DTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("SoundCards").GetValueAsync();
        yield return new WaitUntil(predicate: () => DTask.IsCompleted);
        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {
            DataSnapshot SoundCardsRoundsnapshot = DTask.Result;

            foreach (DataSnapshot soundcardroundSnapshot in SoundCardsRoundsnapshot.Children)
            {
                string checkinground = soundcardroundSnapshot.Key.ToString();
                if (CurrentRound == checkinground)
                {
                    GameActiveSoundCardsID[1] = int.Parse(soundcardroundSnapshot.Child("1").Child("ID").Value.ToString());
                    GameActiveSoundCardsOwner[1] = soundcardroundSnapshot.Child("1").Child("Owner").Value.ToString();
                    GameActiveSoundCardsID[2] = int.Parse(soundcardroundSnapshot.Child("2").Child("ID").Value.ToString());
                    GameActiveSoundCardsOwner[2] = soundcardroundSnapshot.Child("2").Child("Owner").Value.ToString();
                    GameActiveSoundCardsID[3] = int.Parse(soundcardroundSnapshot.Child("3").Child("ID").Value.ToString());
                    GameActiveSoundCardsOwner[3] = soundcardroundSnapshot.Child("3").Child("Owner").Value.ToString();
                    GameActiveSoundCardsID[4] = int.Parse(soundcardroundSnapshot.Child("4").Child("ID").Value.ToString());
                    GameActiveSoundCardsOwner[4] = soundcardroundSnapshot.Child("4").Child("Owner").Value.ToString();
                    GameActiveSoundCardsID[5] = int.Parse(soundcardroundSnapshot.Child("5").Child("ID").Value.ToString());
                    GameActiveSoundCardsOwner[5] = soundcardroundSnapshot.Child("5").Child("Owner").Value.ToString();
                }
            }
        }

        var ETask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId.ToString()).Child("SoundCardRecord").GetValueAsync();
        yield return new WaitUntil(predicate: () => ETask.IsCompleted);
        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {
            DataSnapshot SoundCardsRecordsnapshot = ETask.Result;
            int checkingcard = 1;
            foreach (DataSnapshot soundcardrecordSnapshot in SoundCardsRecordsnapshot.Children)
            {
                int checkingsoundcardstatus = int.Parse(soundcardrecordSnapshot.Value.ToString());
                switch(checkingsoundcardstatus)
                {
                    case 0:
                        SoundCardsGuessButtons[checkingcard].interactable = true;
                        SoundCardGuessButtonsPlayStatus[checkingcard].SetActive(true);
                        SoundCardGuessButtonsCheckStatus[checkingcard].SetActive(false);
                        SoundCardGuessButtonsWrongStatus[checkingcard].SetActive(false);
                        break;
                    case 1:
                        SoundCardsGuessButtons[checkingcard].interactable = false;
                        SoundCardGuessButtonsPlayStatus[checkingcard].SetActive(false);
                        SoundCardGuessButtonsCheckStatus[checkingcard].SetActive(true);
                        SoundCardGuessButtonsWrongStatus[checkingcard].SetActive(false);
                        break;
                    case 2:
                        SoundCardsGuessButtons[checkingcard].interactable = false;
                        SoundCardGuessButtonsPlayStatus[checkingcard].SetActive(false);
                        SoundCardGuessButtonsCheckStatus[checkingcard].SetActive(false);
                        SoundCardGuessButtonsWrongStatus[checkingcard].SetActive(true);
                        break;
                }
                checkingcard += 1;
            }
        }
        HideLoadingScreen();
    }

    IEnumerator ResultRoomRefresh(int RoomID)
    {
        string FirstPlacerID = "";
        string SecondPlacerID = "";
        string ThirdPlacerID = "";
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("PhaseTimeLeft").GetValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);
        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot phasetimeleftsnapshot = ATask.Result;
            float timeleftinminutes = float.Parse(phasetimeleftsnapshot.Value.ToString());
            float timeleftinhours = timeleftinminutes / 60;
            if (timeleftinminutes > 60)
            {
                ResultTimeLeftText.text = "The next elimination game setup will start in " + timeleftinhours + " HOUR/S";
            }
            else
            {
                ResultTimeLeftText.text = "The next elimination game setup will start in " + timeleftinminutes + " MINUTE/S";
            }
        }

        var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Places").Child("First").GetValueAsync();
        yield return new WaitUntil(predicate: () => BTask.IsCompleted);
        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {
            DataSnapshot FirstPlaceIDssnapshot = BTask.Result;
            FirstPlacerID = FirstPlaceIDssnapshot.Value.ToString();
        }

        var CTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Places").Child("Second").GetValueAsync();
        yield return new WaitUntil(predicate: () => CTask.IsCompleted);
        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {
            DataSnapshot SecondPlaceIDssnapshot = CTask.Result;
            SecondPlacerID = SecondPlaceIDssnapshot.Value.ToString();
        }

        var DTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Places").Child("Third").GetValueAsync();
        yield return new WaitUntil(predicate: () => DTask.IsCompleted);
        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {
            DataSnapshot ThirdPlaceIDssnapshot = DTask.Result;
            ThirdPlacerID = ThirdPlaceIDssnapshot.Value.ToString();
        }

        var ETask = theLoginScript.DBreference.Child("users").GetValueAsync();
        yield return new WaitUntil(predicate: () => ETask.IsCompleted);
        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {
            DataSnapshot UsersIDssnapshot = ETask.Result;
            foreach (DataSnapshot playerSnapshot in UsersIDssnapshot.Children.Reverse<DataSnapshot>())
            {
                if (playerSnapshot.Key.ToString() == FirstPlacerID)
                {
                    string username = playerSnapshot.Child("username").Value.ToString();
                    int bodyID = int.Parse(playerSnapshot.Child("Avatar").Child("Body").Child("BodyID").Value.ToString());
                    Color bodyColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Body").Child("BodyColor").Value.ToString());
                    int UTID = int.Parse(playerSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoID").Value.ToString());
                    Color UTColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoColor").Value.ToString());
                    int lowertorsoID = int.Parse(playerSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoID").Value.ToString());
                    Color LTColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoColor").Value.ToString());
                    int hairID = int.Parse(playerSnapshot.Child("Avatar").Child("Hair").Child("HairID").Value.ToString());
                    Color hairColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Hair").Child("HairColor").Value.ToString());
                    int eyesID = int.Parse(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyesID").Value.ToString());
                    Color pupilColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Pupils").Child("PupilsColor").Value.ToString());
                    int eyebrowsID = int.Parse(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsID").Value.ToString());
                    Color eyebrowsColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsColor").Value.ToString());
                    int noseID = int.Parse(playerSnapshot.Child("Avatar").Child("Nose").Child("NoseID").Value.ToString());
                    int mouthID = int.Parse(playerSnapshot.Child("Avatar").Child("Mouth").Child("MouthID").Value.ToString());

                    FirstPlayerAvatar.NewAvatarElement(username,bodyID, bodyColor, UTID, UTColor, lowertorsoID, LTColor, hairID, hairColor, eyesID, pupilColor, eyesID, eyebrowsID, eyebrowsColor, noseID, mouthID);
                }
                else if (playerSnapshot.Key.ToString() == SecondPlacerID)
                {
                    string username = playerSnapshot.Child("username").Value.ToString();
                    int bodyID = int.Parse(playerSnapshot.Child("Avatar").Child("Body").Child("BodyID").Value.ToString());
                    Color bodyColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Body").Child("BodyColor").Value.ToString());
                    int UTID = int.Parse(playerSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoID").Value.ToString());
                    Color UTColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoColor").Value.ToString());
                    int lowertorsoID = int.Parse(playerSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoID").Value.ToString());
                    Color LTColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoColor").Value.ToString());
                    int hairID = int.Parse(playerSnapshot.Child("Avatar").Child("Hair").Child("HairID").Value.ToString());
                    Color hairColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Hair").Child("HairColor").Value.ToString());
                    int eyesID = int.Parse(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyesID").Value.ToString());
                    Color pupilColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Pupils").Child("PupilsColor").Value.ToString());
                    int eyebrowsID = int.Parse(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsID").Value.ToString());
                    Color eyebrowsColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsColor").Value.ToString());
                    int noseID = int.Parse(playerSnapshot.Child("Avatar").Child("Nose").Child("NoseID").Value.ToString());
                    int mouthID = int.Parse(playerSnapshot.Child("Avatar").Child("Mouth").Child("MouthID").Value.ToString());

                    SecondPlayerAvatar.NewAvatarElement(username, bodyID, bodyColor, UTID, UTColor, lowertorsoID, LTColor, hairID, hairColor, eyesID, pupilColor, eyesID, eyebrowsID, eyebrowsColor, noseID, mouthID);
                }
                else if (playerSnapshot.Key.ToString() == ThirdPlacerID)
                {
                    string username = playerSnapshot.Child("username").Value.ToString();
                    int bodyID = int.Parse(playerSnapshot.Child("Avatar").Child("Body").Child("BodyID").Value.ToString());
                    Color bodyColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Body").Child("BodyColor").Value.ToString());
                    int UTID = int.Parse(playerSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoID").Value.ToString());
                    Color UTColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoColor").Value.ToString());
                    int lowertorsoID = int.Parse(playerSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoID").Value.ToString());
                    Color LTColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoColor").Value.ToString());
                    int hairID = int.Parse(playerSnapshot.Child("Avatar").Child("Hair").Child("HairID").Value.ToString());
                    Color hairColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Hair").Child("HairColor").Value.ToString());
                    int eyesID = int.Parse(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyesID").Value.ToString());
                    Color pupilColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Pupils").Child("PupilsColor").Value.ToString());
                    int eyebrowsID = int.Parse(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsID").Value.ToString());
                    Color eyebrowsColor = ConvertHexToColor(playerSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsColor").Value.ToString());
                    int noseID = int.Parse(playerSnapshot.Child("Avatar").Child("Nose").Child("NoseID").Value.ToString());
                    int mouthID = int.Parse(playerSnapshot.Child("Avatar").Child("Mouth").Child("MouthID").Value.ToString());

                    ThirdPlayerAvatar.NewAvatarElement(username, bodyID, bodyColor, UTID, UTColor, lowertorsoID, LTColor, hairID, hairColor, eyesID, pupilColor, eyesID, eyebrowsID, eyebrowsColor, noseID, mouthID);
                }
            }
        }


    }

    IEnumerator CreateRoom(int RoomIDToMake, string Phase)
    {
        ShowLoadingScreen();
        ActiveRoom = RoomIDToMake;
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("Phase").SetValueAsync(Phase);
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
        }

        var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("1").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => BTask.IsCompleted);

        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {
        }
        var CTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("2").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => CTask.IsCompleted);

        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {
        }
        var DTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("3").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => DTask.IsCompleted);

        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {
        }
        var ETask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("4").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ETask.IsCompleted);

        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {
        }
        var FTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("5").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => FTask.IsCompleted);

        if (FTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
        }
        else
        {
        }
        var GTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("Players").Child("0").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => GTask.IsCompleted);

        if (GTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
        }
        else
        {
        }

        var HTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("1").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => HTask.IsCompleted);

        if (HTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {HTask.Exception}");
        }
        else
        {
        }
        var ITask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("2").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ITask.IsCompleted);

        if (ITask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ITask.Exception}");
        }
        else
        {
        }
        var JTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("3").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => JTask.IsCompleted);

        if (JTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {JTask.Exception}");
        }
        else
        {
        }
        var KTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("4").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => KTask.IsCompleted);

        if (KTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {KTask.Exception}");
        }
        else
        {
        }
        var LTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("5").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => LTask.IsCompleted);

        if (LTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LTask.Exception}");
        }
        else
        {
        }

        var MTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("1").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => MTask.IsCompleted);

        if (MTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {MTask.Exception}");
        }
        else
        {
        }
        var NTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("2").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => NTask.IsCompleted);

        if (NTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {NTask.Exception}");
        }
        else
        {
        }
        var OTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("3").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => OTask.IsCompleted);

        if (OTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {OTask.Exception}");
        }
        else
        {
        }
        var PTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("4").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => PTask.IsCompleted);

        if (PTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {PTask.Exception}");
        }
        else
        {
        }
        var QTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("5").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => QTask.IsCompleted);

        if (QTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {QTask.Exception}");
        }
        else
        {
        }
        var RTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("1").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => RTask.IsCompleted);

        if (RTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {RTask.Exception}");
        }
        else
        {
        }
        var STask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("2").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => STask.IsCompleted);

        if (STask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {STask.Exception}");
        }
        else
        {
        }
        var TTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("3").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => TTask.IsCompleted);

        if (TTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {TTask.Exception}");
        }
        else
        {
        }
        var UTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("4").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => UTask.IsCompleted);

        if (UTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {UTask.Exception}");
        }
        else
        {
        }
        var VTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("5").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => VTask.IsCompleted);

        if (VTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {VTask.Exception}");
        }
        else
        {
        }
        var WTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("1").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => WTask.IsCompleted);

        if (WTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {WTask.Exception}");
        }
        else
        {
        }
        var XTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("2").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => XTask.IsCompleted);

        if (XTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {XTask.Exception}");
        }
        else
        {
        }
        var YTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("3").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => YTask.IsCompleted);

        if (YTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {YTask.Exception}");
        }
        else
        {
        }
        var ZTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("4").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ZTask.IsCompleted);

        if (ZTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ZTask.Exception}");
        }
        else
        {
        }
        var AATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("5").Child("ID").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AATask.IsCompleted);

        if (AATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AATask.Exception}");
        }
        else
        {
        }
        var ABTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("1").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ABTask.IsCompleted);

        if (ABTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ABTask.Exception}");
        }
        else
        {
        }
        var ACTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("2").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ACTask.IsCompleted);

        if (ACTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ACTask.Exception}");
        }
        else
        {
        }
        var ADTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("3").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ADTask.IsCompleted);

        if (ADTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ADTask.Exception}");
        }
        else
        {
        }
        var AETask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("4").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AETask.IsCompleted);

        if (AETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AETask.Exception}");
        }
        else
        {
        }
        var AFTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round1").Child("5").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AFTask.IsCompleted);

        if (AFTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AFTask.Exception}");
        }
        else
        {
        }

        var AHTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("1").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AHTask.IsCompleted);

        if (AHTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AHTask.Exception}");
        }
        else
        {
        }
        var AITask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("2").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AITask.IsCompleted);

        if (AITask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AITask.Exception}");
        }
        else
        {
        }
        var AJTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("3").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AJTask.IsCompleted);

        if (AJTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AJTask.Exception}");
        }
        else
        {
        }
        var AKTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("4").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AKTask.IsCompleted);

        if (AKTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AKTask.Exception}");
        }
        else
        {
        }
        var ALTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round2").Child("5").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ALTask.IsCompleted);

        if (ALTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ALTask.Exception}");
        }
        else
        {
        }

        var AMTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("1").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AMTask.IsCompleted);

        if (AMTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AMTask.Exception}");
        }
        else
        {
        }
        var ANTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("2").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ANTask.IsCompleted);

        if (ANTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ANTask.Exception}");
        }
        else
        {
        }
        var AOTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("3").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AOTask.IsCompleted);

        if (AOTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AOTask.Exception}");
        }
        else
        {
        }
        var APTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("4").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => APTask.IsCompleted);

        if (APTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {APTask.Exception}");
        }
        else
        {
        }
        var AQTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round3").Child("5").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AQTask.IsCompleted);

        if (AQTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AQTask.Exception}");
        }
        else
        {
        }
        var ARTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("1").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ARTask.IsCompleted);

        if (ARTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ARTask.Exception}");
        }
        else
        {
        }
        var ASTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("2").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ASTask.IsCompleted);

        if (ASTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ASTask.Exception}");
        }
        else
        {
        }
        var ATTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("3").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ATTask.IsCompleted);

        if (ATTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATTask.Exception}");
        }
        else
        {
        }
        var AUTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("4").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AUTask.IsCompleted);

        if (AUTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AUTask.Exception}");
        }
        else
        {
        }
        var AVTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round4").Child("5").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AVTask.IsCompleted);

        if (AVTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AVTask.Exception}");
        }
        else
        {
        }
        var AWTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("1").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AWTask.IsCompleted);

        if (AWTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AWTask.Exception}");
        }
        else
        {
        }
        var AXTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("2").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AXTask.IsCompleted);

        if (AXTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AXTask.Exception}");
        }
        else
        {
        }
        var AYTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("3").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AYTask.IsCompleted);

        if (AYTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AYTask.Exception}");
        }
        else
        {
        }
        var AZTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("4").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => AZTask.IsCompleted);

        if (AZTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {AZTask.Exception}");
        }
        else
        {
        }
        var BATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("SoundCards").Child("Round5").Child("5").Child("Owner").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => BATask.IsCompleted);

        if (BATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BATask.Exception}");
        }
        else
        {
        }

        var BBTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("Places").Child("First").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => BBTask.IsCompleted);
        if (BBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BBTask.Exception}");
        }
        else
        {
        }

        var BCTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("Places").Child("Second").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => BCTask.IsCompleted);
        if (BCTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BCTask.Exception}");
        }
        else
        {
        }

        var BDTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomIDToMake.ToString()).Child("Places").Child("Third").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => BDTask.IsCompleted);
        if (BDTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BDTask.Exception}");
        }
        else
        {
        }
        StartCoroutine(AddPlayerToRoom(RoomIDToMake));
        HideLoadingScreen();
    }

    IEnumerator AddPlayerToRoom(int RoomID)
    {
        ShowLoadingScreen();
        ActiveRoom = RoomID;
        Debug.Log("Adding player in room");
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("TotalPoints").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
        }

        var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").Child("1").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => BTask.IsCompleted);

        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {
        }
        var CTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").Child("2").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => CTask.IsCompleted);

        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {
        }
        var DTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").Child("3").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => DTask.IsCompleted);

        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {
        }
        var ETask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").Child("4").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ETask.IsCompleted);

        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {
        }
        var FTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").Child("5").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => FTask.IsCompleted);

        if (FTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
        }
        else
        {
        }
        var GTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("InGame").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => GTask.IsCompleted);

        if (GTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
        }
        else
        {
        }
        var HTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCardRecord").Child("1").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => HTask.IsCompleted);

        if (HTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {HTask.Exception}");
        }
        else
        {
        }
        var ITask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCardRecord").Child("2").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => ITask.IsCompleted);

        if (ITask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ITask.Exception}");
        }
        else
        {
        }
        var JTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCardRecord").Child("3").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => JTask.IsCompleted);

        if (JTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {JTask.Exception}");
        }
        else
        {
        }
        var KTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCardRecord").Child("4").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => KTask.IsCompleted);

        if (KTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {KTask.Exception}");
        }
        else
        {
        }
        var LTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCardRecord").Child("5").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => LTask.IsCompleted);

        if (LTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LTask.Exception}");
        }
        else
        {
        }

        InitializeWelcomeRoom(RoomID);
        HideLoadingScreen();
    }

    IEnumerator RemovePlayerToRoom(int RoomID)
    {
        ShowLoadingScreen();
        Debug.Log("Removing player in room");
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(RoomID.ToString()).Child("Players").Child(theLoginScript.User.UserId).RemoveValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
        }
        HideLoadingScreen();
        OnClickBack();
    }

    IEnumerator UpdatePlayerSoundCard(int SoundCardBeingSetup, int SoundCardID)
    {
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(ActiveRoom.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCards").Child(SoundCardBeingSetup.ToString()).SetValueAsync(SoundCardID);
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            UseSoundCardScreen.SetActive(false);
            SoundCardScreen.SetActive(false);
            WelcomeSoundCardBeinSetup = 0;
            ShowingSoundCardID = 0;
            StartCoroutine(WelcomeRoomRefresh(ActiveRoom));
        }
    }

    IEnumerator UpdatePlayerInGame()
    {
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(ActiveRoom.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("InGame").SetValueAsync(1);
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
        }

        InitializeWaitingRoom(ActiveRoom);
    }

    IEnumerator UpdatePlayerScore(int scoretogive, string playerIDToGive)
    {
        int ScoreToSet = 0;
        int bonusScore = Mathf.RoundToInt(TimerBonus);
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(ActiveRoom.ToString()).Child("Players").Child(playerIDToGive).Child("TotalPoints").GetValueAsync();
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot currentpoint = ATask.Result;
            ScoreToSet = int.Parse(currentpoint.Value.ToString()) + scoretogive + bonusScore;
        }

        var BTask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(ActiveRoom.ToString()).Child("Players").Child(playerIDToGive).Child("TotalPoints").SetValueAsync(ScoreToSet);
        yield return new WaitUntil(predicate: () => BTask.IsCompleted);

        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {

        }
    }

    IEnumerator UpdatePlayerSoundCardsRecord(int recordid) //0 = null 1=correct 2=wrong
    {
        var ATask = theLoginScript.DBreference.Child("EliminationGame").Child("Rooms").Child(ActiveRoom.ToString()).Child("Players").Child(theLoginScript.User.UserId).Child("SoundCardRecord").Child(ActiveCardNumber.ToString()).SetValueAsync(recordid);
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
        }
    }

    private Color ConvertHexToColor(string HexValue)
    {
        Color newCol = Color.white;
        ColorUtility.TryParseHtmlString(HexValue, out newCol);
        return newCol;
    }


}
