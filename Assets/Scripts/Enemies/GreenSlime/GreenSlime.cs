using Assets.Scripts.Classes;
using Assets.Scripts.Enemiies;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemies.GreenSlime
{
    public class GreenSlime : MeleeEnemy
    {
        public float chargeDuration = 1f;
        public float jumpForce = 10f;

        private bool isCharging = false;
        private Vector2 chargeDirection;

        protected void Awake()
        {
            enemyData = new GreenSlimeData();
        }
        protected override void Attack(PlayerController player)
        {
            base.Attack(player);

            StartCharge();
        }

        public void StartCharge()
        {
            if (!isCharging)
            {
                isCharging = true;
                StartCoroutine(ChargeAttack());
            }
        }

        private IEnumerator ChargeAttack()
        {
            // Wait for the charge duration
            yield return new WaitForSeconds(chargeDuration);

            chargeDirection = ((Vector2)targetPlayerTransform.position - (Vector2)transform.position).normalized;

            // Perform the jump (you may need to adjust based on movement code)
            animationManager.rb.AddForce(chargeDirection * jumpForce, ForceMode2D.Impulse);

            // Reset charging state after the attack
            StartCoroutine(StopAttack());
        }

        private IEnumerator StopAttack()
        {
            yield return new WaitForSeconds(0.5f);

            isAttacking = false;
            isCharging = false;
        }

    }
}