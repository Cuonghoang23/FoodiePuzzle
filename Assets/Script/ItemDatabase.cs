using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public LevelDatabase levelDatabase;
    public List<ItemData> items = new List<ItemData>();

#if UNITY_EDITOR
    [ContextMenu("Auto Generate Items (1-40)")]
    public void GenerateItems()
    {
        items.Clear();

        for (int i = 1; i <= 40; i++)
        {
            ItemData data = new ItemData();
            data.id = i;
            data.itemName = "Item" + i.ToString("D2");

            // load sprite theo tên: Item_1, Item_2...

            string path = $"Assets/Art/Item/Item_{i.ToString("D2")}.png";
            data.icon = AssetDatabase.LoadAssetAtPath<Sprite>(path);

            if (data.icon == null)
            {
                Debug.LogWarning($"❌ Missing sprite: {path}");
            }

            items.Add(data);
        }

        Debug.Log("✅ Generated Items!");

        EditorUtility.SetDirty(this);
    }
#endif
#if UNITY_EDITOR
    [ContextMenu("🔥 Sync Level Unlock From LevelDatabase")]
    public void SyncLevelUnlock()
    {
        if (levelDatabase == null)
        {
            Debug.LogError("❌ LevelDatabase is NULL");
            return;
        }

        foreach (var level in levelDatabase.levels)
        {
            foreach (var itemId in level.unlockItems)
            {
                var item = items.Find(i => i.id == itemId);
                if (item != null)
                {
                    item.levelUnlock = level.level;
                }
                else
                {
                    Debug.LogWarning("Not Item");
                }
            }
        }

        Debug.Log("✅ Sync Item Level Unlock Done!");

        EditorUtility.SetDirty(this);
    }
#endif
    public ItemData GetItem(int id)
    {
        return items.Find(i => i.id == id);
    }

    public List<ItemData> GetItemUnlock(int playerLevel)
    {
        return items.FindAll(i => i.levelUnlock == playerLevel);
    }

}