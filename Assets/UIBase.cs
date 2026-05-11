using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIBase : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] protected float duration = 0.25f;

    [SerializeField] protected Vector3 showScale = Vector3.one;
    [SerializeField] protected Vector3 hideScale = Vector3.zero;

    protected CanvasGroup canvasGroup;
    protected RectTransform rectTransform;

    protected Tween scaleTween;
    protected Tween fadeTween;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // ================= ENABLE =================
    protected virtual void OnEnable()
    {
        PlayShowAnimation();
    }

    // ================= DISABLE =================
    protected virtual void OnDisable()
    {
        KillTween();
    }

    // ================= SHOW =================
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    // ================= HIDE =================
    public virtual void Hide()
    {
        PlayHideAnimation();
    }

    // ================= EXIT =================
    public virtual void Exit()
    {
        Hide();
    }

    // ================= SHOW ANIMATION =================
    protected virtual void PlayShowAnimation()
    {
        KillTween();

        rectTransform.localScale = hideScale;

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        scaleTween = rectTransform
            .DOScale(showScale, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        fadeTween = canvasGroup
            .DOFade(1, duration)
            .SetUpdate(true);
    }

    // ================= HIDE ANIMATION =================
    protected virtual void PlayHideAnimation()
    {
        KillTween();

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        scaleTween = rectTransform
            .DOScale(hideScale, duration)
            .SetEase(Ease.InBack)
            .SetUpdate(true);

        fadeTween = canvasGroup
            .DOFade(0, duration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    // ================= KILL TWEEN =================
    protected virtual void KillTween()
    {
        scaleTween?.Kill();
        fadeTween?.Kill();
    }
}