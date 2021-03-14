using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArcadeItemList", menuName = "GameMode/OfflineMode/Arcade/ArcadeItemList")]
public class ArcadeItemList : ScriptableObject
{
    public ArcadeItemLevel[] ItemLevel;
    public ArcadeItemLevel[] NormalItemLevel;
    public ArcadeItemLevel[] HardItemLevel;
    public ArcadeItemLevel[] ExtremeItemLevel;
    public bool[] ItemDone;
    public bool[] NormalItemDone;
    public bool[] HardItemDone;
    public bool[] ExtremeItemDone;
}
