using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBtnLV : MonoBehaviour
{
    [Header("UI")]
    public GameObject iconLock;
    public Image item;
    public Color colorLock;
    public TextMeshProUGUI textLV;
    public Button btnItem;

    [SerializeField] private int lv;
    [SerializeField] private bool isLocked;

    private void Awake()
    {
        if (btnItem == null)
            btnItem = GetComponent<Button>();
    }

    public void Init(int lv, bool isLock)
    {
        this.lv = lv;
        this.isLocked = isLock;

        textLV.text = lv.ToString();

        // Hiển thị UI lock/unlock
        iconLock.SetActive(isLock);
        textLV.gameObject.SetActive(!isLock);
        item.color = isLock ? colorLock : Color.white;

        // Nếu lock thì không cho bấm
        btnItem.interactable = !isLock;

        // Gán sự kiện click
        btnItem.onClick.RemoveAllListeners();
        btnItem.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (isLocked)
            return;

        SetLevelSelect();

        if (UISelectLV.Instance != null)
        {
            UISelectLV.Instance.ButtonPlayClick();
        }
    }

    private void SetLevelSelect()
    {
        GameData.Instance.CurrentLevel = lv;
        Debug.Log("Selected Level: " + lv);
    }
}