using UnityEngine.EventSystems;

public class UI_Craft : UI_Itemslot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemData_Equament _data)
    {
        if (_data == null)
        {
            return;
        }

        item.data = _data;

        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;

        
    }

    private void OnEnable()
    {
        UpdateSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetCraftWindow(item.data as ItemData_Equament);
    }
}
