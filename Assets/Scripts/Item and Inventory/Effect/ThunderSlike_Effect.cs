using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item Effect/Thunder strike")]
public class ThunderSlike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExcuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab,_enemyPosition.position,Quaternion.identity);

        Destroy(newThunderStrike, 1);
    }
}
