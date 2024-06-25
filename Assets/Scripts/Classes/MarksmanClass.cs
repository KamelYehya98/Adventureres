using UnityEngine;

public class MarksmanClass : PlayerClass
{
    public PlayerClassData marksmanData;

    private void Start()
    {
        className = marksmanData.className;
        health = marksmanData.health;
        mana = marksmanData.mana;
        attackPower = marksmanData.attackPower;
        defense = marksmanData.defense;
    }

    public override void Attack()
    {
        Debug.Log($"{className} shoots an arrow.");
    }

    public override void SpecialAbility()
    {
        Debug.Log($"{className} uses Rapid Fire.");
    }
}