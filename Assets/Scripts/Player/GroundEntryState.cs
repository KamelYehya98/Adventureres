using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class GroundEntryState : MeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            // Attack
            attackIndex = 1;
            duration = 0.429f; // First attack duration
            animationManager.StartAttackAnimation("Attack " + attackIndex);
            Debug.Log("Player Attack " + attackIndex + " Fired!");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (animationManager.animator.GetFloat("AttackWindow.Open") > 0f && inputController.attackInput > 0)
            {
                shouldCombo = true;  // Allow combo if the attack input was pressed in the attack window
                AttackPressedTimer = 0;  // Reset the input buffer
                inputController.attackInput = 0;
            }

            if (fixedtime >= duration)
            {
                if (shouldCombo)
                {
                    stateMachine.SetNextState(new GroundComboState()); // Transition to second attack
                }
                else
                {
                    stateMachine.SetNextState(new IdleCombatState()); // Return to main state
                }
            }
        }


        public override void OnExit()
        {
            base.OnExit();
            animationManager.animator.SetBool("IsAttacking", false);
        }

    }
}