using Assets.Scripts.Managers;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateBase : State
    {
        protected InventoryManager inventoryManager;
        protected PlayerCoreController playerController;
        protected PlayerInputController inputController;
        protected AnimationManager animationManager;
        protected PlayerSpells playerSpells;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);


            playerController = GetComponent<PlayerCoreController>();
            animationManager = GetComponent<AnimationManager>();
            inputController = GetComponent<PlayerInputController>();
            playerSpells = GetComponent<PlayerSpells>();

            inventoryManager = playerController.GetComponentInChildren<InventoryManager>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
