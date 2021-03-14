using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectModeScript : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField]
    private string TitleScene;
    [SerializeField]
    private string OfflineScene;
    [SerializeField]
    private string OnlineScene;
    //[SerializeField]
    //private Button OnlineButton;
    [SerializeField]
    private LoginScript TheLoginScript;
    [Header("SCREENS")]
    [SerializeField]
    private GameObject SettingScreen;
    private SoundsScript theSoundScript;
    [SerializeField]
    private Text CoinText;

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theSoundScript.PlayBGM(1);
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
    }

    public void OnClickOffline()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(OfflineScene);
    }

    public void OnClickOnline()
    {
        theSoundScript.PlaySelectSound();
        if (TheLoginScript.User != null)
        {
            SceneManager.LoadScene(OnlineScene);
        }
        else
        {
            TheLoginScript.ShowAccount();
        }
    }

    public void OnClickHome()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(TitleScene);
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
}
