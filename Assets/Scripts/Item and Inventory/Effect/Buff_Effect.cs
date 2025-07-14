using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff effect")]

public class Buff_Effect :ItemEffect
{
        private PlayerStats stats;
        [SerializeField] private StatType buffType;// Buff ������
        [SerializeField] private int buffAmount; // Buff ��������
        [SerializeField] private float buffDuration;// Buff ����ʱ��


        public override void ExcuteEffect(Transform _enemyPositon)
        {
            stats = PlayerManager.instance.player.GetComponent<PlayerStats>();//��ȡ��ǰ��ҽ�ɫ�� PlayerStats ������������ʺ��޸Ľ�ɫ�ĸ������ԡ�
            stats.IncreaseStatBy(buffAmount, buffDuration,stats.GetStat(buffType));
        }




    
}
