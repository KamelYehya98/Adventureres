using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MeleeAttack1_2 : AttackBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            // Attack
            attackIndex = 2;
            duration = 0.417f;  // Second attack duration
            animationManager.StartAttackAnimation("Attack " + attackIndex);
            weaponController.SetComboIndex(attackIndex - 1);

            Debug.Log("Player Attack " + attackIndex + " Fired!");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (animationManager.animator.GetFloat("AttackWindow.Open") > 0f && inputController.attackInput == 1)
            {
                shouldCombo = true;  // Allow combo if the attack input was pressed in the attack window
                AttackPressedTimer = 0;  // Reset the input buffer
                inputController.attackInput = 0;
            }

            if (fixedtime >= duration)
            {
                if (shouldCombo)
                {
                    stateMachine.SetNextState(new MeleeAttack1_3()); // Transition to the third attack
                }
                else
                {
                    stateMachine.SetNextStateToMain(); // Return to main state
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}