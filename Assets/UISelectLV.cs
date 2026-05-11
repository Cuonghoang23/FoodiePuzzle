using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectLV : UIBase
{
    public static UISelectLV Instance;
    public List<ItemBtnLV> items;
    public GameObject UIPlay;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < items.Count; i++) {
            items[i].Init(i + 1, i + 1 > GameData.Instance.LevelUnlock);
        }
    }

    public void ButtonPlayClick()
    {
        UIPlay.SetActive(true);
        Debug.Log("Select Level");
    }
}
