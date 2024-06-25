using UnityEngine;

public class WarriorClass : PlayerClass
{
    public PlayerClassData warriorData;

    private void Start()
    {
        className = warriorData.className;
        health = warriorData.health;
        mana = warriorData.mana;
        attackPower = warriorData.attackPower;
        defense = warriorData.defense;
    }

    public override void Attack()
    {
        Debug.Log($"{className} swings a sword.");
    }

    public override void SpecialAbility()
    {
        Debug.Log($"{className} uses Shield Bash.");
    }
}
