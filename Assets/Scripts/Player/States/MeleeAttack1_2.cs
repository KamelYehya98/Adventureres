using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MeleeAttack1_2 : AttackBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            attackIndex = 2;
            duration = 0.417f;

            StartAttackAnimation("Attack " + attackIndex);
            SetComboIndex(attackIndex - 1);
            ResetAttackBufferOnAttack();

            Debug.Log("Player Attack " + attackIndex + " Fired!");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            CheckIfShouldCombo();

            if (time >= duration)
            {
                if (shouldCombo)
                {
                    stateMachine.SetNextState(new MeleeAttack1_3());
                }
                else
                {
                    stateMachine.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}