using Assets.Scripts.Managers;
using Assets.Scripts.Scriptable_Objects;
using UnityEngine;


namespace Assets.Scripts.Player.States
{
    public class IdleCombatState : PlayerStateBase
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            animationManager.animator.SetBool("IsAttacking", false);
            playerController.OnAttackStopForceTrigger();
            Debug.Log("entered idle states");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            animationManager.MovementAnimation();

            if (inputController.moveInput != Vector2.zero)
            {
                stateMachine.SetNextState(new RunningState());
            }
            else if (inputController.attackInput > 0)
            {
                ItemType itemType = inventoryManager.GetCurrentItemType();

                if (itemType != ItemType.None)
                {
                    Debug.Log("Item type not null");

                    if (itemType == ItemType.Sword)
                    {
                        Debug.Log("Initiating sword attack");

                        stateMachine.SetNextState(new MeleeEntryState());
                    }
                    else if (itemType == ItemType.Staff)
                    {
                        Debug.Log("Initiating magic attack");

                        stateMachine.SetNextState(new MagicEntryState());
                    }
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