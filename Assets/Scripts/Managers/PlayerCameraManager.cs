﻿using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Assets.Scripts.Managers
{
    using System.Collections.Generic;
    using UnityEngine;
    using Cinemachine;

    public class PlayerCameraManager : MonoBehaviour
    {
        public List<CinemachineVirtualCamera> playerCameras = new List<CinemachineVirtualCamera>();
        public List<Camera> mainCameras = new List<Camera>();

        private void Start()
        {
            SetupCameras();
        }

        private void SetupCameras()
        {
            int playerCount = playerCameras.Count;

            for (int i = 0; i < playerCount; i++)
            {
                if (mainCameras[i] != null)
                {
                    mainCameras[i].rect = GetViewportRect(i, playerCount);
                    playerCameras[i].Priority = 10;
                    Debug.Log($"Setting viewport for player {i} with rect {GetViewportRect(i, playerCount)}");
                }
                else
                {
                    Debug.LogError($"mainCameras[{i}] is null");
                }
            }
        }

        private Rect GetViewportRect(int index, int totalPlayers)
        {
            if (totalPlayers == 1)
            {
                return new Rect(0, 0, 1, 1);
            }
            else if (totalPlayers == 2)
            {
                return new Rect(index * 0.5f, 0, 0.5f, 1);
            }
            else if (totalPlayers == 3)
            {
                if (index == 0)
                    return new Rect(0, 0.5f, 1, 0.5f);
                else
                    return new Rect((index - 1) * 0.5f, 0, 0.5f, 0.5f);
            }
            else if (totalPlayers == 4)
            {
                return new Rect((index % 2) * 0.5f, (1 - index / 2) * 0.5f, 0.5f, 0.5f);
            }

            return new Rect(0, 0, 1, 1); // Default fallback
        }

        public void AddPlayerCamera(CinemachineVirtualCamera vcam, Camera mainCam)
        {
            if (vcam == null)
            {
                Debug.LogError("vcam is null");
                return;
            }

            if (mainCam == null)
            {
                Debug.LogError("mainCam is null");
                return;
            }

            playerCameras.Add(vcam);
            mainCameras.Add(mainCam);

            Debug.Log($"Added player camera: {vcam} and main camera: {mainCam}");

            SetupCameras();
        }
    }

}
