using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image imgFood;

    public int ItemId { get; private set; } = -1;
    public Sprite CurrentSprite => imgFood.sprite;
    public bool IsEmpty => imgFood.sprite == null;

    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GameObject dragGhost;
    private UIOutlineEffect outline;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        if (imgFood == null)
            imgFood = GetComponent<Image>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        outline = GetComponent<UIOutlineEffect>();
    }

    public void SetItem(int itemId, Sprite sprite)
    {
        ItemId = itemId;

        imgFood.sprite = sprite;

        bool hasSprite = sprite != null;
        imgFood.enabled = hasSprite;
        imgFood.raycastTarget = hasSprite;
    }

    public void Clear()
    {
        SetItem(-1, null);
    }

    public void SetSelected(bool value)
    {
        if (outline != null)
            outline.SetOutline(value);

        transform.localScale = value ? Vector3.one * 1.15f : Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty) return;

        GrillDragManager.Instance.ClickItem(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty) return;

        GrillDragManager.Instance.ClearSelected();

        SetSelected(true);

        dragGhost = new GameObject("DragGhost");
        dragGhost.transform.SetParent(GrillDragManager.Instance.dragLayer, false);

        Image ghostImage = dragGhost.AddComponent<Image>();
        ghostImage.sprite = imgFood.sprite;
        ghostImage.material = imgFood.material;
        ghostImage.raycastTarget = false;

        RectTransform ghostRect = dragGhost.GetComponent<RectTransform>();
        RectTransform myRect = GetComponent<RectTransform>();

        ghostRect.sizeDelta = myRect.sizeDelta;
        ghostRect.position = myRect.position;

        imgFood.enabled = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragGhost == null) return;

        RectTransform ghostRect = dragGhost.GetComponent<RectTransform>();
        ghostRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (dragGhost != null)
            Destroy(dragGhost);

        if (imgFood.sprite != null)
            imgFood.enabled = true;

        SetSelected(false);
    }
}