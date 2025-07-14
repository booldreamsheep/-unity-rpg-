using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;



    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius=.8f;
    private Transform closestEnemy;



    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer <0)
        {
           sr.color = new Color(1,1,1,sr.color.a - colorLoosingSpeed * Time.deltaTime);
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }




    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Player _player,float _attackMutipler)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber",Random.Range(1,3));
        }
        attackMultiplier = _attackMutipler;
        transform.position = _newTransform.position+_offset;
        cloneTimer = _cloneDuration;
        player = _player;




        FaceClosestTarget();
    }



    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            AudioManager.instance.PlaySFX(2, null);//¹¥»÷ÒôÐ§
            if (hit.GetComponent<Enemy>() != null)
            {
                // player.stats.DoDamager(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats=player.GetComponent<PlayerStats>();
                EnemyStats enemyStats =hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemData_Equament weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    if (weaponData != null)
                    {
                        weaponData.Effect(hit.transform);
                    }
                }
            }
        }


    }


    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closetDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                 if(distanceToEnemy < closetDistance)
                {
                    closetDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }

        }
    }


}
