using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    private UI ui;
     private Image skillImage;

    [SerializeField] private int skillCost;
    [SerializeField]private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI-" + skillName;
    }
    private void Awake()
    {
        
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage= GetComponent<Image>();
        ui=GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if(unlocked)
        {
            skillImage.color = Color.white;
        }
      
    }
    public void UnlockSkillSlot()
    {
        if(unlocked)
        {
            Debug.Log("Skill already unlocked");
            return;
        }

        if (PlayerManager.instance.HaveEnoughMoney(skillCost)==false)
            { return; }
        

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked==false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }

            
        }
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked==true)
            {
                Debug.Log("cannot unlock skill");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName,skillCost);

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;


        if (mousePosition.x > 200)
            xOffset = -100;
        else
            xOffset = 100;//鼠标靠近屏幕右侧时，提示框向左偏移；否则向右偏移

        if (mousePosition.y > 150)

            yOffset = -70;
        else
            yOffset = 70;//鼠标靠近屏幕顶部时，提示框向下偏移；否则向上偏移


        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);//更新提示框位置为鼠标位置偏移后的点

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
       if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
            //skillImage.color = Color.white;
        }
       
    }

    public void SaveData(ref GameData _data)
    {
       if(_data.skillTree.TryGetValue(skillName, out bool value))
       {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
       }
       else
            _data.skillTree.Add(skillName, unlocked);
    }
}
