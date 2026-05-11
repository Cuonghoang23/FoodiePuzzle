using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoosterData", menuName = "Game/Booster Data")]
public class BoosterData : ScriptableObject
{
    public List<BoosterItem> boosters;

    public BoosterItem GetBoosterById(int id)
    {
        return boosters.Find(b => b.id == id);
    }
}

[System.Serializable]
public class BoosterItem
{
    public int id;
    public string boosterName;
    public Sprite icon;
    public int coin;

    public int levelUnlock;
    public string description;

    public BoosterType boosterType;
}

public enum BoosterType
{
    Magnet,
    Shuffle,
    MagicWand,
    Time,
    EnergyInfinity,
    MagnetPro,
    X2Star,
    UnlimitedTime
}