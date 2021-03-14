using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManagerScript : MonoBehaviour
{
    [SerializeField]
    private string SelectModeScene;
    [SerializeField]
    private GameObject SettingScreen;

    private SoundsScript theSoundScript;


    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theSoundScript.PlayBGM(1);
    }

    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPlay()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(SelectModeScene);
    }

    public void OnClickExit()
    {
        theSoundScript.PlaySelectSound();
        Application.Quit();
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
}

