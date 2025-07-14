using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnlockled { get; private set; }
    [SerializeField] private float maxSize;//最大尺寸
    [SerializeField] private float growSpeed;//变大速度
    [SerializeField] private float shrinkSpeed;//缩小速度
    [SerializeField] int amountOfAttacks;
    [SerializeField] float cloneCooldown;
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float blackholeDuration;

    Blackhold_Skill_Controller currentBlackhole;



    private void UnlockBlackhole()
    {
        if (blackHoleUnlockButton.unlocked)
        {
            blackHoleUnlockled = true;
        }
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackhole = Instantiate(blackholePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Blackhold_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown,blackholeDuration);
        

        AudioManager.instance.PlaySFX(3, player.transform);
        AudioManager.instance.PlaySFX(6, player.transform);
    }

    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
        {
            return false;
        }

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockBlackhole();
    }
}
