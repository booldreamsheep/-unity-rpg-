using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Details")]
    [SerializeField]private float attackMuittiper;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [Header("Clone attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggresive clone")]
    [SerializeField] private UI_SkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField]private float aggresiveCloneAttackMultiplier;
    [SerializeField] public bool canApplyOnHitEffect;

    //[SerializeField] private bool creatCloneOnDashStart;
    //[SerializeField] private bool creatCloneOnDashEnd;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().
            SetupClone(_clonePosition, cloneDuration, canAttack, _offset, player,attackMuittiper);
    }

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAgssresiveClone);
    }

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAgssresiveClone();
        
    }

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked) 
        {

            canAttack = true;
            attackMuittiper = cloneAttackMultiplier;
        }
    }

    private void UnlockAgssresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect= true;
            attackMuittiper = aggresiveCloneAttackMultiplier;
        }
    }
}
