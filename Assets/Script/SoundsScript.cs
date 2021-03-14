using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsScript : MonoBehaviour
{

    public AudioSource BGM;
    public AudioSource SFX;

    [SerializeField]
    private int SceneBGMNumber;
    public AudioClip[] BGMClip;
    [SerializeField]
    private AudioClip SelectClip;
    [SerializeField]
    private AudioClip CorrectClip;
    [SerializeField]
    private AudioClip WrongClip;
    [SerializeField]
    private AudioClip StarClip;

    private int CurrentlyPlayingBGM;
    private float CurrentBGMVolume;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void PlayBGM(int BGMNumberttoPlay)
    {
        StartCoroutine(PlayNewBGM(BGMNumberttoPlay));
    }

    public void PlaySelectSound()
    {
        SFX.clip = SelectClip;
        if(SFX.isPlaying)
        {
            SFX.Stop();
        }
        SFX.Play();
    }

    public void PlayCorrectSound()
    {
        SFX.clip = CorrectClip;
        if (SFX.isPlaying)
        {
            SFX.Stop();
        }
        SFX.Play();
    }

    public void PlayWrongSound()
    {
        SFX.clip = WrongClip;
        if (SFX.isPlaying)
        {
            SFX.Stop();
        }
        SFX.Play();
    }

    public void PlayStarSound()
    {
        SFX.clip = StarClip;
        if (SFX.isPlaying)
        {
            SFX.Stop();
        }
        SFX.Play();
    }

    public void PlaySpecificFX(AudioClip toPlay)
    {
        StartCoroutine(PlayFXAndPauseBGM(toPlay));
    }
    
    public void VolumeBackBGM()
    {
        BGM.volume = CurrentBGMVolume;
    }

    IEnumerator PlayNewBGM(int BGMToPlay)
    {
        if(CurrentlyPlayingBGM != BGMToPlay)
        {
            BGM.Stop();
            yield return new WaitForSeconds(0.5f);
            BGM.clip = BGMClip[BGMToPlay];
            CurrentlyPlayingBGM = BGMToPlay;
            BGM.Play();
        }
       
    }

    IEnumerator PlayFXAndPauseBGM(AudioClip toPlay)
    {
        if(BGM.volume != 0)
        {
            CurrentBGMVolume = BGM.volume;
        }
        BGM.volume = 0;
        SFX.clip = toPlay;
        if (SFX.isPlaying)
        {
            SFX.Stop();
        }
        SFX.Play();
        yield return new WaitForSeconds(3);
        BGM.volume = CurrentBGMVolume;
    }

    public void PlayOnlineSound(AudioClip ToPlay)
    {
        SFX.clip = ToPlay;
        if (SFX.isPlaying)
        {
            SFX.Stop();
        }
        SFX.Play();
    }
}
