using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float health;
    public int attackPower;
    public float attackRange;
    public float attackCooldown;
    public float lastAttackTime;
    public float moveSpeed;
    public float stoppingDistance;
}