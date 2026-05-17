using UnityEngine;

public class GrillDragManager : MonoBehaviour
{
    public static GrillDragManager Instance;

    public Transform dragLayer;

    private FoodItem selectedItem;

    private void Awake()
    {
        Instance = this;
    }

    public void ClickItem(FoodItem item)
    {
        if (item == null || item.IsEmpty) return;

        // Nếu đã chọn item khác thì reset item cũ
        if (selectedItem != null && selectedItem != item)
        {
            selectedItem.transform.localScale = Vector3.one;
            selectedItem.SetSelected(false);
        }

        // Nếu click lại chính item đang chọn thì bỏ chọn
        if (selectedItem == item)
        {
            selectedItem.transform.localScale = Vector3.one;
            selectedItem.SetSelected(false);
            selectedItem = null;
            return;
        }

        // Chọn item mới
        selectedItem = item;
        selectedItem.transform.localScale = Vector3.one * 1.15f;
        selectedItem.SetSelected(true);
    }

    public void ClickSlot(FoodSlot targetSlot)
    {
        if (selectedItem == null) return;

        MoveSprite(selectedItem, targetSlot);
    }

    public void MoveSprite(FoodItem fromItem, FoodSlot targetSlot)
    {
        if (fromItem == null || targetSlot == null) return;
        if (fromItem.IsEmpty) return;
        if (!targetSlot.IsEmpty()) return;

        targetSlot.foodItem.SetItem(fromItem.ItemId, fromItem.CurrentSprite);
        fromItem.Clear();

        fromItem.transform.localScale = Vector3.one;
        selectedItem = null;
    }

    public void ClearSelected()
    {
        if (selectedItem != null)
        {
            selectedItem.SetSelected(false);
            selectedItem = null;
        }
    }
}