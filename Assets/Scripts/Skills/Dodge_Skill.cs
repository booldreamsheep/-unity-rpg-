using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockedDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Dodge pro")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();
        unlockedDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);

    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockDodgeMirage();
    }

    private void UnlockDodge()
    {
        if (unlockedDodgeButton.unlocked&&!dodgeUnlocked)
        {
            player.stats.evasion.AddModiFier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockDodgeMirage()
    {
        if(unlockMirageDodgeButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void CreateMirageOnDoDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform,new Vector3(2*player.facingDir,0));
        }
    }
}
