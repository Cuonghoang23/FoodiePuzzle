using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBuyBooster : UIBase
{
    public static UIBuyBooster Instance;

    public TextMeshProUGUI txtNameBooster , txtCoinBuy, txtNumbuy;
    private BoosterItem itemData;
    
    

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public void Init(int idBooster)
    {
        Init(idBooster, 1);
    }

    public void Init(int idBooster, int quantity)
    {
        itemData = GameData.Instance.boosterData.GetBoosterById(idBooster);

        txtNameBooster.text = itemData.boosterName;
        txtCoinBuy.text = (itemData.coin * quantity).ToString();
        txtNumbuy.text = "x" + quantity.ToString();
    }



}
