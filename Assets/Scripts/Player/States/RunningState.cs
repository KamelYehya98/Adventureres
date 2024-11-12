using Assets.Scripts.Managers;
using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class RunningState : PlayerStateBase
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            animationManager.animator.SetBool("IsAttacking", false);
            Debug.Log("Entered Running State");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            animationManager.MovementAnimation();

            if (inputController.moveInput == Vector2.zero)
            {
                stateMachine.SetNextState(new IdleCombatState());
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
