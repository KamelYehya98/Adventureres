﻿using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class GroundFinisherState : MeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            attackIndex = 3;
            duration = 0.5f;
            animator.SetTrigger("Attack" + attackIndex);
            Debug.Log("Player Attack " + attackIndex + " Fired!");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= duration)
            {
                stateMachine.SetNextStateToMain();
            }
        }
    }
}
