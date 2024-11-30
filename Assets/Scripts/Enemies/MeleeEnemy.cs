using UnityEngine;

namespace Assets.Scripts.Enemiies
{
    public class MeleeEnemy : EnemyControllerBase
    {
        protected override void Attack()
        {
            Debug.LogWarning("Melee Enemey Attacking State");

            flashOnHit.Flash(false);

            movementController.underAttack = true;

            movementController.navMeshAgent.isStopped = true;

            statsController.enemyData.lastAttackTime = Time.time;
        }

        protected void Update()
        {
            if (movementController.isAttacking && !movementController.underAttack)
            {
                Attack();
            }
        }
    }
}