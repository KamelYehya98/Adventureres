using Assets.Scripts.Effects.FlashOnHit;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemiies
{
    public abstract class EnemyControllerBase : MonoBehaviour
    {
        public EnemyMovementController movementController;
        public EnemyAnimationManager animationManager;
        public EnemyStatsController statsController;
        public FlashOnHit flashOnHit;
        public LayerMask obstacleMask;

        protected Vector3 lastPlayerPosition;

        protected abstract void Attack();

        public void KnockBackEffect(Vector2 force)
        {
            movementController.rb.AddForce(force, ForceMode2D.Impulse);

            StartCoroutine(RemoveKnockBackEffect());
        }

        public IEnumerator RemoveKnockBackEffect()
        {
            yield return new WaitForSeconds(0.25f);

            movementController.rb.velocity = Vector2.zero;
        }

        public void TakeDamage(float damage)
        {
            if(statsController.enemyData.health > 0)
            {
                movementController.isStunned = true;

                StopCoroutine(RemoveStun());

                flashOnHit.Flash();

                statsController.enemyData.health -= damage;

                if (statsController.enemyData.health <= 0)
                {
                    Die();
                }
                else
                {
                    animationManager.TakeDamageAnimation();
                }

                StartCoroutine(RemoveStun());
            }
        }

        private IEnumerator RemoveStun()
        {
            yield return new WaitForSeconds(movementController.stunDuration);

            movementController.isStunned = false;
            movementController.navMeshAgent.isStopped = false;
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.GetComponentInChildren<TeamComponent>() is TeamComponent teamComponenet)
            {
                if(teamComponenet.teamIndex == TeamIndex.Player)
                {
                    Debug.LogWarning("Player took damage");

                    other.gameObject.GetComponentInChildren<PlayerStatsController>().TakeDamage(10);
                }
            }
        }

        public void Die()
        {
            movementController.isDead = true;

            movementController.rb.velocity = Vector2.zero;
            animationManager.Die();
        }
    }

    public static class GenericEnemyAnimationStates
    {
        public const string IdleUp = "IdleTop";
        public const string IdleDown = "IdleDown";
        public const string IdleRight = "IdleRight";
        public const string WalkUp = "WalkTop";
        public const string WalkDown = "WalkDown";
        public const string WalkRight = "WalkRight";
        public const string DamageUp = "DamageTop";
        public const string DamageDown = "DamageDown";
        public const string DamageRight = "DamageRight";
        public const string Death = "Death";
    }
}