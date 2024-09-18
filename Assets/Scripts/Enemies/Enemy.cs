using Assets.Scripts.Classes;
using Assets.Scripts.ScriptableObjects;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemiies
{
    public abstract class Enemy : MonoBehaviour
    {
        public EnemyData enemyData;
        protected NavMeshAgent navMeshAgent;
        protected Transform targetPlayerTransform;
        protected Vector3 lastPlayerPosition;

        public LayerMask obstacleMask;

        private bool _hasSpottedPlayer = false;

        protected virtual void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            ApplyNavMeshAgentSettings();
        }

        protected virtual void Start()
        {
            enemyData.lastAttackTime = 0;
        }

        protected virtual void Update()
        {
            FindClosestPlayer();
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
                else if(!IsPlayerInRange() && _hasSpottedPlayer)
                {
                    if(transform.position == lastPlayerPosition)
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

        public void TakeDamage(float damage)
        {
            enemyData.health -= damage;
            if (enemyData.health <= 0)
            {
                Die();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                Attack(player);
            }
        }

        private void Die()
        {
            //Debug.Log($"{enemyData.enemyName} has died.");
            Destroy(gameObject);
        }

        protected void MoveTowardsPlayer()
        {
            if (navMeshAgent == null || !navMeshAgent.isOnNavMesh)
            {
                //Debug.LogWarning("NavMeshAgent is not on the NavMesh.");
                return;
            }

            float distanceToPlayer = Vector2.Distance(targetPlayerTransform.position, transform.position);

            navMeshAgent.stoppingDistance = enemyData.stoppingDistance;

            if (distanceToPlayer <= enemyData.attackRange)
            {
                navMeshAgent.isStopped = false; // Allow the agent to move
                navMeshAgent.SetDestination(targetPlayerTransform.position);

                if (IsPlayerInLineOfSight())
                {
                    // Only attack if there is a line of sight
                    Attack(targetPlayerTransform.GetComponent<PlayerController>());
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

        protected abstract void Attack(PlayerController player);

        private void FindClosestPlayer()
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            if (players.Length == 0) return;

            PlayerController closestPlayer = players.OrderBy(p => Vector2.Distance(transform.position, p.transform.position)).FirstOrDefault();
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
                ////Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");
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
            Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
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
}
