using Assets.Scripts.Managers;
using UnityEngine;


namespace Assets.Scripts.Player
{
    public class IdleCombatState : MeleeBaseState
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
            animationManager.animator.SetBool("IsAttacking", false);
            Debug.Log("entered idle states");
        }

        public override void OnUpdate()
        {
            animationManager.MovementAnimation();

            if (inputController.inputActions.Player.Move.triggered)
            {
                stateMachine.SetNextState(new RunningState(animationManager, inputController));
            }
            else if (inputController.inputActions.Player.Attack.triggered)
            {
                stateMachine.SetNextState(new MeleeEntryState());
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            animationManager.animator.SetBool("IsAttacking", false);
        }
    }
}