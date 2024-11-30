using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerCoreController coreController;
        public PlayerCanvasController canvasController;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void InstantiatePlayer(PlayerData playerData, string controlScheme)
        {
            InitializePlayerData(playerData);
            AssignPlayerControlScheme(controlScheme);
            SetupPlayerCameras();
            SetupPlayerCanvas();
        }

        private void InitializePlayerData(PlayerData playerData)
        {
            coreController.statsController.Initialize(playerData);
        }

        private void AssignPlayerControlScheme(string controlScheme)
        {
            coreController.inputController.AssignControlScheme(controlScheme);
        }

        private void SetupPlayerCameras()
        {
            coreController.cinemachine.Follow = transform;
            coreController.cinemachine.LookAt = transform;

            coreController.camera.enabled = true;
            coreController.camera.GetComponent<AudioListener>().enabled = true;
        }

        private void SetupPlayerCanvas()
        {
            canvasController.playerCanvas.worldCamera = coreController.GetComponent<Camera>();
            canvasController.playerCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvasController.playerCanvas.sortingLayerName = "Background";
        }
    }
}
