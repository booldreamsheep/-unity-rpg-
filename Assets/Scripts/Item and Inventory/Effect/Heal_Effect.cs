using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExcuteEffect(Transform _enemyPositon)
    {
        //������״̬��
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //���Ƶ�Ѫ���Ƕ���
        int healAmont = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        //�������Ч��
        playerStats.IncreaseHealthBy(healAmont);

    }
}
