using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class MagicEntryState : State
    {
        public override void OnEnter(StateMachine _stateMachine)
        {

            Debug.Log("Entered MagicEntry Stateeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
            base.OnEnter(_stateMachine);

            State nextState = new MagicAttack1_1();
            stateMachine.SetNextState(nextState);
        }
    }
}
