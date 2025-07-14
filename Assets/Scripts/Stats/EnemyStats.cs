using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stats soulsDropAmount;

    [Header("µÈ¼¶")]
    [SerializeField] private int level=1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier=.4f;
    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem= GetComponent<ItemDrop>();

        

    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

       
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }

    private void Modify(Stats _stats)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier=_stats.GetValue()*percantageModifier;

            _stats.AddModiFier(Mathf.RoundToInt(modifier));
        }
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
      
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();


        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        myDropSystem.GenerateDrop();


        Destroy(gameObject, 5f);
    }
}
