using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArcadeModeManager : MonoBehaviour
{
    [Header("SCENES")]
    [SerializeField]
    private string ArcadeGameScene;
    [SerializeField]
    private string OfflineModeScene;
    [Header("SCREENS")]
    [SerializeField]
    private GameObject SettingScreen;
    private SoundsScript theSoundScript;
    [SerializeField]
    private Text CoinText;
    [Header("DIFFICULTY BUTTONS")]
    [SerializeField]
    private Button NormalButton;
    [SerializeField]
    private Button HardButton;
    [SerializeField]
    private Button ExtremeButton;
    [Header("DIFFICULTY STARS")]
    [SerializeField]
    private GameObject[] EasyStar;
    [SerializeField]
    private GameObject[] NormalStar;
    [SerializeField]
    private GameObject[] HardStar;
    [SerializeField]
    private GameObject[] ExtremeStar;
    [Header("SCORE")]
    [SerializeField]
    private Text ScoreText;
    private bool StarCountChecked;


    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        StarCountChecked = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
        PlayerPrefs.SetInt("GuessItDifficulty", 0);
        PlayerPrefs.SetInt("MatchItDifficulty", 0);
       
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = Mathf.Round(PlayerPrefs.GetInt("Score")).ToString();
        if(!StarCountChecked)
        {
            for (int i = 0; i < EasyStar.Length; i++)
            {
                if (PlayerPrefs.GetInt("EasyLevelComplete") > i)
                {
                    EasyStar[i].SetActive(true);
                }
                else
                {
                    EasyStar[i].SetActive(false);
                }
            }

            if (PlayerPrefs.GetInt("EasyLevelComplete") > 0)
            {
                NormalButton.interactable = true;

                for (int i = 0; i < NormalStar.Length; i++)
                {
                    if (PlayerPrefs.GetInt("NormalLevelComplete") > i)
                    {
                        NormalStar[i].SetActive(true);
                    }
                    else
                    {
                        NormalStar[i].SetActive(false);
                    }
                }
            }
            else
            {
                NormalButton.interactable = false;
            }

            if (PlayerPrefs.GetInt("NormalLevelComplete") > 0)
            {
                HardButton.interactable = true;

                for (int i = 0; i < HardStar.Length; i++)
                {
                    if (PlayerPrefs.GetInt("HardLevelComplete") > i)
                    {
                        HardStar[i].SetActive(true);
                    }
                    else
                    {
                        HardStar[i].SetActive(false);
                    }
                }
            }
            else
            {
                HardButton.interactable = false;
            }

            if (PlayerPrefs.GetInt("HardLevelComplete") > 0)
            {
                ExtremeButton.interactable = true;

                for (int i = 0; i < ExtremeStar.Length; i++)
                {
                    if (PlayerPrefs.GetInt("ExtremeLevelComplete") > i)
                    {
                        ExtremeStar[i].SetActive(true);
                    }
                    else
                    {
                        ExtremeStar[i].SetActive(false);
                    }
                }
            }
            else
            {
                ExtremeButton.interactable = false;
            }
            CoinText.text = "" + PlayerPrefs.GetInt("Coins");
            StarCountChecked = true;
        }
    }

    public void OnClickGuessItDifficulty(int _DifficultyID)
    {
        theSoundScript.PlaySelectSound();
        PlayerPrefs.SetInt("GuessItDifficulty", _DifficultyID);
        SceneManager.LoadScene(ArcadeGameScene);
    }

    public void OnClickBack()
    {
        theSoundScript.PlaySelectSound();
        SceneManager.LoadScene(OfflineModeScene);
    }

    public void OnClickSetting()
    {
        theSoundScript.PlaySelectSound();
        if (SettingScreen.activeInHierarchy)
        {
            StarCountChecked = false;
            SettingScreen.SetActive(false);
        }
        else
        {
            SettingScreen.SetActive(true);
        }
    }

}
