using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MeleeAttack1_1 : AttackBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            attackIndex = 1;
            duration = 0.417f;

            StartAttackAnimation("Attack " + attackIndex);
            SetComboIndex(attackIndex - 1);

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
                    stateMachine.SetNextState(new MeleeAttack1_2());
                }
                else
                {
                    stateMachine.SetNextState(new IdleState());
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}