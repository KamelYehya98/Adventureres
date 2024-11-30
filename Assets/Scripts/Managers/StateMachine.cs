using Assets.Scripts.Player.States;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class StateMachine : MonoBehaviour
    {
        public string customName;
        public State CurrentState { get; private set; }

        private State _nextState;
        private State _mainStateType;


        public void Awake()
        {
            _mainStateType = new IdleState();

            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (_mainStateType == null)
                {
                    if (customName == "Player")
                    {
                        _mainStateType = new IdleState();
                    }
                }
            };

            SetNextStateToMain();
        }

        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }

        public void Update()
        {
            if (_nextState != null)
            {
                SetState(_nextState);
            }

            CurrentState?.OnUpdate();
        }

        public void LateUpdate()
        {
            CurrentState?.OnLateUpdate();
        }

        private void SetState(State _newState)
        {
            _nextState = null;

            CurrentState?.OnExit();
            CurrentState = _newState;
            CurrentState.OnEnter(this);
        }

        public void SetNextState(State _newState)
        {
            if (_newState != null)
            {
                _nextState = _newState;
            }
        }

        public void SetNextStateToMain()
        {
            _nextState = _mainStateType;
        }
    }
}