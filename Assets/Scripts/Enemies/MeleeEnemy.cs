using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Enemiies
{
    public class MeleeEnemy : Enemy
    {
        protected override void Attack(PlayerController player)
        {
            if (Time.time >= enemyData.lastAttackTime + enemyData.attackCooldown)
            {
                enemyData.lastAttackTime = Time.time;
                player.TakeDamage(enemyData.attackPower);
                ////Debug.Log($"{enemyData.enemyName} attacks the player with melee attack.");
            }
            //else
            //{
            //    //Debug.Log("Melee not attacking");
            //}
        }
    }

}
