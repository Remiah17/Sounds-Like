using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCampaignItemList", menuName = "GameMode/OfflineMode/Campaign/CampaignItemList")]
public class CampaignItemList : ScriptableObject
{
    public CampaignItem[] LevelItemList;
    public bool[] Unlocked;
}
