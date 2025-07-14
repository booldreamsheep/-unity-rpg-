using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour,IPointerDownHandler
{

    [SerializeField] private Transform craftSlotParent;//�ϳɲ۸�����
    [SerializeField] private GameObject craftSlotPrefab;//�ϳɲ�Ԥ����

    [SerializeField] private List<ItemData_Equament> craftEquipment;
    [SerializeField] private List<UI_Craft> craftSlots;
    void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }


    public void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }


        for (int i = 0; i < craftEquipment.Count; i++)
        {

            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_Craft>().SetupCraftSlot(craftEquipment[i]);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }


    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetCraftWindow(craftEquipment[0]);
    }


}
