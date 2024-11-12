using Assets.Scripts.Classes;
using Assets.Scripts.Managers;
using Unity;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateBase : State
    {
        protected PlayerController playerController;
        protected PlayerInputController inputController;
        protected InventoryManager inventoryManager;
        protected AnimationManager animationManager;
        protected PlayerSpells playerSpells;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            playerController = GetComponent<PlayerController>();
            inventoryManager = GetComponent<InventoryManager>();
            animationManager = GetComponent<AnimationManager>();
            inputController = GetComponent<PlayerInputController>();
            playerSpells = GetComponent<PlayerSpells>();
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
