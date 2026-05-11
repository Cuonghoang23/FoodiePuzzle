using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Quản lý hiển thị Coin UI.
/// Dữ liệu Coin được lưu trong GameData.Instance.Coin.
/// </summary>
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("UI Text Coin")]
    [SerializeField] private List<TextMeshProUGUI> coinTexts = new();

    [Header("Animation")]
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.25f;
    [SerializeField] private Ease bounceEase = Ease.OutBack;

    private readonly Dictionary<TextMeshProUGUI, Tween> tweenDict = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        RefreshUI(false);
    }

    private void OnDestroy()
    {
        foreach (var tween in tweenDict.Values)
        {
            tween?.Kill();
        }

        tweenDict.Clear();
    }


    public int Coin
    {
        get
        {
            if (GameData.Instance == null)
                return 0;

            return GameData.Instance.Coin;
        }
        private set
        {
            if (GameData.Instance != null)
            {
                GameData.Instance.Coin = Mathf.Max(0, value);
            }
        }
    }

    public void AddCoin(int amount)
    {
        if (amount <= 0)
            return;

        Coin += amount;
        RefreshUI();
    }

    public bool SpendCoin(int amount)
    {
        if (amount <= 0)
            return false;

        if (Coin < amount)
            return false;

        Coin -= amount;
        RefreshUI();
        return true;
    }

    public bool HasEnough(int amount)
    {
        return Coin >= amount;
    }

    public void SetCoin(int value)
    {
        Coin = value;
        RefreshUI();
    }

    public void RefreshUI()
    {
        RefreshUI(true);
    }

    public void RegisterText(TextMeshProUGUI text)
    {
        if (text == null)
            return;

        if (!coinTexts.Contains(text))
        {
            coinTexts.Add(text);
        }

        UpdateText(text, false);
    }

    public void UnregisterText(TextMeshProUGUI text)
    {
        if (text == null)
            return;

        coinTexts.Remove(text);

        if (tweenDict.TryGetValue(text, out Tween tween))
        {
            tween?.Kill();
            tweenDict.Remove(text);
        }
    }


    private void RefreshUI(bool playBounce)
    {
        foreach (var txt in coinTexts)
        {
            UpdateText(txt, playBounce);
        }
    }

    private void UpdateText(TextMeshProUGUI txt, bool playBounce)
    {
        if (txt == null)
            return;

        txt.text = FormatCoin(Coin);

        if (playBounce)
        {
            PlayBounce(txt);
        }
    }

    private void PlayBounce(TextMeshProUGUI txt)
    {
        RectTransform rect = txt.rectTransform;

        if (tweenDict.TryGetValue(txt, out Tween oldTween))
        {
            oldTween?.Kill();
        }

        rect.localScale = Vector3.one;

        Tween tween = rect
            .DOScale(Vector3.one * bounceScale, bounceDuration * 0.5f)
            .SetEase(bounceEase)
            .SetLoops(2, LoopType.Yoyo);

        tweenDict[txt] = tween;
    }

    private string FormatCoin(int value)
    {
        return value.ToString("N0");
    }


    //test


    [Header("Test")]
    [SerializeField] private int testAmount = 100;

    [ContextMenu("SpendCoin")]
    public void TestSpendCoin()
    {
            SpendCoin(testAmount);
    }

    // Set coin
    [ContextMenu("AddCoin")]
    public void TestSetCoin()
    {
        AddCoin(testAmount);
    }
}