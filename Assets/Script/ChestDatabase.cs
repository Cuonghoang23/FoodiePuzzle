using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestData
{
    public int priceOpen;          // Giá mở chest
    public int boosterRandomCount; // Số booster ngẫu nhiên
    public int coinReward;         // Coin thưởng
    public Sprite icon;            // Hình ảnh chest
}

[CreateAssetMenu(fileName = "ChestDatabase", menuName = "Game/Chest Database")]
public class ChestDatabase : ScriptableObject
{
    public List<ChestData> chests = new List<ChestData>();
}