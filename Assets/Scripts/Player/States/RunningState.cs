using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class RunningState : PlayerStateBase
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            EndAttackAnimation();
            ResetAttacksIndicies();
            OnAttackStopForceTrigger();

            Debug.Log("Entered Running State");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            CheckIdleState();
            CheckMeleeEntryState();
        }

        public override void OnExit()
        {
            base.OnExit();

            playerStateManager.coreController.animationManager.EndAttackAnimation();
        }

        public void CheckIdleState()
        {
            if (!playerStateManager.coreController.animationManager.IsMoving())
            {
                stateMachine.SetNextState(new IdleState());
            }
        }
    }
}
