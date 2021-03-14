using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnlineModeManager : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField]
    private string SelectModeSceneName;
    [SerializeField]
    private string SoundsOfTheSeasonSceneName;
    [SerializeField]
    private string EliminationGameSceneName;
    [Header("SCREENS")]
    [SerializeField]
    private GameObject NewsScreen;
    [SerializeField]
    private GameObject SettingScreen;
    [SerializeField]
    private Text CoinText;
    [SerializeField]
    private Text SMOTSNameText;
    [SerializeField]
    private Text SMOTSSeasonPointsText;
    private bool ShownSMOT = false;
    [SerializeField]
    private Text NewsText;
    [Header("SCRIPTS")]
    [SerializeField]
    private LoginScript TheLoginScript;
    private SoundsScript theSoundScript;

    [SerializeField]
    private Text SMOTSDaysLeftText;
    private bool DaysLeftChecked = false;


    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theSoundScript.PlayBGM(2);
        CoinText.text = "" + PlayerPrefs.GetInt("Coins");
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        CoinText.text = "" + PlayerPrefs.GetInt("Coins");
    }

    private void Update()
    {
        CoinText.text = "" + PlayerPrefs.GetInt("Coins");
        if (TheLoginScript.User == null)
        {
            GoBack();
        }

        if(!ShownSMOT && PlayerPrefs.GetInt("ReadNews") < 1 && TheLoginScript.SoundMasterOfSeasonScore > 0)
        {
            ShowNews();
        }

        if(!DaysLeftChecked && TheLoginScript.User != null)
        {
            StartCoroutine(GetDaysLeft());
        }
    }

    public void GoBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(SelectModeSceneName);
    }
    public void OnClick_SoundsOfTheSeason()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(SoundsOfTheSeasonSceneName);
    }
    public void OnClick_EliminationGame()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(EliminationGameSceneName);
    }
    public void CloseNews()
    {
        theSoundScript.PlaySelectSound();
        NewsScreen.SetActive(false);
        CoinText.text = "" + PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("ReadNews", 1);
    }
    public void ShowNews()
    {
        ShownSMOT = true;
        SMOTSNameText.text = TheLoginScript.SoundMasterOfSeasonName;
        SMOTSSeasonPointsText.text = "" + TheLoginScript.SoundMasterOfSeasonScore;
        NewsText.text = TheLoginScript.NewsString;
        NewsScreen.SetActive(true);
    }
    public void OnClickSetting()
    {
        theSoundScript.PlaySelectSound();
        if (SettingScreen.activeInHierarchy)
        {
            SettingScreen.SetActive(false);
            CoinText.text = "" + PlayerPrefs.GetInt("Coins");
        }
        else
        {
            SettingScreen.SetActive(true);
        }
    }

    IEnumerator GetDaysLeft()
    {
        var ATask = TheLoginScript.DBreference.Child("SoundsOfTheSeason").Child("DaysLeft").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            DataSnapshot codesSnapshot = ATask.Result;
            SMOTSDaysLeftText.text = codesSnapshot.Value.ToString();
        }
        DaysLeftChecked = true;
    }


}
