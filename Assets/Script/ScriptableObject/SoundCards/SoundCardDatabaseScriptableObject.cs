using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundCardDatabase", menuName = "GameMode/OnlineMode/EliminationGame/SoundCardDatabase")]
public class SoundCardDatabaseScriptableObject : ScriptableObject
{
    public SoundCardItemScriptableObject[] SoundCardList;
    public bool[] SoundCardUnlocked;
    public int[] SoundCardAmount;
    public int[] AmountBeingUsed;
}
