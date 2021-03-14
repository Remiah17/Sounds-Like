using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureModeManager : MonoBehaviour
{
    public string DarkRoomScene;
    public string MyAdventureScene;
    public string SelectModeScene;


    public GameObject SettingScreen;

    private SoundsScript theSoundScript;

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        theSoundScript.PlayBGM(3);
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickDarkRoom()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(DarkRoomScene);
    }

    public void OnClickMyAdventure()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(MyAdventureScene);
    }

    public void OnClickBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(SelectModeScene);
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
