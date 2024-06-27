using UnityEngine;

public class MarksmanClass : PlayerClass
{
    [SerializeField]
    private PlayerClassData _marksmanData;

    private new void Start()
    {
        Initialize(_marksmanData);

        inputActions.Player.Attack.performed += ctx => Attack();
        inputActions.Player.SpecialAbility.performed += ctx => SpecialAbility();
    }

    public override void Attack()
    {
        if (basicAttack != null && _marksmanData.mana >= basicAttack.manaCost)
        {
            _marksmanData.mana -= basicAttack.manaCost;
            basicAttack.Activate(gameObject);
            Debug.Log($"{_marksmanData.className} performs {basicAttack.abilityName}.");
        }
    }

    public override void SpecialAbility()
    {
        if (specialAbility != null && _marksmanData.mana >= specialAbility.manaCost)
        {
            _marksmanData.mana -= specialAbility.manaCost;
            specialAbility.Activate(gameObject);
            Debug.Log($"{_marksmanData.className} performs {specialAbility.abilityName}.");
        }
    }
}