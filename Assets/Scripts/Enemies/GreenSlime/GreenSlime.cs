using Assets.Scripts.Enemiies;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemies.GreenSlime
{
    public class GreenSlime : MeleeEnemy
    {
        public float chargeDuration = 2f;
        public float jumpForce = 1f;

        private bool isCharging = false;
        private Vector2 chargeDirection;

        public void Awake()
        {
            statsController.InitializeData(new GreenSlimeData());
        }

        protected override void Attack()
        {
            base.Attack();

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
            yield return new WaitForSeconds(chargeDuration);

            chargeDirection = movementController.GetDirectionToPlayer();

            movementController.rb.AddForce(chargeDirection * jumpForce, ForceMode2D.Impulse);

            StartCoroutine(StopAttack());
        }

        private IEnumerator StopAttack()
        {
            yield return new WaitForSeconds(0.5f);

            movementController.EndAttack();

            isCharging = false;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}