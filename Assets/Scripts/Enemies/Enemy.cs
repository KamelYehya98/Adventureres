using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float health;
    public int attackPower;
    public float attackRange;
    public float attackCooldown;
    private float lastAttackTime;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerClass player = other.GetComponent<PlayerClass>();
        if (player != null)
        {
            Attack(player);
        }
    }
    
    private void Die()
    {
        Debug.Log($"{enemyName} has died.");
        Destroy(gameObject);
    }

    public void Attack(PlayerClass player)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            player.TakeDamage(attackPower);
        }
    }
}
