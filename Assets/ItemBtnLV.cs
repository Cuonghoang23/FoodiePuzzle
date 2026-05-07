using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBtnLV : MonoBehaviour
{
    public GameObject iconLock;
    public Image item;
    public Color colorLock;
    public TextMeshProUGUI textLV;
    public Button btnItem;

    public void Init(int lv, bool isLock)
    {
        if (isLock)
        {
            item.color = colorLock;
            iconLock.SetActive(true);
            textLV.gameObject.SetActive(false);
        }
        else
        {
            iconLock.SetActive(false);
            textLV.gameObject.SetActive(true);
            item.color = Color.white;
        }
    }
    

}
