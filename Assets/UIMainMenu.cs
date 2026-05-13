using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;

    [Header("Buttons")]
    [SerializeField] private RectTransform btnHome;
    [SerializeField] private RectTransform btnShop;
    [SerializeField] private RectTransform bgBtnBottom;

    [SerializeField] private GameObject txtHome;
    [SerializeField] private GameObject txtShop;

    [Header("Energy")]
    [SerializeField] private TextMeshProUGUI txtNumEnergy; 
    [SerializeField] private TextMeshProUGUI txtTimeAddEnergy;

    [SerializeField] private int maxEnergy = 5;
    [SerializeField] private int currentEnergy = 5;
    [SerializeField] private float timeAddEnergy = 15 * 60f; // 15 phút

    private float timerEnergy;

    [Header("Chess")]
    [SerializeField] private TextMeshProUGUI txtProgressChest;
    [SerializeField] private Image imgProgressChest;

    [SerializeField] private int currentChestIndex = 0;
    [SerializeField] private int currentStarChest = 0;
    private ChestDatabase chestDatabase;

    [Header("Animation")]
    [SerializeField] private float selectedScale = 1f;
    [SerializeField] private float unselectedScale = 0.65f;

    [SerializeField] private float selectedY = 20f;     // Button được chọn sẽ nhô lên
    [SerializeField] private float unselectedY = 0f;    // Button thường

    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease moveEase = Ease.OutCubic;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    private Tween moveTween;
    private Tween homeTween;
    private Tween shopTween;

    private Vector2 homeDefaultPos;
    private Vector2 shopDefaultPos;

    private void Awake()
    {
        Instance = this;

        homeDefaultPos = btnHome.anchoredPosition;
        shopDefaultPos = btnShop.anchoredPosition;
    }

    private void Start()
    {
        SelectButton(btnHome, btnShop, txtHome, txtShop, true);

        LoadEnergy();
        UpdateEnergyUI();

        LoadChestProgress();
        UpdateChestProgressUI();
    }

    private void Update()
    {
        UpdateEnergyTimer();
    }

    #region Energy
    private void LoadEnergy()
    {
        currentEnergy = GameData.Instance.NumEnergy;

        string savedTime = GameData.Instance.TimeADDEnergy;

        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
            timerEnergy = timeAddEnergy;
            return;
        }

        if (!string.IsNullOrEmpty(savedTime))
        {
            System.DateTime lastTime = System.DateTime.Parse(savedTime);
            System.TimeSpan timePassed = System.DateTime.Now - lastTime;

            int energyToAdd = Mathf.FloorToInt((float)timePassed.TotalSeconds / timeAddEnergy);

            if (energyToAdd > 0)
            {
                currentEnergy += energyToAdd;

                if (currentEnergy >= maxEnergy)
                {
                    currentEnergy = maxEnergy;
                    timerEnergy = timeAddEnergy;
                }
                else
                {
                    float remainSeconds = (float)timePassed.TotalSeconds % timeAddEnergy;
                    timerEnergy = timeAddEnergy - remainSeconds;

                    SaveEnergyStartTime();
                }
            }
            else
            {
                timerEnergy = timeAddEnergy - (float)timePassed.TotalSeconds;
            }
        }
        else
        {
            timerEnergy = timeAddEnergy;
            SaveEnergyStartTime();
        }

        SaveEnergy();
    }

    private void UpdateEnergyTimer()
    {
        if (currentEnergy >= maxEnergy)
        {
            txtTimeAddEnergy.text = "Full";
            return;
        }

        timerEnergy -= Time.deltaTime;

        if (timerEnergy <= 0)
        {
            AddEnergy(1);
        }

        UpdateEnergyUI();
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;

        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = maxEnergy;
            timerEnergy = timeAddEnergy;
        }
        else
        {
            timerEnergy = timeAddEnergy;
            SaveEnergyStartTime();
        }

        SaveEnergy();
        UpdateEnergyUI();
    }

    public bool UseEnergy(int amount)
    {
        if (currentEnergy < amount)
            return false;

        currentEnergy -= amount;

        if (currentEnergy < maxEnergy)
        {
            timerEnergy = timeAddEnergy;
            SaveEnergyStartTime();
        }

        SaveEnergy();
        UpdateEnergyUI();

        return true;
    }

    private void UpdateEnergyUI()
    {
        txtNumEnergy.text = currentEnergy.ToString();

        if (currentEnergy >= maxEnergy)
        {
            txtTimeAddEnergy.text = "Full";
        }
        else
        {
            int minutes = Mathf.FloorToInt(timerEnergy / 60f);
            int seconds = Mathf.FloorToInt(timerEnergy % 60f);

            txtTimeAddEnergy.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    private void SaveEnergy()
    {
        GameData.Instance.NumEnergy = currentEnergy;
    }

    private void SaveEnergyStartTime()
    {
        GameData.Instance.TimeADDEnergy = System.DateTime.Now.ToString();
    }
    #endregion

    #region chest
    private void LoadChestProgress()
    {
        chestDatabase = GameData.Instance.ChestDatabase;
        currentStarChest = GameData.Instance.NumStarChest;
        currentChestIndex = GameData.Instance.CurrentChestIndex;
    }

    [ContextMenu("Test Add Star")]
    public void TesAddStar()
    {
               AddStarChest(50);
    }

    public void AddStarChest(int amount)
    {
        currentStarChest += amount;

        SaveChestProgress();
        UpdateChestProgressUI();
    }

    private void UpdateChestProgressUI()
    {
        ChestData chest = chestDatabase.chests[currentChestIndex];

        int needStar = chest.priceOpen;

        txtProgressChest.text = currentStarChest + "/" + needStar;

        float fill = Mathf.Clamp01((float)currentStarChest / needStar);
        imgProgressChest.fillAmount = fill;
    }

    public bool CanOpenChest()
    {
        ChestData chest = chestDatabase.chests[currentChestIndex];
        return currentStarChest >= chest.priceOpen;
    }


    [ContextMenu("Test Open Chest")]
    public void OpenChest()
    {
        ChestData chest = chestDatabase.chests[currentChestIndex];

        if (currentStarChest < chest.priceOpen)
        {
            Debug.Log("Chưa đủ sao để mở chest");
            return;
        }

        currentStarChest -= chest.priceOpen;
        currentChestIndex++;

        // TODO: cộng thưởng
        // GameData.Instance.Coin += chest.coinReward;
        // Random booster theo chest.boosterRandomCount

        SaveChestProgress();
        UpdateChestProgressUI();

        Debug.Log("Mở chest thành công");
    }

    private void SaveChestProgress()
    {
        GameData.Instance.NumStarChest = currentStarChest;
        GameData.Instance.CurrentChestIndex = currentChestIndex;
    }

    #endregion

    public void ButtonHomeClick()
    {
        SelectButton(btnHome, btnShop, txtHome, txtShop);
        ButtonClick();
    }

    public void ButtonShopClick()
    {
        SelectButton(btnShop, btnHome, txtShop, txtHome);
        ButtonClick();
    }

    private void SelectButton(
        RectTransform selected,
        RectTransform unselected,
        GameObject selectedText,
        GameObject unselectedText,
        bool instant = false)
    {
        moveTween?.Kill();
        homeTween?.Kill();
        shopTween?.Kill();

        // ===== Hiện text =====
        selectedText.SetActive(true);
        unselectedText.SetActive(false);

        // ===== Move background chỉ theo X =====
        float targetX = selected.anchoredPosition.x;

        if (instant)
        {
            Vector2 bgPos = bgBtnBottom.anchoredPosition;
            bgPos.x = targetX;
            bgBtnBottom.anchoredPosition = bgPos;
        }
        else
        {
            moveTween = bgBtnBottom
                .DOAnchorPosX(targetX, duration)
                .SetEase(moveEase);
        }

        // ===== Vị trí target =====
        Vector2 selectedTargetPos = selected == btnHome
            ? homeDefaultPos
            : shopDefaultPos;

        selectedTargetPos.y += selectedY;

        Vector2 unselectedTargetPos = unselected == btnHome
            ? homeDefaultPos
            : shopDefaultPos;

        unselectedTargetPos.y += unselectedY;

        // ===== Instant =====
        if (instant)
        {
            selected.localScale = Vector3.one * selectedScale;
            unselected.localScale = Vector3.one * unselectedScale;

            selected.anchoredPosition = selectedTargetPos;
            unselected.anchoredPosition = unselectedTargetPos;
            return;
        }

        // ===== Selected =====
        Sequence selectedSeq = DOTween.Sequence();
        selectedSeq.Join(
            selected.DOScale(Vector3.one * selectedScale, duration)
                .SetEase(scaleEase));
        selectedSeq.Join(
            selected.DOAnchorPos(selectedTargetPos, duration)
                .SetEase(moveEase));

        // ===== Unselected =====
        Sequence unselectedSeq = DOTween.Sequence();
        unselectedSeq.Join(
            unselected.DOScale(Vector3.one * unselectedScale, duration)
                .SetEase(scaleEase));
        unselectedSeq.Join(
            unselected.DOAnchorPos(unselectedTargetPos, duration)
                .SetEase(moveEase));

        // ===== Lưu tween =====
        if (selected == btnHome)
        {
            homeTween = selectedSeq;
            shopTween = unselectedSeq;
        }
        else
        {
            shopTween = selectedSeq;
            homeTween = unselectedSeq;
        }
    }

    public void ButtonClick()
    {
        Debug.Log("Button Click");
        // SoundManager.Instance.PlayClick();
    }

    private void OnDestroy()
    {
        moveTween?.Kill();
        homeTween?.Kill();
        shopTween?.Kill();
    }

    private void OnApplicationQuit()
    {
        SaveEnergy();

        if (currentEnergy < maxEnergy)
            SaveEnergyStartTime();
    }
}