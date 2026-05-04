using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int level;

    [Header("Core")]
    public int stoveCount;
    public int lockedByItem;
    public int lockedByCoin;

    [Header("Item")]
    public int totalItem;
    public List<int> unlockItems;

    [Header("Gameplay")]
    [Range(0, 1)] public float shipperRate;
    public float playTime; // seconds

}