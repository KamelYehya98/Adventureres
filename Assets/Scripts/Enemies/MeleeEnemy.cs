using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Enemiies
{
    public class MeleeEnemy : Enemy
    {
        protected override void Attack(PlayerCoreController player)
        {
            Debug.LogWarning("Melee Enemey Attacking State");

            flashOnHit.Flash(false);

            navMeshAgent.isStopped = true;

            isAttacking = true;

            enemyData.lastAttackTime = Time.time;
        }
    }

}
