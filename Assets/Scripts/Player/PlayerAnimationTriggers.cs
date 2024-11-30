using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationTriggers : MonoBehaviour
    {
        [SerializeField]
        private PlayerCoreController coreController;

        public void OnAttackAddForceTrigger()
        {
            coreController.combatController.OnAttackAddForce();
        }

        public void OnAttackStopForceTrigger()
        {
            coreController.combatController.OnAttackStopForce();
        }
    }
}
