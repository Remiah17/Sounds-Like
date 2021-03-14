using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySoundsManager : MonoBehaviour
{
    public string OnlineSoundsSceneName;

    public LoginScript TheLoginScript;


    public GameObject SettingScreen;

    private SoundsScript theSoundScript;

    public Button EasyButton;
    public Button NormalButton;
    public Button HardButton;

    public GameObject EasySoundsViewScreen;
    public GameObject NormalSoundsViewScreen;
    public GameObject HardSoundsViewScreen;

    public GameObject AddEasySoundsScreen;
    public GameObject AddNormalSoundsScreen;
    public GameObject AddHardSoundsScreen;

    public InputField AddEasySoundItemName;

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        ShowEasySounds();
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    public void GoBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(OnlineSoundsSceneName);
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

    public void ShowEasySounds()
    {
        EasySoundsViewScreen.SetActive(true);
        NormalSoundsViewScreen.SetActive(false);
        HardSoundsViewScreen.SetActive(false);

        EasyButton.gameObject.SetActive(false);
        NormalButton.gameObject.SetActive(true);
        HardButton.gameObject.SetActive(true);
    }

    public void ShowNormalSounds()
    {
        EasySoundsViewScreen.SetActive(false);
        NormalSoundsViewScreen.SetActive(true);
        HardSoundsViewScreen.SetActive(false);

        EasyButton.gameObject.SetActive(true);
        NormalButton.gameObject.SetActive(false);
        HardButton.gameObject.SetActive(true);
    }

    public void ShowHardSounds()
    {
        EasySoundsViewScreen.SetActive(false);
        NormalSoundsViewScreen.SetActive(false);
        HardSoundsViewScreen.SetActive(true);

        EasyButton.gameObject.SetActive(true);
        NormalButton.gameObject.SetActive(true);
        HardButton.gameObject.SetActive(false);
    }

    public void AddSong()
    {
        Application.OpenURL("https://www.solunagames.com/online-sounds");
    }
}
