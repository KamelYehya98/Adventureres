using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Enemiies
{
    public class RangedEnemy : Enemy
    {
        public GameObject projectilePrefab;
        private Transform _firePoint;

        protected override void Attack(PlayerClass player)
        {
            if (Time.time >= enemyData.lastAttackTime + enemyData.attackCooldown)
            {
                enemyData.lastAttackTime = Time.time;
                ShootProjectile(player);
                ////Debug.Log($"{enemyData.enemyName} attacks the player with ranged attack.");
            }
        }

        private void ShootProjectile(PlayerClass player)
        {
            _firePoint = GetComponent<Transform>();

            GameObject projectile = Instantiate(projectilePrefab, _firePoint.position, _firePoint.rotation);
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.Initialize(player.transform);
        }
    }

}
