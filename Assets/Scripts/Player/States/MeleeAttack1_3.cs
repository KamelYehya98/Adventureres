using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MeleeAttack1_3 : AttackBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            attackIndex = 3;
            duration = 0.714f;
            animationManager.StartAttackAnimation("Attack " + attackIndex);
            weaponComponent.SetComboIndex(attackIndex - 1);

            AttackPressedTimer = 0;  // Reset the input buffer
            inputController.attackInput = 0;

            Debug.Log("Player Attack " + attackIndex + " Fired!");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= duration)
            {
                stateMachine.SetNextStateToMain();
            }
        }
        public override void OnExit()
        {
            base.OnExit();
        }
    }
}