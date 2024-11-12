using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Enemiies
{
    public class MeleeEnemy : Enemy
    {
        protected override void Attack(PlayerController player)
        {
            Debug.LogWarning("Melee Enemey Attacking State");

            navMeshAgent.isStopped = true;

            isAttacking = true;

            enemyData.lastAttackTime = Time.time;
        }
    }

}
