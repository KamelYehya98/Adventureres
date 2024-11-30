using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField]
        private PlayerCoreController coreController;

        public Rigidbody2D rb;

        public void Move(Vector2 direction, float agility)
        {
            if (direction != Vector2.zero && agility != 0)
            {
                Vector2 newPosition = rb.position + (direction.normalized * agility * Time.fixedDeltaTime);

                rb.MovePosition(newPosition);
                coreController.animationManager.SetMovement(direction);
            }
            else if(!coreController.animationManager.IsAttackState())
            {
                coreController.animationManager.SetMovement(Vector2.zero);
            }
        }

        public bool IsChangingDirection()
        {
            if (
                    coreController.inputController.moveInput != Vector2.zero &&
                    (
                        (coreController.animationManager.facingDown && coreController.inputController.moveInput.y > 0) ||
                        (coreController.animationManager.facingUp && coreController.inputController.moveInput.y < 0) ||
                        (coreController.animationManager.facingHorizontal && coreController.animationManager.GetLocalScaleX() == -1 && coreController.inputController.moveInput.x > 0) ||
                        (coreController.animationManager.facingHorizontal && coreController.animationManager.GetLocalScaleX() == 1 && coreController.inputController.moveInput.x < 0)
                    )
               )
            {
                return true;
            }

            return false;
        }

        public Vector2 GetFacingDirection()
        {
            if (coreController.animationManager.facingDown)
                return Vector2.down;
            else if (coreController.animationManager.facingUp)
                return Vector2.up;
            else if (coreController.animationManager.facingHorizontal && coreController.transform.localScale.x == -1)
                return Vector2.left;
            else if (coreController.animationManager.facingHorizontal && coreController.transform.localScale.x == 1)
                return Vector2.right;

            else return Vector2.zero;
        }
    }
}
