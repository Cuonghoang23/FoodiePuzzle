using DG.Tweening;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public static UIMainMenu Instance;

    [Header("Buttons")]
    [SerializeField] private RectTransform btnHome;
    [SerializeField] private RectTransform btnShop;
    [SerializeField] private RectTransform bgBtnBottom;

    [SerializeField] private GameObject txtHome;
    [SerializeField] private GameObject txtShop;

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

        // Lưu vị trí gốc
        homeDefaultPos = btnHome.anchoredPosition;
        shopDefaultPos = btnShop.anchoredPosition;
    }

    private void Start()
    {
        // Mặc định chọn Home
        SelectButton(btnHome, btnShop, txtHome, txtShop, true);
    }

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
}