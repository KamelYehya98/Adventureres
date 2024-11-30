using Assets.Scripts.Managers;
using UnityEngine;


namespace Assets.Scripts.Player.States
{
    public class IdleState : PlayerStateBase
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            EndAttackAnimation();
            OnAttackStopForceTrigger();
            //ResetAttacksIndicies();

            Debug.Log("Entered idle state");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            CheckRunningState();
            CheckMeleeEntryState();
        }

        public override void OnExit()
        {
            base.OnExit();

            EndAttackAnimation();
        }

        public void CheckRunningState()
        {
            if (playerStateManager.coreController.animationManager.IsMoving())
            {
                stateMachine.SetNextState(new RunningState());
            }
        }
    }
}