using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarManager : MonoBehaviour
{
    public Image BodyImage;
    public Image UpperTorsoImage;
    public Image LowerTorsoImage;
    public Image HairImage;
    public Image EyesBGImage;
    public Image PupilsImage;
    public Image EyeLashesImage;
    public Image EyebrowsImage;
    public Image NoseImage;
    public Image MouthImage;

    public AvatarDatabaseScriptableObject AvatarDatabase;

    public void Awake()
    {
        SetupPlayersAvatar();
    }

    public void SetupPlayersAvatar()
    {
        BodyImage.color = AvatarDatabase.BodyColorUsed;
        BodyImage.sprite = AvatarDatabase.BodySprite[AvatarDatabase.BodyIDInUse - 1];
        UpperTorsoImage.color = AvatarDatabase.UpperTorsoColorUsed;
        UpperTorsoImage.sprite = AvatarDatabase.UpperTorsoSprite[AvatarDatabase.UpperTorsoIDInUse - 1];
        LowerTorsoImage.color = AvatarDatabase.LowerTorsoColorUsed;
        LowerTorsoImage.sprite = AvatarDatabase.LowerTorsoSprite[AvatarDatabase.LowerTorsoIDInUse - 1];
        HairImage.sprite = AvatarDatabase.HairSprite[AvatarDatabase.HairIDInUse - 1];
        HairImage.color = AvatarDatabase.HairColor;
        EyesBGImage.sprite = AvatarDatabase.EyeBG[AvatarDatabase.EyesIDInUse - 1];
        PupilsImage.color = AvatarDatabase.PupilColor;
        EyeLashesImage.sprite = AvatarDatabase.EyeLashes[AvatarDatabase.EyesIDInUse - 1];
        EyebrowsImage.sprite = AvatarDatabase.EyebrowSprite[AvatarDatabase.EyebrowsInUse - 1];
        EyebrowsImage.color = AvatarDatabase.EyebrowColor;
        NoseImage.sprite = AvatarDatabase.NoseSprite[AvatarDatabase.NoseIDInUse - 1];
        MouthImage.sprite = AvatarDatabase.MouthSprite[AvatarDatabase.MouthIDInUse - 1];
    }
}
