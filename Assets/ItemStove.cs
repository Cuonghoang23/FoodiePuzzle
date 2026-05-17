using System.Collections.Generic;
using UnityEngine;

public enum StoveLockType
{
    None,
    Coin,
    Item
}

[System.Serializable]
public class StoveSpawnWave
{
    public List<int> itemIds = new List<int>();

    public StoveSpawnWave(List<int> itemIds)
    {
        this.itemIds = itemIds;
    }
}

public class ItemStove : MonoBehaviour
{
    [Header("Lock")]
    [SerializeField] private StoveLockType lockType = StoveLockType.None;
    [SerializeField] private GameObject lockCoinObj;
    [SerializeField] private GameObject lockItemObj;

    [SerializeField] private int unlockCoinPrice = 100;
    [SerializeField] private int unlockItemId = -1;

    [Header("Slots")]
    [SerializeField] private List<FoodSlot> foodSlots = new List<FoodSlot>();

    [Header("Spawn Waves")]
    [SerializeField] private List<StoveSpawnWave> spawnWaves = new List<StoveSpawnWave>();

    private int currentWaveIndex = 0;

    public bool IsUnlocked => lockType == StoveLockType.None;
    public bool IsEmpty => GetCurrentItemCount() <= 0;
    public bool HasNextWave => currentWaveIndex < spawnWaves.Count;

    private void Awake()
    {
        if (foodSlots.Count == 0)
        {
            foodSlots.AddRange(GetComponentsInChildren<FoodSlot>());
        }

        UpdateLockUI();
    }

    public void SetupSpawnWaves(List<StoveSpawnWave> waves)
    {
        spawnWaves.Clear();

        if (waves != null)
            spawnWaves.AddRange(waves);

        currentWaveIndex = 0;

        ClearAllSlots();
        UpdateLockUI();

        SpawnNextWave();
    }

    public void SpawnNextWave()
    {
        if (!IsUnlocked) return;
        if (!IsEmpty) return;
        if (!HasNextWave) return;

        StoveSpawnWave wave = spawnWaves[currentWaveIndex];
        currentWaveIndex++;

        for (int i = 0; i < wave.itemIds.Count; i++)
        {
            FoodSlot slot = GetFirstEmptySlot();

            if (slot == null)
                return;

            int itemId = wave.itemIds[i];
            Sprite sprite = GameData.Instance.ItemDatabase.items[itemId].icon;

            slot.foodItem.SetItem(itemId, sprite);
        }
    }

    public void OnItemRemoved()
    {
        if (!IsUnlocked) return;

        if (IsEmpty)
        {
            SpawnNextWave();
        }
    }

    public void SetLockNone()
    {
        lockType = StoveLockType.None;
        UpdateLockUI();

        if (IsEmpty)
            SpawnNextWave();
    }

    public void SetLockCoin(int price)
    {
        lockType = StoveLockType.Coin;
        unlockCoinPrice = price;
        UpdateLockUI();
    }

    public void SetLockItem(int itemId)
    {
        lockType = StoveLockType.Item;
        unlockItemId = itemId;
        UpdateLockUI();
    }

    public void TryUnlockByCoin()
    {
        if (lockType != StoveLockType.Coin) return;

        if (GameData.Instance.Coin < unlockCoinPrice)
        {
            Debug.Log("Không đủ coin để mở bếp");
            return;
        }

        GameData.Instance.Coin -= unlockCoinPrice;
        SetLockNone();
    }

    public void TryUnlockByItem(int itemId)
    {
        if (lockType != StoveLockType.Item) return;

        if (unlockItemId == itemId)
        {
            SetLockNone();
        }
    }

    private FoodSlot GetFirstEmptySlot()
    {
        for (int i = 0; i < foodSlots.Count; i++)
        {
            if (foodSlots[i].IsEmpty())
                return foodSlots[i];
        }

        return null;
    }

    private int GetCurrentItemCount()
    {
        int count = 0;

        for (int i = 0; i < foodSlots.Count; i++)
        {
            if (!foodSlots[i].IsEmpty())
                count++;
        }

        return count;
    }

    private void ClearAllSlots()
    {
        for (int i = 0; i < foodSlots.Count; i++)
        {
            foodSlots[i].foodItem.Clear();
        }
    }

    private void UpdateLockUI()
    {
        if (lockCoinObj != null)
            lockCoinObj.SetActive(lockType == StoveLockType.Coin);

        if (lockItemObj != null)
            lockItemObj.SetActive(lockType == StoveLockType.Item);

        bool locked = !IsUnlocked;

        for (int i = 0; i < foodSlots.Count; i++)
        {
            foodSlots[i].SetLocked(locked);
        }
    }
}