using Assets.Scripts.Abilities;
using Assets.Scripts.Classes;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MagicAttack1_1 : AttackBaseState
    {
        private Vector3 direction;
        private PlayerController controller;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            controller = inputController.GetComponent<PlayerController>();

            if (animationManager.facingDown)
            {
                direction = Vector3.down;
            }
            else if (animationManager.facingUp)
            {
                direction = Vector3.up;
            }
            else if (animationManager.facingHorizontal)
            {
                direction = controller._spriteRenderer.flipX ? Vector3.left : Vector3.right;
            }

            GameObject fireball = Object.Instantiate(playerSpells.FireBall1, controller.transform.position, Quaternion.identity);

            fireball.transform.localScale = new(3, 3, 3);

            fireball.GetComponent<SpriteRenderer>().flipX = controller._spriteRenderer.flipX;
            fireball.GetComponent<Fireball>().Launch(direction);

            stateMachine.SetNextState(new IdleCombatState()); // Return to main state
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
