using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundOfTheSeasonItemlist", menuName = "GameMode/OnlineMode/SoundsOfTheSeason/SoundOfTheSeasonItemlist")]
public class SoundOfTheSeasonItemList : ScriptableObject
{
    public SoundOfTheSeasonItem[] SoundsOfTheSeasonItemList;
    public bool[] Unlocked;
}
