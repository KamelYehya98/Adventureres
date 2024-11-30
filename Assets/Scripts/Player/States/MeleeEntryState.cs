using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MeleeEntryState : State
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            Debug.Log("Entered MeleeEntry State");

            State nextState = new MeleeAttack1_1();
            stateMachine.SetNextState(nextState);
        }
    }

}
