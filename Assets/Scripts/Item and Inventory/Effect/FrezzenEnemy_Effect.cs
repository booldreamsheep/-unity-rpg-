using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FreezeEnemies Effect", menuName = "Data/Item Effect/Freeze Enemies")]

public class FrezzenEnemy_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExcuteEffect(Transform _transform)
    {
        PlayerStats playerStats =PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
        {
            return;
        }

        if (!Inventory.instance.CanUseArmor())
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position,2);
       foreach(var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
