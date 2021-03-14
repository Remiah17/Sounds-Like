using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField]
    private GameObject LogInScreen;
    [SerializeField]
    private GameObject LoggedInScreen;
    [SerializeField]
    private GameObject RegisterScreen;

    //LOGIN SCREEN
    [Header("Login")]
    [SerializeField]
    private InputField LoginUsernameInput;
    [SerializeField]
    private InputField LoginPasswordInput;
    [SerializeField]
    private Button LoginButton;
    [SerializeField]
    private Button CreateAccountButton;
    [SerializeField]
    private Text LoginWarningText;

    //REGISTER SCREEN
    [Header("Register")]
    [SerializeField]
    private InputField RegisterUsernameInput;
    [SerializeField]
    private InputField RegisterEmailInput;
    [SerializeField]
    private InputField RegisterPasswordInput;
    [SerializeField]
    private InputField RegisterConfirmPasswordInput;
    [SerializeField]
    private Button ConfirmButton;
    [SerializeField]
    private Text RegisterWarningText;

    //LOGGED IN SCREEN
    [Header("Logged In")]
    [SerializeField]
    private Text UsernameText;
    [SerializeField]
    private Text ScoreText;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;
    public FirebaseStorage storage;

    //DATABASE
    [Header("Database")]
    private int OnlineScore = -1;
    [SerializeField]
    private GameObject ScoreboardView;
    [SerializeField]
    private GameObject scoreElement;
    [SerializeField]
    private Transform scoreboardContent;
    private int Place;

    [Header("CHANGE USERNAME")]
    [SerializeField]
    private GameObject ChangeUserNameScreen;
    [SerializeField]
    private InputField NewUserNameInputField;
    [SerializeField]
    private GameObject WarningText;

    [Header("LOADING SCREEN")]
    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private Text LoadingMessageText;
    private bool SeasonPointsChecked = false;
    private bool RegisterTimeChecked = false;
    private bool LoginTimeChecked = false;

    [Header("FORGOT PASSWORD")]
    [SerializeField]
    private GameObject ForgotPasswordScreen;
    [SerializeField]
    private InputField ForgotPasswordEmailInputField;
    [SerializeField]
    private GameObject ConnectionErrorScreen;

    public string SoundMasterOfSeasonName;
    public int SoundMasterOfSeasonScore;
    public string SoundMasterOfSeasonID;
    public bool SoundMasterChecked =  false;
    public string NewsString;

    private SoundsScript theSoundScript;

    [Header("AVATAR")]
    [SerializeField]
    private AvatarDatabaseScriptableObject AvatarsDatabase;
    [SerializeField]
    private GameObject EditAvatarScreen;
    [SerializeField]
    private AvatarManager LoginAvatarManager;

    [SerializeField]
    private GameObject GiftScreen;

    [SerializeField]
    private GameObject UpdateTheGameScreen;

    FirebaseOnlineChecker theOnlineChecker;


    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theOnlineChecker = gameObject.GetComponent<FirebaseOnlineChecker>();

        LogInScreen.SetActive(false);
        LoggedInScreen.SetActive(false);
        RegisterScreen.SetActive(false);

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
                InitiateConnectionError();
            }
        });
    }

    private void Update()
    {
        if(LoggedInScreen.activeInHierarchy || LoggedInScreen.activeInHierarchy || RegisterScreen.activeInHierarchy)
        {
            if (SeasonPointsChecked && OnlineScore < 0)
            {
                InitiateLoginConnectionError();
            }

            if(RegisterTimeChecked && User == null)
            {
                InitiateLoginConnectionError();
            }

            if(LoginTimeChecked && User == null)
            {
                InitiateLoginConnectionError();
            }
        }

        if(auth != null && DBreference != null)
        {
            if (!SoundMasterChecked && auth.CurrentUser != null)
            {
                StartCoroutine(checkTheSoundMasterOfTheSeason());
                StartCoroutine(CheckifAppLatestVersion());
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
    }

    public void ShowAccount()
    {
        theSoundScript.PlaySelectSound();
        if (auth.CurrentUser != null)
        {
            SeasonPointsChecked = false;
            LoggedInScreen.SetActive(true);
            LogInScreen.SetActive(false);
            RegisterScreen.SetActive(false);
            UsernameText.text = auth.CurrentUser.DisplayName;
            StartCoroutine(LoadUserData());
            SaveDataTheData();
            ScoreboardView.SetActive(false);
        }
        else
        {
            LoginTimeChecked = false;
            LogInScreen.SetActive(true);
            LoggedInScreen.SetActive(false);
            LoginWarningText.gameObject.SetActive(false);
            LoginUsernameInput.text = null;
            LoginPasswordInput.text = null;
            RegisterScreen.SetActive(false);
        }
    }

    public void GoBack()
    {
        theSoundScript.PlaySelectSound();
        if (LoggedInScreen.activeInHierarchy)
        {
            LoggedInScreen.SetActive(false);
        }
        if (LogInScreen.activeInHierarchy)
        {
            LogInScreen.SetActive(false);
            LoginWarningText.gameObject.SetActive(false);
            LoginUsernameInput.text = null;
            LoginPasswordInput.text = null;
        }
    }
    public void LoginAccount()
    {
        theSoundScript.PlaySelectSound();
        StartCoroutine(Login(LoginUsernameInput.text, LoginPasswordInput.text));
    }

    public void ShowRegisterScreen()
    {
        RegisterTimeChecked = false;
        theSoundScript.PlaySelectSound();
        LoginWarningText.gameObject.SetActive(false);
        LoginUsernameInput.text = null;
        LoginPasswordInput.text = null;
        LogInScreen.SetActive(false);
        RegisterScreen.SetActive(true);
        RegisterWarningText.gameObject.SetActive(false);
        RegisterUsernameInput.text = null;
        RegisterEmailInput.text = null;
        RegisterPasswordInput.text = null;
        RegisterConfirmPasswordInput.text = null;
    }

    public void CreateAccount()
    {
        theSoundScript.PlaySelectSound();
        StartCoroutine(Register(RegisterEmailInput.text, RegisterPasswordInput.text, RegisterUsernameInput.text));
    }

    public void CancelRegistration()
    {
        theSoundScript.PlaySelectSound();
        RegisterWarningText.gameObject.SetActive(false);
        RegisterUsernameInput.text = null;
        RegisterEmailInput.text = null;
        RegisterPasswordInput.text = null;
        RegisterConfirmPasswordInput.text = null;
        RegisterScreen.SetActive(false);
        LogInScreen.SetActive(true);
        LoginWarningText.gameObject.SetActive(false);
        LoginUsernameInput.text = null;
        LoginPasswordInput.text = null;
    }

    public void SignOut()
    {
        theSoundScript.PlaySelectSound();
        theOnlineChecker.SendStatusOffline();
        auth.SignOut();
        UsernameText.text = null;
        ScoreText.text = null;
        LoggedInScreen.SetActive(false);
        LogInScreen.SetActive(true);
        LoginWarningText.gameObject.SetActive(false);
        LoginUsernameInput.text = null;
        LoginPasswordInput.text = null;
    }

    public void OnClick_ForgotPassword()
    {
        theSoundScript.PlaySelectSound();
        if (ForgotPasswordScreen.activeInHierarchy)
        {
            ForgotPasswordScreen.SetActive(false);
        }
        else
        {
            ForgotPasswordScreen.SetActive(true);
            ForgotPasswordEmailInputField.text = null;
        }
    }

    public void SendPasswordResetEmail()
    {
        if (ForgotPasswordEmailInputField.text == null)
        {
            StartCoroutine(ShowLoginWarningText("EMAIL FIELD IS EMPTY"));
        }
        else
        {
            string emailAddress = ForgotPasswordEmailInputField.text;
            if (User != null)
            {
                auth.SendPasswordResetEmailAsync(emailAddress).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                        //StartCoroutine(ShowLoginWarningText("Password reset email  was canceled."));
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                        //StartCoroutine(ShowLoginWarningText("Password reset email encountered an error."));
                        return;
                    }
                    Debug.Log("Password reset email sent successfully.");
                    StartCoroutine(ShowLoginWarningText("Password reset email sent successfully."));
                    OnClick_ForgotPassword();
                });
            }
           
        }
       
    }

    private void SaveDataTheData()
    {
        StartCoroutine(UpdateUsernameAuth(auth.CurrentUser.DisplayName));
        StartCoroutine(UpdateUsernameDatabase(auth.CurrentUser.DisplayName));
        StartCoroutine(UpdateOnlineScore(PlayerPrefs.GetInt("Score")));
    }

    public void ScoreboardButton()
    {
        theSoundScript.PlaySelectSound();
        if (!ScoreboardView.activeInHierarchy)
        {
            ScoreboardView.SetActive(true);
            StartCoroutine(LoadScoreboardData());
        }
        else
        {
            ScoreboardView.SetActive(false);
        }
    }

    public void ShowChangeUserNameScreen()
    {
        theSoundScript.PlaySelectSound();
        if (ChangeUserNameScreen.activeInHierarchy)
        {
            ChangeUserNameScreen.SetActive(false);
        }
        else
        {
            ChangeUserNameScreen.SetActive(true);
            NewUserNameInputField.text = null;
            WarningText.gameObject.SetActive(false);
        }
    }

    public void OnClick_ChangeUserName()
    {
        theSoundScript.PlaySelectSound();
        if(string.IsNullOrEmpty(NewUserNameInputField.text))
        {
            WarningText.gameObject.SetActive(true);
        }
        else
        {
            WarningText.gameObject.SetActive(false);
            StartCoroutine(UpdateUsernameAuth(NewUserNameInputField.text));
            UsernameText.text = NewUserNameInputField.text;
        }
        ShowChangeUserNameScreen();
    }

    public void InitiateLoginConnectionError()
    {
        StartCoroutine(LoginConnectionFailedSequence());
    }

    public void InitiateConnectionError()
    {
        HideLoadingScreen();
        ConnectionErrorScreen.SetActive(true);
        Debug.LogError("CONNECTION ERROR RUN");
    }

    public void OnClick_ConnectionErrorOkay()
    {
        theSoundScript.PlaySelectSound();
        ConnectionErrorScreen.SetActive(false);
        SceneManager.LoadScene("SelectModeScene");
    }

    public void ShowLoadingScreen(string message)
    {
        LoadingScreen.SetActive(true);
        LoadingMessageText.text = message;
    }
    public void HideLoadingScreen()
    {
        LoadingScreen.SetActive(false);
        LoadingMessageText.text = "";
    }

    public void HideShowGiftScreen()
    {
        theSoundScript.PlaySelectSound();
        if (GiftScreen.activeInHierarchy)
        {
            GiftScreen.SetActive(false);
        }
        else
        {
            GiftScreen.SetActive(true);
        }
    }

    public void ShowHideAvatarScreen()
    {
        theSoundScript.PlaySelectSound();
        if (EditAvatarScreen.activeInHierarchy)
        {
            EditAvatarScreen.SetActive(false);
        }
        else
        {
            EditAvatarScreen.SetActive(true);
        }
        LoginAvatarManager.SetupPlayersAvatar();
        StartCoroutine(UpdateAvatarData());
    }

    IEnumerator Login(string _email, string _password)
    {
        CheckLoginConnectionTime();
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        if (LoginTask.Exception != null)
        {
            //if there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            StartCoroutine(ShowLoginWarningText(message));
        }
        else
        {
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            LoggedInScreen.SetActive(true);
            ScoreboardView.SetActive(false);
            LogInScreen.SetActive(false);
            PlayerPrefs.SetInt("ReadNews", 0);
            StartCoroutine(LoadUserData());
            LoginTimeChecked = true;
            HideLoadingScreen();
        }
    }
   
    IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            RegisterWarningText.text = "Missing Username";
        }
        else if (RegisterPasswordInput.text != RegisterConfirmPasswordInput.text)
        {
            RegisterWarningText.text = "Password Does Not Match!";
        }
        else
        {
            StartCoroutine(CheckRegisterConnectionTIme());
            //Call the firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //wait until the task complete
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //if there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                string message = "Login Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        break;
                }
                StartCoroutine(ShowRegisterWarningText(message));
            }
            else
            {
                User = RegisterTask.Result;
                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //wait until task complete
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //if there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        StartCoroutine(ShowRegisterWarningText("Username Set Failed!"));
                    }
                    else
                    {
                        StartCoroutine(UpdateSeasonPoints(0));
                        StartCoroutine(UpdateOnlineScore(PlayerPrefs.GetInt("Score")));
                        StartCoroutine(UpdateSoundItemTryData(0));
                        StartCoroutine(UpdateUsernameDatabase(User.DisplayName));
                        StartCoroutine(UpdateGifts());
                        StartCoroutine(LoadUserData());
                        LoggedInScreen.SetActive(true);
                        ScoreboardView.SetActive(false);
                        LogInScreen.SetActive(false);
                        RegisterScreen.SetActive(false);
                        RegisterTimeChecked = true;
                        HideLoadingScreen();
                        ShowHideAvatarScreen();
                    }
                }
            }
        }
    }

    IEnumerator ShowLoginWarningText(string message)
    {
        LoginWarningText.gameObject.SetActive(true);
        LoginWarningText.text = message;
        yield return new WaitForSeconds(3);
        LoginWarningText.gameObject.SetActive(false);
    }

    IEnumerator ShowRegisterWarningText(string message)
    {
        RegisterWarningText.gameObject.SetActive(true);
        RegisterWarningText.text = message;
        yield return new WaitForSeconds(3);
        RegisterWarningText.gameObject.SetActive(false);
    }

    IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //call the firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
        }
    }

    IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data base username is now updated
        }
    }

    IEnumerator UpdateOnlineScore(int _score)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("onlinescore").SetValueAsync(_score);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
        }
    }

    IEnumerator UpdateSoundItemTryData(int value)
    {
                var LTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundOneAnswered").SetValueAsync(value);

                yield return new WaitUntil(predicate: () => LTask.IsCompleted);

                if (LTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {LTask.Exception}");
                }
                else
                {

                }
                var MTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundTwoAnswered").SetValueAsync(value);

                yield return new WaitUntil(predicate: () => MTask.IsCompleted);

                if (MTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {MTask.Exception}");
                }
                else
                {

                }
                var NTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundThreeAnswered").SetValueAsync(value);

                yield return new WaitUntil(predicate: () => NTask.IsCompleted);

                if (NTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {NTask.Exception}");
                }
                else
                {

                }
                var OTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundFourAnswered").SetValueAsync(value);

                yield return new WaitUntil(predicate: () => OTask.IsCompleted);

                if (OTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {OTask.Exception}");
                }
                else
                {

                }
                var PTask = DBreference.Child("users").Child(User.UserId).Child("SeasonSoundFiveAnswered").SetValueAsync(value);

                yield return new WaitUntil(predicate: () => PTask.IsCompleted);

                if (PTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {PTask.Exception}");
                }
                else
                {

                }
    }

    IEnumerator LoadUserData()
    {
        StartCoroutine(CheckConnectionTime());
        ShowLoadingScreen("Checking user name.");
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            ShowLoadingScreen("Checking user name failed.");
        }
        else
        {
            DataSnapshot Usernamesnapshot = DBTask.Result;
            if (LoggedInScreen.activeInHierarchy)
            {
                UsernameText.text = Usernamesnapshot.Value.ToString();
                ShowLoadingScreen("User name found.");
            }
        }
        ShowLoadingScreen("Checking season points.");
        var DCTask = DBreference.Child("users").Child(User.UserId).Child("SeasonPoints").GetValueAsync();

        yield return new WaitUntil(predicate: () => DCTask.IsCompleted);

        if (DCTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DCTask.Exception}");
            ShowLoadingScreen("Checking season points failed.");
        }
        else
        {
            DataSnapshot snapshot = DCTask.Result;
            OnlineScore = int.Parse(snapshot.Value.ToString());
            ScoreText.text = OnlineScore.ToString();
            ShowLoadingScreen("Season points updated.");
        }
        var DDTask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("UserID").GetValueAsync();

        yield return new WaitUntil(predicate: () => DDTask.IsCompleted);

        if (DDTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DDTask.Exception}");
            ShowLoadingScreen("Checking season points failed.");
        }
        else
        {
            DataSnapshot SoundMasterUserID = DDTask.Result;
            SoundMasterOfSeasonID = SoundMasterUserID.Value.ToString();

            var DETask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("Season Points").GetValueAsync();

            yield return new WaitUntil(predicate: () => DETask.IsCompleted);

            if (DETask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DETask.Exception}");
                ShowLoadingScreen("Checking season points failed.");
            }
            else
            {
                DataSnapshot SoundMasterScore = DETask.Result;
                SoundMasterOfSeasonScore = int.Parse(SoundMasterScore.Value.ToString());

                var DFTask = DBreference.Child("users").Child(SoundMasterOfSeasonID).Child("username").GetValueAsync();

                yield return new WaitUntil(predicate: () => DFTask.IsCompleted);

                if (DFTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DFTask.Exception}");
                    ShowLoadingScreen("Checking season points failed.");
                }
                else
                {
                    DataSnapshot SoundMasterName = DFTask.Result;
                    SoundMasterOfSeasonName = SoundMasterName.Value.ToString();
                    ShowLoadingScreen("Sound Master of the Season updated.");
                }
            }
            
        }
        SeasonPointsChecked = true;
        HideLoadingScreen();
        StartCoroutine(UpdateOnlineScore(PlayerPrefs.GetInt("Score")));
    }

    IEnumerator LoadScoreboardData()
    {
        ShowLoadingScreen("Checking for leaderboard datas.");
        Place = 0;
        var DBTask = DBreference.Child("users").OrderByChild("SeasonPoints").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            ShowLoadingScreen("Checking leaderboard datas failed.");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int _onlinescore = int.Parse(childSnapshot.Child("SeasonPoints").Value.ToString());
                int bodyID = int.Parse(childSnapshot.Child("Avatar").Child("Body").Child("BodyID").Value.ToString());
                Color bodyColor = ConvertHexToColor(childSnapshot.Child("Avatar").Child("Body").Child("BodyColor").Value.ToString());
                int UTID = int.Parse(childSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoID").Value.ToString());
                Color UTColor = ConvertHexToColor(childSnapshot.Child("Avatar").Child("UpperTorso").Child("UpperTorsoColor").Value.ToString());
                int lowertorsoID = int.Parse(childSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoID").Value.ToString());
                Color LTColor = ConvertHexToColor(childSnapshot.Child("Avatar").Child("LowerTorso").Child("LowerTorsoColor").Value.ToString());
                int hairID = int.Parse(childSnapshot.Child("Avatar").Child("Hair").Child("HairID").Value.ToString());
                Color hairColor = ConvertHexToColor(childSnapshot.Child("Avatar").Child("Hair").Child("HairColor").Value.ToString());
                int eyesID = int.Parse(childSnapshot.Child("Avatar").Child("Eyes").Child("EyesID").Value.ToString());
                Color pupilColor = ConvertHexToColor(childSnapshot.Child("Avatar").Child("Pupils").Child("PupilsColor").Value.ToString());
                int eyebrowsID = int.Parse(childSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsID").Value.ToString());
                Color eyebrowsColor = ConvertHexToColor(childSnapshot.Child("Avatar").Child("Eyes").Child("EyebrowsColor").Value.ToString());
                int noseID = int.Parse(childSnapshot.Child("Avatar").Child("Nose").Child("NoseID").Value.ToString());
                int mouthID = int.Parse(childSnapshot.Child("Avatar").Child("Mouth").Child("MouthID").Value.ToString());


                //Instantiate new scoreboard element
                if (Place < 6)
                {
                    Place += 1;
                    GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                    scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, _onlinescore, Place);
                    scoreboardElement.GetComponent<ScoreElement>().NewAvatarElement(bodyID,bodyColor,UTID, UTColor,lowertorsoID, LTColor, hairID, hairColor, eyesID, pupilColor, eyesID, eyebrowsID, eyebrowsColor, noseID, mouthID);
                    ShowLoadingScreen("Number " + Place + " in leaderboard found.");
                }

            }
            HideLoadingScreen();
        }
    }

    private Color ConvertHexToColor(string HexValue)
    {
        Color newCol = Color.white;
        ColorUtility.TryParseHtmlString(HexValue, out newCol);
        return newCol;
    }

    private string ConvertColorToHex(Color colorValue)
    {
        string HexVal = "#" + ColorUtility.ToHtmlStringRGB(colorValue);
        return HexVal;
    }

    IEnumerator UpdateSeasonPoints(int PointsGetting)
    {
        var ATask = DBreference.Child("users").Child(User.UserId).Child("SeasonPoints").SetValueAsync(PointsGetting);

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {

        }
    }

    IEnumerator CheckConnectionTime()
    {
        ShowLoadingScreen("Retrieving datas from the server.");
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
                                            if (!SeasonPointsChecked)
                                            {
                                                yield return new WaitForSeconds(1);
                                                SeasonPointsChecked = true;
                                            }
                                            else
                                            {
                                                HideLoadingScreen();
                                            }
                                        }
                                        else
                                        {
                                            HideLoadingScreen();
                                        }
                                    }
                                    else
                                    {
                                        HideLoadingScreen();
                                    }
                                }
                                else
                                {
                                    HideLoadingScreen();
                                }
                            }
                            else
                            {
                                HideLoadingScreen();
                            }
                        }
                        else
                        {
                            HideLoadingScreen();
                        }
                    }
                    else
                    {
                        HideLoadingScreen();
                    }
                }
                else
                {
                    HideLoadingScreen();
                }
            }
            else
            {
               HideLoadingScreen();
            }
        }
        else
        {
            SeasonPointsChecked = true;
            HideLoadingScreen();
        }
    }

    IEnumerator CheckRegisterConnectionTIme()
    {
        ShowLoadingScreen("Retrieving datas from the server.");
        if (!RegisterTimeChecked)
        {
            yield return new WaitForSeconds(1);
            if (!RegisterTimeChecked)
            {
                yield return new WaitForSeconds(1);
                if (!RegisterTimeChecked)
                {
                    yield return new WaitForSeconds(1);
                    if (!RegisterTimeChecked)
                    {
                        yield return new WaitForSeconds(1);
                        if (!RegisterTimeChecked)
                        {
                            yield return new WaitForSeconds(1);
                            if (!RegisterTimeChecked)
                            {
                                yield return new WaitForSeconds(1);
                                if (!RegisterTimeChecked)
                                {
                                    yield return new WaitForSeconds(1);
                                    if (!RegisterTimeChecked)
                                    {
                                        yield return new WaitForSeconds(1);
                                        if (!RegisterTimeChecked)
                                        {
                                            yield return new WaitForSeconds(1);
                                            if (!RegisterTimeChecked)
                                            {
                                                yield return new WaitForSeconds(1);
                                                RegisterTimeChecked = true;
                                            }
                                            else
                                            {
                                                HideLoadingScreen();
                                            }
                                        }
                                        else
                                        {
                                            HideLoadingScreen();
                                        }
                                    }
                                    else
                                    {
                                        HideLoadingScreen();
                                    }
                                }
                                else
                                {
                                    HideLoadingScreen();
                                }
                            }
                            else
                            {
                                HideLoadingScreen();
                            }
                        }
                        else
                        {
                            HideLoadingScreen();
                        }
                    }
                    else
                    {
                        HideLoadingScreen();
                    }
                }
                else
                {
                    HideLoadingScreen();
                }
            }
            else
            {
                HideLoadingScreen();
            }
        }
        else
        {
            RegisterTimeChecked = true;
            HideLoadingScreen();
        }
    }

    IEnumerator CheckLoginConnectionTime()
    {
        ShowLoadingScreen("Retrieving datas from the server.");
        if (!LoginTimeChecked)
        {
            yield return new WaitForSeconds(1);
            if (!LoginTimeChecked)
            {
                yield return new WaitForSeconds(1);
                if (!LoginTimeChecked)
                {
                    yield return new WaitForSeconds(1);
                    if (!LoginTimeChecked)
                    {
                        yield return new WaitForSeconds(1);
                        if (!LoginTimeChecked)
                        {
                            yield return new WaitForSeconds(1);
                            if (!LoginTimeChecked)
                            {
                                yield return new WaitForSeconds(1);
                                if (!LoginTimeChecked)
                                {
                                    yield return new WaitForSeconds(1);
                                    if (!LoginTimeChecked)
                                    {
                                        yield return new WaitForSeconds(1);
                                        if (!LoginTimeChecked)
                                        {
                                            yield return new WaitForSeconds(1);
                                            if (!LoginTimeChecked)
                                            {
                                                yield return new WaitForSeconds(1);
                                                LoginTimeChecked = true;
                                            }
                                            else
                                            {
                                                HideLoadingScreen();
                                            }
                                        }
                                        else
                                        {
                                            HideLoadingScreen();
                                        }
                                    }
                                    else
                                    {
                                        HideLoadingScreen();
                                    }
                                }
                                else
                                {
                                    HideLoadingScreen();
                                }
                            }
                            else
                            {
                                HideLoadingScreen();
                            }
                        }
                        else
                        {
                            HideLoadingScreen();
                        }
                    }
                    else
                    {
                        HideLoadingScreen();
                    }
                }
                else
                {
                    HideLoadingScreen();
                }
            }
            else
            {
                HideLoadingScreen();
            }
        }
        else
        {
            LoginTimeChecked = true;
            HideLoadingScreen();
        }
    }

    IEnumerator LoginConnectionFailedSequence()
    {
        ShowLoadingScreen("Connecting server failed. Please check your network and try again.");
        yield return new WaitForSeconds(3);
        HideLoadingScreen();
        LogInScreen.SetActive(false);
        LoggedInScreen.SetActive(false);
        RegisterScreen.SetActive(false);
    }

    IEnumerator checkTheSoundMasterOfTheSeason()
    {
        SoundMasterChecked = true;
        var DDTask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("UserID").GetValueAsync();

        yield return new WaitUntil(predicate: () => DDTask.IsCompleted);

        if (DDTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DDTask.Exception}");
        }
        else
        {
            DataSnapshot SoundMasterUserID = DDTask.Result;
            SoundMasterOfSeasonID = SoundMasterUserID.Value.ToString();

            StartCoroutine(GetNewsOnline());

            var DFTask = DBreference.Child("users").Child(SoundMasterOfSeasonID).Child("username").GetValueAsync();

            yield return new WaitUntil(predicate: () => DFTask.IsCompleted);

            if (DFTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DFTask.Exception}");
            }
            else
            {
                DataSnapshot SoundMasterName = DFTask.Result;
                SoundMasterOfSeasonName = SoundMasterName.Value.ToString();
                var DETask = DBreference.Child("Online").Child("SoundMasterOfSeason").Child("Season Points").GetValueAsync();

                yield return new WaitUntil(predicate: () => DETask.IsCompleted);

                if (DETask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {DETask.Exception}");
                }
                else
                {
                    DataSnapshot SoundMasterScore = DETask.Result;
                    SoundMasterOfSeasonScore = int.Parse(SoundMasterScore.Value.ToString());
                }
            }
            

        }
    }

    IEnumerator UpdateAvatarData()
    {
        var ATask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Body").Child("BodyColor").SetValueAsync(ConvertColorToHex(AvatarsDatabase.BodyColorUsed));

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {

        }

        var BTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("UpperTorso").Child("UpperTorsoColor").SetValueAsync(ConvertColorToHex(AvatarsDatabase.UpperTorsoColorUsed));

        yield return new WaitUntil(predicate: () => BTask.IsCompleted);

        if (BTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {BTask.Exception}");
        }
        else
        {

        }

        var CTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("LowerTorso").Child("LowerTorsoColor").SetValueAsync(ConvertColorToHex(AvatarsDatabase.LowerTorsoColorUsed));

        yield return new WaitUntil(predicate: () => CTask.IsCompleted);

        if (CTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {CTask.Exception}");
        }
        else
        {

        }

        var DTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Hair").Child("HairID").SetValueAsync(AvatarsDatabase.HairIDInUse.ToString());

        yield return new WaitUntil(predicate: () => DTask.IsCompleted);

        if (DTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DTask.Exception}");
        }
        else
        {

        }

        var ETask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Hair").Child("HairColor").SetValueAsync(ConvertColorToHex(AvatarsDatabase.HairColor));

        yield return new WaitUntil(predicate: () => ETask.IsCompleted);

        if (ETask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ETask.Exception}");
        }
        else
        {

        }

        var FTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Eyes").Child("EyesID").SetValueAsync(AvatarsDatabase.EyesIDInUse.ToString());

        yield return new WaitUntil(predicate: () => FTask.IsCompleted);

        if (FTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {FTask.Exception}");
        }
        else
        {

        }

        var GTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Pupils").Child("PupilsColor").SetValueAsync(ConvertColorToHex(AvatarsDatabase.PupilColor));

        yield return new WaitUntil(predicate: () => GTask.IsCompleted);

        if (GTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {GTask.Exception}");
        }
        else
        {

        }

        var HTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Eyes").Child("EyebrowsID").SetValueAsync(AvatarsDatabase.EyebrowsInUse.ToString());

        yield return new WaitUntil(predicate: () => HTask.IsCompleted);

        if (HTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {HTask.Exception}");
        }
        else
        {

        }

        var ITask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Eyes").Child("EyebrowsColor").SetValueAsync(ConvertColorToHex(AvatarsDatabase.EyebrowColor));

        yield return new WaitUntil(predicate: () => ITask.IsCompleted);

        if (ITask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ITask.Exception}");
        }
        else
        {

        }

        var JTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Nose").Child("NoseID").SetValueAsync(AvatarsDatabase.NoseIDInUse.ToString());

        yield return new WaitUntil(predicate: () => JTask.IsCompleted);

        if (JTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {JTask.Exception}");
        }
        else
        {

        }

        var KTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Mouth").Child("MouthID").SetValueAsync(AvatarsDatabase.MouthIDInUse.ToString());

        yield return new WaitUntil(predicate: () => KTask.IsCompleted);

        if (KTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {KTask.Exception}");
        }
        else
        {

        }

        var LTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("Body").Child("BodyID").SetValueAsync(AvatarsDatabase.BodyIDInUse.ToString());

        yield return new WaitUntil(predicate: () => LTask.IsCompleted);

        if (LTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LTask.Exception}");
        }
        else
        {

        }

        var MTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("UpperTorso").Child("UpperTorsoID").SetValueAsync(AvatarsDatabase.UpperTorsoIDInUse.ToString());

        yield return new WaitUntil(predicate: () => MTask.IsCompleted);

        if (MTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {MTask.Exception}");
        }
        else
        {

        }

        var NTask = DBreference.Child("users").Child(User.UserId).Child("Avatar").Child("LowerTorso").Child("LowerTorsoID").SetValueAsync(AvatarsDatabase.LowerTorsoIDInUse.ToString());

        yield return new WaitUntil(predicate: () => NTask.IsCompleted);

        if (NTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {NTask.Exception}");
        }
        else
        {

        }

    }

    IEnumerator GetNewsOnline()
    {
        var ATask = DBreference.Child("Online").Child("News").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot codesSnapshot = ATask.Result;
            NewsString = codesSnapshot.Value.ToString();
        }
    }

    IEnumerator CheckifAppLatestVersion()
    {
        var ATask = DBreference.Child("Main").Child("CurrentVersion").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot codesSnapshot = ATask.Result;
            if(Application.version != codesSnapshot.Value.ToString())
            {
                UpdateTheGameScreen.SetActive(true);
                Debug.Log("Update your app!");
            }
            else
            {
                Debug.Log("Your version is up to date.");
            }
        }
    }

    public void UpdateMyApp()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.SolunaGames.SoundsLikePro");
            Debug.Log("Opening google play store.");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.OpenURL("https://apps.apple.com/us/app/sounds-like/id1552651477");
            Debug.Log("Opening App Store.");
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.SolunaGames.SoundsLikePro");
            Debug.Log("Opening Windows store.");
        }
    }

    IEnumerator UpdateGifts()
    {
        var ATask = DBreference.Child("users").Child(User.UserId).Child("Reward Mail").Child("0").SetValueAsync(0);

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

