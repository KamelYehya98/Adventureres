using UnityEngine;

public abstract class PlayerClass : MonoBehaviour
{
    public string className;
    public int health;
    public int mana;
    public int attackPower;
    public int defense;

    public Ability basicAttack;
    public Ability specialAbility;

    public void Initialize(PlayerClassData data)
    {
        className = data.className;
        health = data.health;
        mana = data.mana;
        attackPower = data.attackPower;
        defense = data.defense;
        basicAttack = data.basicAttack;
        specialAbility = data.specialAbility;
    }

    public abstract void Attack();
    public abstract void SpecialAbility();

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{className} has died.");
    }
}
