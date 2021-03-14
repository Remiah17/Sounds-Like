using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArcadeItem", menuName = "GameMode/OfflineMode/Arcade/ArcadeItem")]
public class ArcadeItem : ScriptableObject
{
    public string ItemName;
    public AudioClip ItemSound;
    public Sprite ItemSprite;
    public int PointsToGive;
}
