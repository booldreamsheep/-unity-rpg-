using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Itemslot : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    protected virtual void Start()
    {
        ui=GetComponentInParent<UI>();
    }
    public void UpdateSlot(InventoryItem _newitem)
    {
        item = _newitem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }

        }
    }


    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item==null) return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
   
            if (item.data.itemType == ItemType.Equipment)
            {
            Inventory.instance.EquipItem(item.data);
            }     

            ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item==null) return;


        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;


        if (mousePosition.x > 200)
            xOffset = -75;
        else
            xOffset = 75;//鼠标靠近屏幕右侧时，提示框向左偏移；否则向右偏移

        if (mousePosition.y > 150)

            yOffset = -50;
        else
            yOffset = 50;//鼠标靠近屏幕顶部时，提示框向下偏移；否则向上偏移

        ui.itemToolTip.ShowToolTIp(item.data as ItemData_Equament);
        ui.itemToolTip.transform.position =new Vector2(mousePosition.x+xOffset, mousePosition.y+yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item==null) return;
        ui.itemToolTip.HideToolTip();
    }
}
