using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAvatar", menuName = "Character/Avatar/Avatar")]

public class AvatarDatabaseScriptableObject : ScriptableObject
{
    [Header("BODY")]
    public int BodyIDInUse;
    public Sprite[] BodySprite;
    public Color BodyColorUsed;
    public bool[] BodyLocked;

    [Header("UPPER TORSO")]
    public int UpperTorsoIDInUse;
    public Sprite[] UpperTorsoSprite;
    public Color UpperTorsoColorUsed;
    public bool[] UpperTorsoLocked;

    [Header("LOWER TORSO")]
    public int LowerTorsoIDInUse;
    public Sprite[] LowerTorsoSprite;
    public Color LowerTorsoColorUsed;
    public bool[] LowerTorsoLocked;

    [Header("HAIR")]
    public int HairIDInUse;
    public bool[] HairLocked;
    public Sprite[] HairSprite;
    public Color HairColor;

    [Header("MOUTH")]
    public int MouthIDInUse;
    public bool[] MouthLocked;
    public Sprite[] MouthSprite;

    [Header("NOSE")]
    public int NoseIDInUse;
    public bool[] NoseLocked;
    public Sprite[] NoseSprite;

    [Header("EYE")]
    public int EyesIDInUse;
    public bool[] EyesLocked;
    public Sprite[] EyeBG;
    public Sprite[] EyeLashes;

    [Header("PUPILS")]
    public Sprite PupilShine;
    public Sprite Pupil;
    public Color PupilColor;

    [Header("EYEBROWS")]
    public int EyebrowsInUse;
    public bool[] EyebrowLocked;
    public Sprite[] EyebrowSprite;
    public Color EyebrowColor;



}
