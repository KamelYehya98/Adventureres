using UnityEngine;

public class WarriorClass : PlayerClass
{
    [SerializeField]
    private PlayerClassData _warriorData;

    private new void Start()
    {
        Initialize(_warriorData);

        inputActions.Player.Attack.performed += ctx => Attack();
        inputActions.Player.SpecialAbility.performed += ctx => SpecialAbility();
    }

    public override void Attack()
    {
        if (basicAttack != null && _warriorData.mana >= basicAttack.manaCost)
        {
            _warriorData.mana -= basicAttack.manaCost;
            basicAttack.Activate(gameObject);
            Debug.Log($"{_warriorData.className} performs {basicAttack.abilityName}.");
        }
    }

    public override void SpecialAbility()
    {
        if (specialAbility != null && _warriorData.mana >= specialAbility.manaCost)
        {
            _warriorData.mana -= specialAbility.manaCost;
            specialAbility.Activate(gameObject);
            Debug.Log($"{_warriorData.className} performs {specialAbility.abilityName}.");
        }
    }
}
