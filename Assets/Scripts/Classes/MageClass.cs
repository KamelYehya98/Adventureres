using UnityEngine;

public class MageClass : PlayerClass
{
    [SerializeField]
    private PlayerClassData _mageData;

    private new void Start()
    {
        Initialize(_mageData);

        inputActions.Player.Attack.performed += ctx => Attack();
        inputActions.Player.SpecialAbility.performed += ctx => SpecialAbility();
    }

    public override void Attack()
    {
        if (basicAttack != null && _mageData.mana >= basicAttack.manaCost)
        {
            _mageData.mana -= basicAttack.manaCost;
            basicAttack.Activate(gameObject);
            Debug.Log($"{_mageData.className} performs {basicAttack.abilityName}.");
        }
    }

    public override void SpecialAbility()
    {
        if (specialAbility != null && _mageData.mana >= specialAbility.manaCost)
        {
            _mageData.mana -= specialAbility.manaCost;
            specialAbility.Activate(gameObject);
            Debug.Log($"{_mageData.className} performs {specialAbility.abilityName}.");
        }
    }
}