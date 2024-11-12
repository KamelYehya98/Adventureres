using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MeleeAttack1_1 : AttackBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            // Attack
            attackIndex = 1;
            duration = 0.571f; // First attack duration
            animationManager.StartAttackAnimation("Attack " + attackIndex);
            weaponComponent.SetComboIndex(attackIndex - 1);
            
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
                    stateMachine.SetNextState(new MeleeAttack1_2()); // Transition to second attack
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
        }

    }
}