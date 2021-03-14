using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleHelpManager : MonoBehaviour
{
    [SerializeField]
    private GameObject HelpButton;
    [SerializeField]
    private GameObject HelpScreen;
    [SerializeField]
    private GameObject TitleHelpScreen;
    [SerializeField]
    private GameObject LoggedInHelpScreen;
    [SerializeField]
    private GameObject RegisterScreen;
    [SerializeField]
    private GameObject LoginScreen;
    [SerializeField]
    private GameObject LogedinScreen;
    [SerializeField]
    private GameObject SettingsScreen;
    [SerializeField]
    private GameObject ChangeAvatarScreen;
    [SerializeField]
    private GameObject ChangeNameScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void ShowHideHelp()
    {
        if(LogedinScreen.activeInHierarchy && !LoggedInHelpScreen.activeInHierarchy)
        {
            HelpScreen.SetActive(true);
            LoggedInHelpScreen.SetActive(true);
            TitleHelpScreen.SetActive(false);
        }
        else if (LogedinScreen.activeInHierarchy && LoggedInHelpScreen.activeInHierarchy)
        {
            HelpScreen.SetActive(false);
            LoggedInHelpScreen.SetActive(false);
            TitleHelpScreen.SetActive(false);
        }
        
        if (!LogedinScreen.activeInHierarchy && !TitleHelpScreen.activeInHierarchy)
        {
            HelpScreen.SetActive(true);
            LoggedInHelpScreen.SetActive(false);
            TitleHelpScreen.SetActive(true);
        }
        else if (!LogedinScreen.activeInHierarchy && TitleHelpScreen.activeInHierarchy)
        {
            HelpScreen.SetActive(false);
            LoggedInHelpScreen.SetActive(false);
            TitleHelpScreen.SetActive(false);
        }
    }

    public void HideHelpButton()
    {
        HelpButton.SetActive(false);
    }

    public void ShowHelpButton()
    {
        HelpButton.SetActive(true);
    }
}
