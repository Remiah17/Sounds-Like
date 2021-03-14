using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundCardItem", menuName = "GameMode/OnlineMode/EliminationGame/SoundCardItem")]
public class SoundCardItemScriptableObject : ScriptableObject
{
    public int SoundCardID;
    public CampaignItem CampaignItemData;
    public int Rarity;
    public int PointsToGive;
    public string ArtistName;
    public int Difficulty;
}
