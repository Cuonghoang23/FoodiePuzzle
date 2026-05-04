using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();

#if UNITY_EDITOR
    [ContextMenu("🔥 Auto Generate Levels")]
    public void GenerateLevels()
    {
        levels.Clear();

        int totalUnlocked = 0;

        for (int lv = 1; lv <= 20; lv++)
        {
            LevelData data = new LevelData();
            data.level = lv;

            //  time
            data.playTime = (lv == 1) ? 0 : 300;

            //  stove
            data.stoveCount = Mathf.Min(3 + lv, 12);

            // unlock item
            if (lv == 1)
            {
                data.unlockItems = new List<int> { 1, 2, 3 };
            } else if ( lv == 20)
            {
                data.unlockItems = new List<int> { lv };
            }
            else
            {
                data.unlockItems = new List<int> {  lv * 2, lv * 2 + 1 };
            }
        
            

                // shipper
            data.shipperRate = (lv < 6) ? 0 : Mathf.Clamp01(0.05f + (lv - 6) * 0.03f);

                // item count
           data.totalItem = Mathf.RoundToInt(
                (totalUnlocked + data.unlockItems.Count) * 3f * Mathf.FloorToInt(Mathf.Pow(lv, 1f / 3f))
                ); //bằng (số item unlock màn + số item đã unlock) *3 * (căn bậc 3 của lv làm tròn xuống)

            Debug.Log(totalUnlocked + " ; " + data.unlockItems.Count +" ; "+ Mathf.FloorToInt(Mathf.Pow(lv, 1f / 3f)));
            totalUnlocked += data.unlockItems.Count;
            levels.Add(data);
            }

        Debug.Log("✅ Generated Level Data!");

        EditorUtility.SetDirty(this); // bắt Unity lưu lại
    }
#endif
}