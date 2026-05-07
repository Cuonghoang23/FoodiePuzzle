using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectLV : MonoBehaviour
{
    public List<ItemBtnLV> items;
    
    void Start()
    {
        for (int i = 0; i < items.Count; i++) {
            items[i].Init(i + 1, i + 1 > GameData.Instance.LevelUnlock);
        }
    }


}
