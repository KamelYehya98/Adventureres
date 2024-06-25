using UnityEngine;

public class MageClass : PlayerClass
{
    public PlayerClassData mageData;
    public Animator animator;
    public Ability basicAttack;
    public Ability specialAbility;

    private void Start()
    {
        Initialize(mageData);
    }

    public override void Attack()
    {
        if (basicAttack != null && mana >= basicAttack.manaCost)
        {
            animator.SetTrigger("Attack");
            mana -= basicAttack.manaCost;
            basicAttack.Activate(gameObject);
            Debug.Log($"{className} performs {basicAttack.abilityName}.");
        }
    }

    public override void SpecialAbility()
    {
        if (specialAbility != null && mana >= specialAbility.manaCost)
        {
            animator.SetTrigger("SpecialAbility");
            mana -= specialAbility.manaCost;
            specialAbility.Activate(gameObject);
            Debug.Log($"{className} performs {specialAbility.abilityName}.");
        }
    }
}