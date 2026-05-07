using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectBoosterUI : MonoBehaviour
{
    public GameObject lockBooster, unLockBooster;
    public TextMeshProUGUI textNumBooster, textLevelUnlock;
    public Button buttonSelect;

    public int boosterId;
    public Image imgBooster;

    private BoosterItem itemData;

    private void Start()
    {
        itemData = GameData.Instance.boosterData.GetBoosterById(boosterId);

        if (itemData == null)
        {
            Debug.LogError("Booster not found ID: " + boosterId);
            return;
        }

        bool isLock = GameData.Instance.LevelUnlock < itemData.levelUnlock;
        lockBooster.SetActive(isLock);
        unLockBooster.SetActive(!isLock);

        textLevelUnlock.text = itemData.levelUnlock.ToString();

        int count = GameData.Instance.GetBoosterCount(itemData.boosterType);
        textNumBooster.text = count > 0 ? count.ToString() : "+";

        imgBooster.sprite = itemData.icon;

        buttonSelect.onClick.RemoveAllListeners();
        buttonSelect.onClick.AddListener(() => ButtonSelectClick(itemData.id));
    }

    public void ButtonSelectClick(int id)
    {
        UIBuyBooster.Instance.Init(id);
    }
}