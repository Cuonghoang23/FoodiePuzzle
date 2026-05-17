using UnityEngine;
using UnityEngine.EventSystems;

public class FoodSlot : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public FoodItem foodItem;

    [SerializeField] private ItemStove ownerStove;

    private bool isLocked = false;

    public ItemStove OwnerStove => ownerStove;

    private void Awake()
    {
        if (foodItem == null)
            foodItem = GetComponentInChildren<FoodItem>();

        if (ownerStove == null)
            ownerStove = GetComponentInParent<ItemStove>();
    }

    public bool IsEmpty()
    {
        return foodItem == null || foodItem.IsEmpty;
    }

    public void SetLocked(bool value)
    {
        isLocked = value;
    }

    public bool CanReceiveItem()
    {
        return !isLocked && IsEmpty();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked) return;

        GrillDragManager.Instance.ClickSlot(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isLocked) return;

        FoodItem draggedItem = eventData.pointerDrag.GetComponent<FoodItem>();
        if (draggedItem == null) return;

        GrillDragManager.Instance.MoveSprite(draggedItem, this);
    }
}