using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlayerCanvasPositionManager : MonoBehaviour
    {
        public Transform player;

        public void Update()
        {
            if (player != null)
            {
                transform.position = player.position;
            }
        }
    }
}
