using Assets.Scripts.Managers;

namespace Assets.Scripts.Player
{
    public class RunningState : State
    {
        private readonly AnimationManager _animationManager;
        private readonly PlayerInputController _playerInputController;

        public RunningState(AnimationManager animationManager, PlayerInputController playerInputController)
        {
            _animationManager = animationManager;
            _playerInputController = playerInputController;
        }

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            _animationManager.MovementAnimation();

            if(_playerInputController.inputActions.Player.Attack.triggered)
            {
                stateMachine.SetNextState(new MeleeEntryState());
            }
        }
    }
}
