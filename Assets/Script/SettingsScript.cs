using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer BGMAudioMixer;
    [SerializeField]
    private AudioMixer SFXAudioMixer;
    [SerializeField]
    private Slider BGMSlider;
    private float BGMVolume;
    [SerializeField]
    private Slider SFXSlider;
    private float SFXVolume;
    [SerializeField]
    private GameObject CreditsScreen;
    [SerializeField]
    private GameObject EraseDataScreen;

    private SoundsScript theSoundScript;

    private void OnEnable()
    {
        BGMSlider.value = BGMVolume;
        SFXSlider.value = SFXVolume;
    }

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }


    private void Start()
    {
        CreditsScreen.SetActive(false);
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    public void SetBGMVolume(float volume)
    {
        BGMAudioMixer.SetFloat("BGMVolume", volume);
        BGMVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXAudioMixer.SetFloat("SFXVolume", volume);
        SFXVolume = volume;
    }

    public void OnClick_Credits()
    {
        theSoundScript.PlaySelectSound();
        if (!CreditsScreen.activeInHierarchy)
        {
            CreditsScreen.SetActive(true);
        }
        else
        {
            CreditsScreen.SetActive(false);
        }
    }

    public void CloseSettings()
    {
        theSoundScript.PlaySelectSound();
        gameObject.SetActive(false);
    }

    public void OnClickResetData()
    {
        theSoundScript.PlaySelectSound();
        if (EraseDataScreen.activeInHierarchy)
        {
            EraseDataScreen.SetActive(false);
        }
        else
        {
            EraseDataScreen.SetActive(true);
        }
    }

    public void YesEraseData()
    {
        theSoundScript.PlaySelectSound();
        //set player prefabs
        PlayerPrefs.SetInt("EasyLevelComplete", 0);
        PlayerPrefs.SetInt("NormalLevelComplete", 0);
        PlayerPrefs.SetInt("HardLevelComplete", 0);
        PlayerPrefs.SetInt("ExtremeLevelComplete", 0);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("CampaignLevelUnlocked", 0);
        PlayerPrefs.SetInt("Coins", 0);
        OnClickResetData();

    }

    public void NoEraseData()
    {
        theSoundScript.PlaySelectSound();
        OnClickResetData();
    }

}
