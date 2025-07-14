using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_Itemslot
{
    public EquipmentType slotType;
    private void OnValidate()
    {
        gameObject.name="Equipment slot-"+slotType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
        {
            return;
        }

        Inventory.instance.Unequipment(item.data as ItemData_Equament);
        Inventory.instance.AddItem(item.data as ItemData_Equament);

        ui.itemToolTip.HideToolTip();

        CleanUpSlot();
    }
}
