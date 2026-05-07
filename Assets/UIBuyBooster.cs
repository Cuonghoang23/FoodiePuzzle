using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBuyBooster : MonoBehaviour
{
    public static UIBuyBooster Instance;

    public TextMeshProUGUI textTile , coinBuy;
    private BoosterItem itemData ;

    private void Awake()
    {
        Instance = this;
    }

    public void Init( int idBosster)
    {
        itemData = GameData.Instance.boosterData.GetBoosterById(idBosster);
    }

}
