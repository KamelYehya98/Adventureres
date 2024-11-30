using Assets.Scripts.Managers;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCoreController : MonoBehaviour
    {
        public Rigidbody2D rb;
        public SpriteRenderer spriteRenderer;

        public PlayerAnimationManager animationManager;
        public PlayerWeaponController weaponController;
        public PlayerCombatController combatController;
        public PlayerMovementController movementController;
        public PlayerStatsController statsController;
        public PlayerInvulnerability invulnerability;
        public PlayerInputController inputController;
        public PlayerStateManager stateManager;
        public CinemachineVirtualCamera cinemachine;
        public PlayerAnimationTriggers animationTriggers;
        public new Camera camera;

        public void FixedUpdate()
        {
            Move(inputController.GetMovementInput());
        }

        public void Move(Vector2 direction)
        {
            if(!animationManager.IsAttackState() && statsController.playerData.Skills != null)
            {
                movementController.Move(direction, statsController.playerData.Skills.Agility);
            }
        }
    }
}