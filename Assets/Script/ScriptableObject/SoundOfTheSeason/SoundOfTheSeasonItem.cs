using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundOfTheSeasonItem", menuName = "GameMode/OnlineMode/SoundsOfTheSeason/SoundOfTheSeasonItem")]
public class SoundOfTheSeasonItem : ScriptableObject
{
    public string ItemName;
    public AudioClip ItemSound;
    public Sprite ItemSprite;
    public string[] PossibleAnswers;
    public int PointsToGive;
    public int[] Gifts;
}
