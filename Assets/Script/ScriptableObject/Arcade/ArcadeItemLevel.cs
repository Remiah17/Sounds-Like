using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArcadeItemLevel", menuName = "GameMode/OfflineMode/Arcade/ArcadeItemLevel")]
public class ArcadeItemLevel : ScriptableObject
{
    public ArcadeItem TargetItem;
    public ArcadeItem[] Distractors;
}
