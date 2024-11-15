using Assets.Scripts.Effects.FlashOnHit;
using Assets.Scripts.Enemies;
using Assets.Scripts.Player;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemiies
{
    public abstract class Enemy : MonoBehaviour
    {
        private bool _hasSpottedPlayer;

        public EnemyAnimationManager animationManager;
        public FlashOnHit flashOnHit;
        public LayerMask obstacleMask;

        [SerializeField] protected bool canAttack;
        [SerializeField] protected bool isAttacking;

        protected EnemyData enemyData;
        protected NavMeshAgent navMeshAgent;
        protected Transform targetPlayerTransform;
        protected Vector3 lastPlayerPosition;

        private bool isStunned;
        private float stunDuration;
        protected virtual void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animationManager = GetComponent<EnemyAnimationManager>();
            flashOnHit = GetComponent<FlashOnHit>();

            ApplyNavMeshAgentSettings();

            _hasSpottedPlayer = false;
            canAttack = false;
            isAttacking = false;
            isStunned = false;
            stunDuration = 1.5f;
        }

        protected void FixedUpdate()
        {
            if (navMeshAgent != null && navMeshAgent.velocity != null && animationManager != null)
            {
                Vector2 velocity = navMeshAgent.velocity;
                if (animationManager.rb != null && !isAttacking && !isStunned)
                {
                    animationManager.rb.velocity = new Vector2(velocity.x, velocity.y);
                }
            }
        }

        private void CanAttackCheck()
        {
            if (Time.time >= enemyData.lastAttackTime + enemyData.attackCooldown && !isStunned)
            {
                canAttack = true;
            }
            else
            {
                canAttack = false;
            }
        }

        protected virtual void Update()
        {
            CanAttackCheck();

            FindClosestPlayer();

            if (!isAttacking && !isStunned)
            {
                if (animationManager != null && enemyData.health > 0)
                {
                    animationManager.MovementAnimation();
                }

                if (targetPlayerTransform != null)
                {
                    DrawRayToPlayer();

                    if (IsPlayerInRange())
                    {
                        if (_hasSpottedPlayer || IsPlayerInLineOfSight())
                        {
                            _hasSpottedPlayer = true;
                            MoveTowardsPlayer();
                            lastPlayerPosition = targetPlayerTransform.position;
                        }
                        else if (_hasSpottedPlayer)
                        {
                            MoveTowardsPlayer(); // Keep moving even without line of sight if the player has been spotted
                        }
                    }
                    else if (!IsPlayerInRange() && _hasSpottedPlayer)
                    {
                        if (transform.position == lastPlayerPosition)
                        {
                            navMeshAgent.isStopped = true;
                            _hasSpottedPlayer = false; // Reset spotting if the player is out of range
                        }
                        else
                        {
                            navMeshAgent.isStopped = false;
                            navMeshAgent.SetDestination(lastPlayerPosition);
                            navMeshAgent.stoppingDistance = enemyData.stoppingDistance;
                        }

                    }
                }
            }
        }

        public void KnockBackEffect(Vector2 force)
        {

            animationManager.rb.AddForce(force, ForceMode2D.Impulse);

        }
        public void TakeDamage(float damage)
        {
            if(enemyData.health > 0)
            {
                isStunned = true;

                StopCoroutine(RemoveStun());

                flashOnHit.Flash();

                enemyData.health -= damage;

                if (enemyData.health <= 0)
                {
                    Die();
                }
                else
                {
                    animationManager.TakeDamage();
                }

                StartCoroutine(RemoveStun());
            }
        }

        private IEnumerator RemoveStun()
        {
            yield return new WaitForSeconds(stunDuration);

            isStunned = false;
        }

        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    //if(other.gameObject.GetComponent<WeaponController>() is WeaponController weaponController && weaponController != null)
        //    //{
        //    //    if (weaponController.playerCoreController.animationManager.animator.GetFloat("Weapon.Active") == 1f)
        //    //    {
        //    //        //Attack();
        //    //        Debug.LogWarning("On trigger entered");

        //    //        KnockBackEffect(weaponController.playerCoreController.GetFacingForce() * weaponController.weaponData.attackForceOnOthers[weaponController.currentComboIndex]);
        //    //        TakeDamage(10);
        //    //    }

        //    //}
        //}

        public void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.GetComponentInChildren<TeamComponent>() is TeamComponent teamComponenet)
            {
                if(teamComponenet.teamIndex == TeamIndex.Player)
                {
                    Debug.LogWarning("Player took damage");
                    other.gameObject.GetComponentInChildren<PlayerCoreController>().TakeDamage(10);
                }
            }
        }

        private void Die()
        {
            animationManager.rb.velocity = Vector2.zero;
            animationManager.Die();
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }

        protected void MoveTowardsPlayer()
        {
            if (navMeshAgent == null || !navMeshAgent.isOnNavMesh || enemyData.health <= 0)
            {
                return;
            }

            navMeshAgent.stoppingDistance = enemyData.stoppingDistance;

            if (isStunned)
            {
                navMeshAgent.isStopped = true;
            }
            else if (IsPlayerInRange() && !isStunned)
            {
                navMeshAgent.isStopped = false; // Allow the agent to move
                navMeshAgent.SetDestination(targetPlayerTransform.position);

                if (IsPlayerInLineOfSight() && canAttack)
                {
                    // Only attack if there is a line of sight                    
                    Attack(targetPlayerTransform.GetComponent<PlayerCoreController>());
                }
                else
                {
                    // Allow the enemy to ignore the stopping distance temporarily, until a line of sight is found
                    navMeshAgent.stoppingDistance = 1;
                }
            }
            else
            {
                navMeshAgent.isStopped = false; // Continue moving if the player is out of attack range but in vision range
                navMeshAgent.SetDestination(targetPlayerTransform.position);
            }
        }

        protected abstract void Attack(PlayerCoreController player);

        private void FindClosestPlayer()
        {
            PlayerCoreController[] players = FindObjectsOfType<PlayerCoreController>();
            if (players.Length == 0) return;

            PlayerCoreController closestPlayer = players.OrderBy(p => Vector2.Distance(transform.position, p.transform.position)).FirstOrDefault();
            if (closestPlayer != null)
            {
                targetPlayerTransform = closestPlayer.transform;
            }
        }

        private bool IsPlayerInRange()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayerTransform.position);
            return distanceToPlayer <= enemyData.attackRange;
        }

        private bool IsPlayerInLineOfSight()
        {
            Vector2 directionToPlayer = (targetPlayerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayerTransform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);

            if (hit.collider != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DrawRayToPlayer()
        {
            if (targetPlayerTransform == null)
            {
                return;
            }

            Vector2 directionToPlayer = (targetPlayerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayerTransform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask);

            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, Color.green);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            if (enemyData != null)
            {
                Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
            }
        }

        private void ApplyNavMeshAgentSettings()
        {
            if (navMeshAgent != null && enemyData != null)
            {
                navMeshAgent.updateRotation = false;
                navMeshAgent.updatePosition = true;
                navMeshAgent.updateUpAxis = false;
                navMeshAgent.speed = enemyData.moveSpeed;
                navMeshAgent.acceleration = enemyData.moveSpeed * 4;
                navMeshAgent.angularSpeed = 0;
                navMeshAgent.stoppingDistance = enemyData.stoppingDistance;
            }
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
