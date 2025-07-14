using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTips
{
    [SerializeField] private TextMeshProUGUI skillText;//显示技能描述的文本组件
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowToolTip(string _skillDescprtion, string _skillName,int _price)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescprtion;
        skillCost.text = "Cost" + _price;

        AdjustPosition();
        gameObject.SetActive(true);
    }

    public virtual void HideToolTip() => gameObject.SetActive(false);
}
