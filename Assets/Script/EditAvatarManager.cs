using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditAvatarManager : MonoBehaviour
{
    private int ActiveScreen;
    private int ActiveEyeScree;
    [SerializeField]
    private AvatarDatabaseScriptableObject AvatarDatabase;
    [SerializeField]
    private AvatarManager AvatarPreviewManager;
    [SerializeField]
    private Button[] ShowBodyPartScreenButton;
    [SerializeField]
    private GameObject[] BodyPartScreens;
    [SerializeField]
    private Slider BodyRSlider;
    [SerializeField]
    private Slider BodyGSlider;
    [SerializeField]
    private Slider BodyBSlider;
    [SerializeField]
    private Slider UTRSlider;
    [SerializeField]
    private Slider UTGSlider;
    [SerializeField]
    private Slider UTBSlider;
    [SerializeField]
    private Slider LTRSlider;
    [SerializeField]
    private Slider LTGSlider;
    [SerializeField]
    private Slider LTBSlider;
    [SerializeField]
    private Slider HairRSlider;
    [SerializeField]
    private Slider HairGSlider;
    [SerializeField]
    private Slider HairBSlider;
    [SerializeField]
    private Slider EyebrowsRSlider;
    [SerializeField]
    private Slider EyebrowsGSlider;
    [SerializeField]
    private Slider EyebrowsBSlider;
    [SerializeField]
    private Slider PupilRSlider;
    [SerializeField]
    private Slider PupilGSlider;
    [SerializeField]
    private Slider PupilBSlider;
    [SerializeField]
    private Button[] EyesButtons;
    [SerializeField]
    private GameObject[] EyesChangeTypeScreens;
    [SerializeField]
    private GameObject[] EyesChangeColorScreens;
    [SerializeField]
    private Image[] ColorPreview;
    [SerializeField]
    private Button[] BodyTypeButtons;
    [SerializeField]
    private Button[] UpperTorsoTypeButtons;
    [SerializeField]
    private Button[] LowerTorsoTypeButtons;
    [SerializeField]
    private Button[] HairTypeButtons;
    [SerializeField]
    private Button[] EyesTypeButtons;
    [SerializeField]
    private Button[] EyebrowsTypeButtons;
    [SerializeField]
    private Button[] NoseTypeButtons;
    [SerializeField]
    private Button[] MouthTypeButtons;

    private SoundsScript theSoundScript;

    public void OnEnable()
    {
        Show_PartScreen(0);
    }

    private void Awake()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    private void Start()
    {
        theSoundScript = FindObjectOfType<SoundsScript>().GetComponent<SoundsScript>();
    }

    private void Update()
    {
        switch(ActiveScreen)
        {
            case 1:
                Color Col1 = Color.white;
                Col1.r = BodyRSlider.value;
                Col1.g = BodyGSlider.value;
                Col1.b = BodyBSlider.value;
                Col1.a = 250f;
                ColorPreview[0].color = Col1;
                break;
            case 2:
                Color Col2 = Color.white;
                Col2.r = UTRSlider.value;
                Col2.g = UTGSlider.value;
                Col2.b = UTBSlider.value;
                Col2.a = 250f;
                ColorPreview[1].color = Col2;
                break;
            case 3:
                Color Col3 = Color.white;
                Col3.r = LTRSlider.value;
                Col3.g = LTGSlider.value;
                Col3.b = LTBSlider.value;
                Col3.a = 250f;
                ColorPreview[2].color = Col3;
                break;
            case 4:
                Color Col4 = Color.white;
                Col4.r = HairRSlider.value;
                Col4.g = HairGSlider.value;
                Col4.b = HairBSlider.value;
                Col4.a = 250f;
                ColorPreview[3].color = Col4;
                break;
            case 5:
                if(ActiveEyeScree == 2)
                {
                    Color Col5 = Color.white;
                    Col5.r = EyebrowsRSlider.value;
                    Col5.g = EyebrowsGSlider.value;
                    Col5.b = EyebrowsBSlider.value;
                    Col5.a = 250f;
                    ColorPreview[5].color = Col5;
                }
                else if (ActiveEyeScree == 3)
                {
                    Color Col5 = Color.white;
                    Col5.r = PupilRSlider.value;
                    Col5.g = PupilGSlider.value;
                    Col5.b = PupilBSlider.value;
                    Col5.a = 250f;
                    ColorPreview[4].color = Col5;
                }
                break;
        }
    }

    public void Show_PartScreen(int PartID)
    {
        theSoundScript.PlaySelectSound();
        for (int i = 0; i < ShowBodyPartScreenButton.Length; i++)
        {
            if(i == PartID)
            {
                ShowBodyPartScreenButton[i].interactable = false;
            }
            else
            {
                ShowBodyPartScreenButton[i].interactable = true;
            }
        }

        for (int i = 0; i < BodyPartScreens.Length; i++)
        {
            if (i == PartID)
            {
                BodyPartScreens[i].SetActive(true);
                if(i == 4)
                {
                    ShowEyesScreen(1);
                }
            }
            else
            {
                BodyPartScreens[i].SetActive(false);
            }
        }

        switch(PartID)
        {
            case 0:
                BodyRSlider.value = AvatarDatabase.BodyColorUsed.r;
                BodyGSlider.value = AvatarDatabase.BodyColorUsed.g;
                BodyBSlider.value = AvatarDatabase.BodyColorUsed.b;
                for (int i = 0; i < BodyTypeButtons.Length; i++)
                {
                    if(AvatarDatabase.BodyLocked[i])
                    {
                        BodyTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        BodyTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 1:
                UTRSlider.value = AvatarDatabase.UpperTorsoColorUsed.r;
                UTGSlider.value = AvatarDatabase.UpperTorsoColorUsed.g;
                UTBSlider.value = AvatarDatabase.UpperTorsoColorUsed.b;
                for (int i = 0; i < UpperTorsoTypeButtons.Length; i++)
                {
                    if (AvatarDatabase.UpperTorsoLocked[i])
                    {
                        UpperTorsoTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        UpperTorsoTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 2:
                LTRSlider.value = AvatarDatabase.LowerTorsoColorUsed.r;
                LTRSlider.value = AvatarDatabase.LowerTorsoColorUsed.g;
                LTRSlider.value = AvatarDatabase.LowerTorsoColorUsed.b;
                for (int i = 0; i < LowerTorsoTypeButtons.Length; i++)
                {
                    if (AvatarDatabase.LowerTorsoLocked[i])
                    {
                        LowerTorsoTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        LowerTorsoTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                HairRSlider.value = AvatarDatabase.HairColor.r;
                HairGSlider.value = AvatarDatabase.HairColor.g;
                HairBSlider.value = AvatarDatabase.HairColor.b;
                for (int i = 0; i < HairTypeButtons.Length; i++)
                {
                    if (AvatarDatabase.HairLocked[i])
                    {
                        HairTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        HairTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 4:
                if(ActiveEyeScree == 1)
                {
                    for (int i = 0; i < EyesTypeButtons.Length; i++)
                    {
                        if (AvatarDatabase.EyesLocked[i])
                        {
                            EyesTypeButtons[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            EyesTypeButtons[i].gameObject.SetActive(true);
                        }
                    }
                }
                else if(ActiveEyeScree == 2)
                {
                    EyebrowsRSlider.value = AvatarDatabase.EyebrowColor.r;
                    EyebrowsGSlider.value = AvatarDatabase.EyebrowColor.g;
                    EyebrowsBSlider.value = AvatarDatabase.EyebrowColor.b;
                    for (int i = 0; i < EyebrowsTypeButtons.Length; i++)
                    {
                        if (AvatarDatabase.EyebrowLocked[i])
                        {
                            EyebrowsTypeButtons[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            EyebrowsTypeButtons[i].gameObject.SetActive(true);
                        }
                    }
                }
                else if (ActiveEyeScree == 3)
                {
                    PupilRSlider.value = AvatarDatabase.PupilColor.r;
                    PupilGSlider.value = AvatarDatabase.PupilColor.g;
                    PupilBSlider.value = AvatarDatabase.PupilColor.b;
                }
                break;
            case 5:
                for (int i = 0; i < NoseTypeButtons.Length; i++)
                {
                    if (AvatarDatabase.NoseLocked[i])
                    {
                        NoseTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        NoseTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 6:
                for (int i = 0; i < MouthTypeButtons.Length; i++)
                {
                    if (AvatarDatabase.MouthLocked[i])
                    {
                        MouthTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        MouthTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
        }
        ActiveScreen = PartID + 1;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Change_BodyType(int bodytype)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.BodyIDInUse = bodytype;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Change_UpperTorsoType(int type)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.UpperTorsoIDInUse = type;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
   public void Change_LowerTorsoType(int type)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.LowerTorsoIDInUse = type;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Change_HairType(int type)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.HairIDInUse = type;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Change_EyesType(int type)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.EyesIDInUse = type;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Change_EyebrowsType(int type)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.EyebrowsInUse = type;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Change_NoseType(int type)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.NoseIDInUse = type;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Change_MouthType(int type)
    {
        theSoundScript.PlaySelectSound();
        AvatarDatabase.MouthIDInUse = type;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Color_ChangeBody()
    {
        theSoundScript.PlaySelectSound();
        Color newColor = Color.white;
        newColor.r = BodyRSlider.value;
        newColor.g = BodyGSlider.value;
        newColor.b = BodyBSlider.value;
        newColor.a = 250f;
        AvatarDatabase.BodyColorUsed = newColor;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Color_ChangeUT()
    {
        theSoundScript.PlaySelectSound();
        Color newColor = Color.white;
        newColor.r = UTRSlider.value;
        newColor.g = UTGSlider.value;
        newColor.b = UTBSlider.value;
        newColor.a = 250f;
        AvatarDatabase.UpperTorsoColorUsed = newColor;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Color_ChangeLT()
    {
        theSoundScript.PlaySelectSound();
        Color newColor = Color.white;
        newColor.r = LTRSlider.value;
        newColor.g = LTGSlider.value;
        newColor.b = LTBSlider.value;
        newColor.a = 250f;
        AvatarDatabase.LowerTorsoColorUsed = newColor;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Color_ChangeHair()
    {
        theSoundScript.PlaySelectSound();
        Color newColor = Color.white;
        newColor.r = HairRSlider.value;
        newColor.g = HairGSlider.value;
        newColor.b = HairBSlider.value;
        newColor.a = 250f;
        AvatarDatabase.HairColor = newColor;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Color_ChangeEyebrows()
    {
        theSoundScript.PlaySelectSound();
        Color newColor = Color.white;
        newColor.r = EyebrowsRSlider.value;
        newColor.g = EyebrowsGSlider.value;
        newColor.b = EyebrowsBSlider.value;
        newColor.a = 250f;
        AvatarDatabase.EyebrowColor = newColor;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void Color_ChangePupils()
    {
        theSoundScript.PlaySelectSound();
        Color newColor = Color.white;
        newColor.r = PupilRSlider.value;
        newColor.g = PupilGSlider.value;
        newColor.b = PupilBSlider.value;
        newColor.a = 250f;
        AvatarDatabase.PupilColor = newColor;
        AvatarPreviewManager.SetupPlayersAvatar();
    }
    public void ShowEyesScreen(int screenNumber)
    {
        theSoundScript.PlaySelectSound();
        for (int i = 0; i < EyesButtons.Length; i++)
        {
            if (i == screenNumber - 1)
            {
                EyesButtons[i].interactable = false;
            }
            else
            {
                EyesButtons[i].interactable = true;
            }
        }

       switch(screenNumber)
        {
            case 1:
                EyesChangeTypeScreens[0].SetActive(true);
                EyesChangeTypeScreens[1].SetActive(false);
                EyesChangeColorScreens[0].SetActive(false);
                EyesChangeColorScreens[1].SetActive(false);
                for (int i = 0; i < EyesTypeButtons.Length; i++)
                {
                    if (AvatarDatabase.EyesLocked[i])
                    {
                        EyesTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        EyesTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 2:
                EyesChangeTypeScreens[1].SetActive(true);
                EyesChangeTypeScreens[0].SetActive(false);
                EyesChangeColorScreens[0].SetActive(true);
                EyesChangeColorScreens[1].SetActive(false);
                EyebrowsRSlider.value = AvatarDatabase.EyebrowColor.r;
                EyebrowsGSlider.value = AvatarDatabase.EyebrowColor.g;
                EyebrowsBSlider.value = AvatarDatabase.EyebrowColor.b;
                for (int i = 0; i < EyebrowsTypeButtons.Length; i++)
                {
                    if (AvatarDatabase.EyebrowLocked[i])
                    {
                        EyebrowsTypeButtons[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        EyebrowsTypeButtons[i].gameObject.SetActive(true);
                    }
                }
                break;
            case 3:
                EyesChangeTypeScreens[1].SetActive(false);
                EyesChangeTypeScreens[0].SetActive(false);
                EyesChangeColorScreens[0].SetActive(false);
                EyesChangeColorScreens[1].SetActive(true);
                PupilRSlider.value = AvatarDatabase.PupilColor.r;
                PupilGSlider.value = AvatarDatabase.PupilColor.g;
                PupilBSlider.value = AvatarDatabase.PupilColor.b;
                break;
        }
        ActiveEyeScree = screenNumber;
    }
}
