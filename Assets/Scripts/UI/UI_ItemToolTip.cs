using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI itemNameText; // ������ʾ�ı�
    [SerializeField] private TextMeshProUGUI itemTypeText; // ������ʾ�ı�
    [SerializeField] private TextMeshProUGUI itemDescription;// ������ʾ�ı�
    public void ShowToolTIp(ItemData_Equament item)//����ʾ������ʾ,����������ͣ����Ʒ��ʱ����
    {
        if (item == null)
            return;


        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        //�޸������С����ֹ�������
        //if(itemDescription.text.Length > 12)
        //    itemNameText.fontSize = itemNameText.fontSize * .7f;
        //else
        //    itemNameText.fontSize = defaultFontSize;

        gameObject.SetActive(true);
    }
    public void HideToolTip()=>gameObject.SetActive(false);

}
