using Assets.Scripts.Enemiies;
using Assets.Scripts.Player;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemies
{
    public class EnemyMovementController : MonoBehaviour
    {
        [SerializeField]
        private EnemyControllerBase controllerBase;

        public Rigidbody2D rb;
        public NavMeshAgent navMeshAgent;
        public Transform targetPlayerTransform;
        public LayerMask obstacleMask;

        [SerializeField]
        private bool _canMove;

        private bool _canAttack;

        private bool _hasSpottedPlayer;
        private Vector3 _lastPlayerPosition;

        public float stunDuration;
        public bool isStunned;
        public bool isAttacking;
        public bool underAttack;
        public bool isDead;


        public void Awake()
        {
            _canMove = true;
            _canAttack = false;
            _hasSpottedPlayer = false;
            
            isAttacking = false;
            isStunned = false;
            isDead = false;
            underAttack = false;

            stunDuration = 1.5f;

            _lastPlayerPosition = Vector3.zero;
        }

        public void Start()
        {
            ApplyNavMeshAgentSettings();
        }

        public void FixedUpdate()
        {
            if(!isDead)
            {
                StopMovementWhenStunned();
                FindClosestPlayer();

                if (_canMove)
                {
                    SetRBVelcoityToNavVelocity();
                    MovementLogicCheck();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
                navMeshAgent.velocity = Vector2.zero;
            }
        }

        public void Update()
        {
            CanMoveCheck();
            CanAttackCheck();
        }

        private void CanMoveCheck()
        {
            if (!isAttacking && !isStunned && !isDead)
            {
                SetCanMove(true);
            }
            else
            {
                SetCanMove(false);
            }
        }

        public bool CanAttack()
        {
            return _canAttack;
        }

        private void ApplyNavMeshAgentSettings()
        {
            if (navMeshAgent != null && controllerBase.statsController.enemyData != null)
            {
                navMeshAgent.updateRotation = false;
                navMeshAgent.updatePosition = true;
                navMeshAgent.updateUpAxis = false;
                navMeshAgent.speed = controllerBase.statsController.enemyData.moveSpeed;
                navMeshAgent.acceleration = controllerBase.statsController.enemyData.moveSpeed * 4;
                navMeshAgent.angularSpeed = 0;
                navMeshAgent.stoppingDistance = controllerBase.statsController.enemyData.stoppingDistance;
            }
        }

        private void StopMovementWhenStunned()
        {
            if (isStunned)
            {
                navMeshAgent.isStopped = true;
            }
        }

        private void CheckAttackPerformance()
        {
            if (IsPlayerInAttackRange())
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetPlayerTransform.position);

                if (IsPlayerInLineOfSight() && _canAttack)
                {
                    isAttacking = true;
                }
                else
                {
                    navMeshAgent.stoppingDistance = 1;
                }
            }
        }

        public void EndAttack()
        {
            isAttacking = false;
            underAttack = false;
        }

        private void SetRBVelcoityToNavVelocity()
        {
            if (navMeshAgent != null && navMeshAgent.velocity != null)
            {
                Vector2 velocity = navMeshAgent.velocity;

                rb.velocity = new Vector2(velocity.x, velocity.y);
            }
        }

        private void CanAttackCheck()
        {
            if (Time.time >= controllerBase.statsController.enemyData.lastAttackTime + controllerBase.statsController.enemyData.attackCooldown && !isStunned && !isDead)
            {
                _canAttack = true;
            }
            else
            {
                _canAttack = false;
            }
        }

        private void MovementLogicCheck()
        {
            if (targetPlayerTransform != null)
            {
                DrawRayToPlayer();

                if (IsPlayerInDetectionRange())
                {
                    CheckAttackPerformance();

                    if (!_hasSpottedPlayer || IsPlayerInLineOfSight())
                    {
                        _hasSpottedPlayer = true;
                        _lastPlayerPosition = targetPlayerTransform.position;

                        MoveTowardsPlayer();
                    }
                    else if (_hasSpottedPlayer)
                    {
                        MoveTowardsPlayer();
                    }
                }
                else if (!IsPlayerInDetectionRange() && _hasSpottedPlayer)
                {
                    if (transform.position == _lastPlayerPosition)
                    {
                        navMeshAgent.isStopped = true;
                        _hasSpottedPlayer = false;
                    }
                    else
                    {
                        navMeshAgent.isStopped = false;
                        navMeshAgent.SetDestination(_lastPlayerPosition);
                        navMeshAgent.stoppingDistance = controllerBase.statsController.enemyData.stoppingDistance;
                    }

                }
            }
        }

        private void MoveTowardsPlayer()
        {
            if (navMeshAgent == null || !navMeshAgent.isOnNavMesh || controllerBase.statsController.enemyData.health <= 0)
            {
                return;
            }

            navMeshAgent.stoppingDistance = controllerBase.statsController.enemyData.stoppingDistance;

            if (!IsPlayerInAttackRange())
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetPlayerTransform.position);
            }
        }

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

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
        }

        public bool GetCanMove()
        {
            return _canMove; 
        }

        public bool IsPlayerInAttackRange()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayerTransform.position);
            return distanceToPlayer <= controllerBase.statsController.enemyData.attackRange;
        }

        private bool IsPlayerInDetectionRange()
        {
            float distanceToPlayer = GetDistanceToPlayer();
            return distanceToPlayer <= controllerBase.statsController.enemyData.detectionRange;
        }

        public bool IsPlayerInLineOfSight()
        {
            Vector2 directionToPlayer = GetDirectionToPlayer();
            float distanceToPlayer = GetDistanceToPlayer();

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

        private float GetDistanceToPlayer()
        {
            return Vector2.Distance(transform.position, targetPlayerTransform.position);
        }


        public Vector2 GetDirectionToPlayer()
        {
            return ((Vector2)targetPlayerTransform.position - (Vector2)transform.position).normalized;
        }

        private void DrawRayToPlayer()
        {
            if (targetPlayerTransform == null)
            {
                return;
            }

            Vector2 directionToPlayer = GetDirectionToPlayer();
            float distanceToPlayer = GetDistanceToPlayer();

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

            if (controllerBase.statsController.enemyData != null)
            {
                Gizmos.DrawWireSphere(transform.position, controllerBase.statsController.enemyData.detectionRange);
            }

            //Gizmos.color = Color.red;

            //if (controllerBase.statsController.enemyData != null)
            //{
            //    Gizmos.DrawWireSphere(transform.position, controllerBase.statsController.enemyData.attackRange);
            //}

        }
    }
}