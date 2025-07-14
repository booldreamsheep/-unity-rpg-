using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();
        player=GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
       
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if (_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnockbackPower(player.rb.velocity= new Vector2(7,10));
            AudioManager.instance.PlaySFX(31, null);
            Debug.Log("High damage taken");
        }

        ItemData_Equament currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDoDodge();
    }

    public void CloneDoDamage(CharacterStats _TargetStats,float _multiplier)
    {
        if (TargetCanAvoidAttack(_TargetStats))
            return;

        int totalDamager = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0) 
        {
            totalDamager=Mathf.RoundToInt(totalDamager*_multiplier);
        }

        if (CanCrit())
        {
            totalDamager = CalculateCriticalDamage(totalDamager);

        }
        totalDamager = CheckTargetArmor(_TargetStats, totalDamager);
        _TargetStats.TakeDamage(totalDamager);
        DoMagicDamage(_TargetStats);   //如果不想让魔法攻击为主要攻击手段，移除
    }
}
