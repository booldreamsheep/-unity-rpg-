using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,//武器
    Armor,//护甲
    Amulet,//护符
    Flask//药瓶
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equament")]
public class ItemData_Equament : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float itemCooldown;
    public ItemEffect[] itemEffect;
  

    [Header("Major stats")]
    public int strength;//物伤
    public int agility;//速度，敏捷点
    public int intelligence;//法伤
    public int vitality;//生命加成

    [Header("Offensive stats")]
    public int damage;
    public int critChance;//暴击率
    public int critPower;//暴击伤害

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;


    [Header("创造面板")]
     public List<InventoryItem> craftinMaterials;

    private int minDescriptionLength;

    public void Effect(Transform _enemyPosition)//执行物品效果
    {
        foreach (var item in itemEffect)
        {
            item.ExcuteEffect(_enemyPosition);
        }
    }
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModiFier(strength);
        playerStats.agility.AddModiFier(agility);
        playerStats.intelligence.AddModiFier(intelligence);
        playerStats.vitality.AddModiFier(vitality);

        playerStats.damage.AddModiFier(damage);
        playerStats.critChance.AddModiFier(critChance);
        playerStats.critPower.AddModiFier(critPower);

        playerStats.maxHealth.AddModiFier(maxHealth);
        playerStats.armor.AddModiFier(armor);
        playerStats.evasion.AddModiFier(evasion);
        playerStats.magicResistance.AddModiFier(magicResistance);

        playerStats.fireDamage.AddModiFier(fireDamage);
        playerStats.iceDamage.AddModiFier(iceDamage);
        playerStats.lightingDamage.AddModiFier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }


    public override string GetDescription()
    {

        sb.Length = 0;//确保每次调用时是从头开始
        minDescriptionLength = 0;//重置描述的属性数量

        AddTtemDescription(strength, "strength");
        AddTtemDescription(agility, "agility");
        AddTtemDescription(intelligence, "intenlli");
        AddTtemDescription(vitality, "vitality");

        AddTtemDescription(damage, "damage");
        AddTtemDescription(critChance, "cirtchance");
        AddTtemDescription(critPower, "critPower");

        AddTtemDescription(maxHealth, "Health");
        AddTtemDescription(armor, "Armor");
        AddTtemDescription(evasion, "Evasion");
        AddTtemDescription(magicResistance, "MagicRes");

        AddTtemDescription(fireDamage, "fire");
        AddTtemDescription(iceDamage, "ice");
        AddTtemDescription(lightingDamage, "light");


        for (int i = 0; i < itemEffect.Length; i++)
        {
            if (itemEffect[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.Append("Unique:"+itemEffect[i].effectDescription);
                minDescriptionLength++;
            }
        }


        if (minDescriptionLength < 5)
        {
            for (int i = 0; i < 5-minDescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }


     

        return sb.ToString();
    }

    private void AddTtemDescription(int _value,string _name)
    {
        if(_value!=0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }
            if (_value > 0)
                sb.Append("+"+_value+""+_name);

            minDescriptionLength++;
        }
    }
}
