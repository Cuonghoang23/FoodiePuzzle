using UnityEngine;

public class GrillLevelManager : MonoBehaviour
{
    public static GrillLevelManager Instance;


    [Header("Prefab")]
    [SerializeField] private GameObject ItemGrillSlotPrefab;
    [SerializeField] private Transform ItemGrillSlotParent;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateStoves(int ItemGrillSlotCount)
    {
        foreach (Transform child in ItemGrillSlotParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < ItemGrillSlotCount; i++)
        {
            Instantiate(ItemGrillSlotPrefab, ItemGrillSlotParent);
        }
    }

    public void SpawnItems(LevelData levelData)
    {
        Debug.Log("Spawn item theo totalItem: " + levelData.totalItem);

        // đoạn này lát nữa mình sẽ nối với ItemDatabase
        // để random item theo unlockItems
    }
}