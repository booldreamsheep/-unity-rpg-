using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry pro")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    public bool restoreUnlocked {  get; private set; }
    [Range(0f, 1f)]
    [SerializeField] private float retoreHealthPerentage;

    [Header("Parry ultra")]
    [SerializeField] private UI_SkillTreeSlot parryWithMiragelockButton;
    public bool parryWithMiragelock {  get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * retoreHealthPerentage);
            player.stats.IncreaseHealthBy(restoreAmount);

        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestor);
        parryWithMiragelockButton.GetComponent<Button>().onClick.AddListener(UnlockParrtWithMirage);
    }
    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestor();
        UnlockParrtWithMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestor()
    {
        if (restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    private void UnlockParrtWithMirage()
    {
        if (parryWithMiragelockButton.unlocked)
            parryWithMiragelock = true;
    }


}
