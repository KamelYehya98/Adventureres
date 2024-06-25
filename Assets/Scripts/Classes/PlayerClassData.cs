using UnityEngine;

[CreateAssetMenu(fileName = "PlayerClassData", menuName = "RPG/PlayerClassData")]
public class PlayerClassData : ScriptableObject
{
    public string className;
    public int health;
    public int mana;
    public int attackPower;
    public int defense;

    public Ability basicAttack;
    public Ability specialAbility;
}
