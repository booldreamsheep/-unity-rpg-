using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice And Fire effect", menuName = "Data/Item Effect/Ice And Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;//������Ԥ����
    [SerializeField] private float xVelocity;//�����ܵ�ˮƽ�ٶ�

    public override void ExcuteEffect(Transform _respawnPositon)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttack.comboCountter== 2;//�����ι��������ͷű�����

        if (thirdAttack)
        {

            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPositon.position, player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);//�޸ĳ���

            Destroy(newIceAndFire, 10);
        }
    }

}
