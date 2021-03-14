using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScript : MonoBehaviour
{
    public GameObject HelpScreen;

    private SoundsScript theSoundScript;


    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    public void ShowHideScreen()
    {
        theSoundScript.PlaySelectSound();
        if(!HelpScreen.activeInHierarchy)
        {
            HelpScreen.SetActive(true);
        }
        else
        {
            HelpScreen.SetActive(false);
        }
    }
   
}
