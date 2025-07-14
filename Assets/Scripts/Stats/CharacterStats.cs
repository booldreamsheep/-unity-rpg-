using Cinemachine;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public enum StatType//枚举 StatType 的定义
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stats strength;//物伤
    public Stats agility;//速度，敏捷点
    public Stats intelligence;//法伤
    public Stats vitality;//生命加成

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance;//暴击率
    public Stats critPower;//暴击伤害

    [Header("Defensive stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;


    public bool isIgnited;//持续伤害
    public bool isChilled;//减防20%
    public bool isShocked;//减伤20%

    [SerializeField] private float alimentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int ignitedDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;


    public int currentHealth;

    public System.Action onHealthChanged;
    public  bool isDead {  get; private set; }
    public bool isInvincible { get; private set; }
    private bool isVuInerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx= GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }
        if(chilledTimer < 0)
        {
            isChilled = false;
        }
        if(shockedTimer < 0)
        {
            isShocked = false;
        }
        if (igniteDamageTimer < 0&&isIgnited)
        {
            
            DecreaseHealthBy(ignitedDamage);
            if (currentHealth < 0&&!isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCorutine(_duration));
    }
    private IEnumerator VulnerableForCorutine(float _duration)
    {
        isVuInerable = true;

        yield return new WaitForSeconds(_duration);

        isVuInerable=false;

    }
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _statToModify)
    {
        StartCoroutine(StatModCoruntine(_modifier, _duration, _statToModify));
    }


    private IEnumerator StatModCoruntine(int _modifier, float _duration, Stats _statToModify)//加buff的协程
    {
        _statToModify.AddModiFier(_modifier);//添加一个buff

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);//移除一个buff
    }

    public virtual void DoDamager(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);


        int totalDamager = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
           totalDamager=CalculateCriticalDamage(totalDamager);
            criticalStrike = true;
        }

        fx.CreateHitFx(_targetStats.transform,criticalStrike);

        totalDamager = CheckTargetArmor(_targetStats, totalDamager);
         _targetStats.TakeDamage(totalDamager);
        DoMagicDamage(_targetStats);   //如果不想让魔法攻击为主要攻击手段，移除
    }
    #region Magical damage and ailemnts
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _firDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamge = _firDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamge = CheckTargetResistance(_targetStats, totalMagicDamge);

        _targetStats.TakeDamage(totalMagicDamge);


        if (Mathf.Max(_firDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        AttemptyToApplyAliement(_targetStats, _firDamage, _iceDamage, _lightingDamage);

    }

    private  void AttemptyToApplyAliement(CharacterStats _targetStats, int _firDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _firDamage > _iceDamage && _firDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _firDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _firDamage && _lightingDamage > _iceDamage;



        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .25f && _firDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyShock, canApplyChill);
                Debug.Log("fire");
                return;
            }

            if (Random.value < .33f && _iceDamage > 0)
            {
                canApplyChill = true;

                _targetStats.ApplyAilments(canApplyIgnite, canApplyShock, canApplyChill);
                Debug.Log("ice");
                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyShock, canApplyChill);
                Debug.Log("light");
                return;
            }
        }


        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_firDamage * .2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyShock, canApplyChill);
    }

    private  int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamge)
    {
        totalMagicDamge -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamge = Mathf.Clamp(totalMagicDamge, 0, int.MaxValue);
        return totalMagicDamge;
    }

    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
       bool canApplyIgnite = !isIgnited&&!isChilled&&!isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;


        if (_ignite&&canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = alimentsDuration;

            fx.IgniteFxFor(alimentsDuration);
        }

        if (_chill&&canApplyChill)
        {
            chilledTimer = alimentsDuration;
          isChilled = _chill;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, alimentsDuration);
            fx.ChillFxFor(alimentsDuration);
        }
        if (_shock&&canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            if (isShocked)
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitTargetWithNearestShock();

            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        shockedTimer = alimentsDuration;
        isShocked = _shock;

        fx.ShockFxFor(alimentsDuration);
    }

    private void HitTargetWithNearestShock()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderContorller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage)=>ignitedDamage= _damage;
    public void SetupShockStrikeDamage(int _damage)=>shockDamage= _damage;
    #endregion
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamager)
    {
        if (_targetStats.isChilled)
        {
            totalDamager-=Mathf.RoundToInt(_targetStats.armor.GetValue()*.8f);
        }
        totalDamager -= _targetStats.armor.GetValue();
        totalDamager = Mathf.Clamp(totalDamager, 0, int.MaxValue);
        return totalDamager;
    }
    public virtual void OnEvasion()
    {

    }
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;


        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0&&!isDead)
        {
            Die();
        }
    }


    public virtual void IncreaseHealthBy(int _amout)
    {      
        currentHealth += _amout;
        if(currentHealth>GetMaxHealthValue())
        {
            currentHealth=GetMaxHealthValue();
        }
        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVuInerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
    }


    public void KillEntity()
    {
        if(!isDead)
        {
            Die();
        }
    }


    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;


    protected bool CanCrit()
    {
        int totalCriticalChance=critChance.GetValue()+agility.GetValue();
        if(Random.Range(0,100)<=totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = ((critPower.GetValue() + strength.GetValue()) * .01f);

        float critDamage=_damage*totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }


   public Stats GetStat(StatType _StatType)//根据 buffType 返回需要修改的属性。
    {
        if (_StatType == StatType.strength) return strength;
        else if (_StatType == StatType.agility) return agility;
        else if (_StatType == StatType.intelligence) return intelligence;
        else if (_StatType == StatType.vitality) return vitality;
        else if (_StatType == StatType.damage) return damage;
        else if (_StatType == StatType.critChance) return critChance;
        else if (_StatType == StatType.critPower) return critPower;
        else if (_StatType == StatType.maxHealth) return maxHealth;
        else if (_StatType == StatType.armor) return armor;
        else if (_StatType == StatType.evasion) return evasion;
        else if (_StatType == StatType.magicResistance) return magicResistance;
        else if (_StatType == StatType.fireDamage) return fireDamage;
        else if (_StatType == StatType.iceDamage) return iceDamage;
        else if (_StatType == StatType.lightingDamage) return lightingDamage;

        else return null;
    }

}
