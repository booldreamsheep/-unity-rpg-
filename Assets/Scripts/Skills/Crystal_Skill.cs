using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystallButton;
    public bool crystalUnlocked {  get; private set; }


    protected override void Start()
    {
        base.Start();

        unlockCrystallButton.GetComponent<Button>().onClick.AddListener(UnlockedCrytal);
    }

    protected override  void CheckUnlock()
    {
        UnlockedCrytal();
    }
    private void UnlockedCrytal()
    {
        if (unlockCrystallButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCystalScript=currentCrystal.GetComponent<Crystal_Skill_Controller>();

            currentCystalScript.SetupCrystal(crystalDuration);
        }
        else
        {
            player.transform.position = currentCrystal.transform.position;
            Destroy(currentCrystal);
        }

    }
}
