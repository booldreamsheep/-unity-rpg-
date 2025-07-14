using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image icon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;


    public void SetCraftWindow(ItemData_Equament _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftinMaterials.Count; i++)
        {
            if (_data.craftinMaterials.Count > materialImage.Length)
                Debug.LogWarning("��ӵ�еĲ��ϱȺϳ���Ҫ�Ĳ��϶�");

            materialImage[i].sprite = _data.craftinMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().text = _data.craftinMaterials[i].stackSize.ToString();
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        }

        icon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data, _data.craftinMaterials));
    }

}
