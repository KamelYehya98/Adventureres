using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField]
        private PlayerCoreController coreController;

        public void ChangeWeapon(ItemData weaponData)
        {
            if (coreController.weaponController.IsDifferentWeapon(weaponData.Id))
            {
                coreController.weaponController.SetWeapon(weaponData);
                coreController.weaponController.ResetAttacksIndices();
            }
        }

        public void OnAttackAddForce()
        {
            if (coreController.weaponController.weaponData.type == ItemType.Sword)
            {
                int currentComboIndex = coreController.weaponController.currentComboIndex;
                float[] forces = coreController.weaponController.weaponData.attackForceOnPlayer;

                Debug.Log("Adding attack force to player swing with current combo index: " + currentComboIndex);

                if (coreController.animationManager.facingDown)
                {
                    coreController.movementController.rb.AddForce(Vector2.down * forces[currentComboIndex], ForceMode2D.Impulse);
                }
                else if (coreController.animationManager.facingUp)
                {
                    coreController.movementController.rb.AddForce(Vector2.up * forces[currentComboIndex], ForceMode2D.Impulse);
                }
                else if (coreController.animationManager.facingHorizontal)
                {
                    Vector2 direction = coreController.transform.localScale.x == -1 ? Vector2.left : Vector2.right;

                    coreController.movementController.rb.AddForce(direction * forces[currentComboIndex], ForceMode2D.Impulse);
                }
            }
        }

        public void OnAttackStopForce()
        {
            coreController.movementController.rb.velocity = Vector2.zero;
        }
    }
}