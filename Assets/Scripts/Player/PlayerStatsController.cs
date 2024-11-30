using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [SerializeField]
        private PlayerCoreController coreController;

        public PlayerData playerData;

        public void Initialize(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public void TakeDamage(float damage)
        {
            StartCoroutine(coreController.invulnerability.StartInvulnerability());
            
            // Take damage
        }
    }
}