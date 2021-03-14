using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OfflineModeManager : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField]
    private string SelectModeSceneName;
    [SerializeField]
    private string OfflineCampaignSceneName;
    [SerializeField]
    private string OfflineArcadeSceneName;
    [Header("SCREENS")]
    [SerializeField]
    private GameObject SettingScreen;
    private SoundsScript theSoundScript;
    [SerializeField]
    private Text CoinText;

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

    public void GoBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(SelectModeSceneName);
    }
    public void OnClick_Arcade()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(OfflineArcadeSceneName);
    }
    public void OnClick_Campaign()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(OfflineCampaignSceneName);
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
