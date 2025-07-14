using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("��ҵ���")]//player drop
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()//��Ҳ�������ĺ���
    {
        Inventory inventory = Inventory.instance;

        //������б�
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();//װ������
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();//���ϵ���


        //Ȼ���ж��Ƿ����
        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)//�������
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);

            }
        }


        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.Unequipment(itemsToUnequip[i].data as ItemData_Equament);
        }



        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)//�������
            {
                DropItem(item.data);
                materialsToLoose.Add(item);
            }
        }


        for (int i = 0; i < materialsToLoose.Count; i++)
        {
            inventory.RemoveItem(materialsToLoose[i].data);
        }

    }
}
