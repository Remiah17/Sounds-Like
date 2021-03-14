using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCampaignItem", menuName = "GameMode/OfflineMode/Campaign/CampaignItem")]
public class CampaignItem : ScriptableObject
{
    public string ItemName;
    public AudioClip ItemSound;
    public Sprite ItemSprite;
    public string[] PossibleAnswers;
    public int PointsToGive;
}
