﻿using Assets.Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    public class SaveSlot : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private string profileId = "";

        [Header("Content")]
        [SerializeField] private GameObject noDataContent;
        [SerializeField] private GameObject hasDataContent;
        [SerializeField] private TextMeshProUGUI percentageCompleteText;
        [SerializeField] private TextMeshProUGUI deathCountText;

        [Header("Clear Data Button")]
        [SerializeField] private Button clearButton;

        public bool hasData { get; private set; } = false;

        private Button saveSlotButton;

        private void Awake()
        {
            saveSlotButton = this.GetComponent<Button>();
        }

        public void SetData(GameData data)
        {
            // there's no data for this profileId
            if (data == null)
            {
                hasData = false;
                noDataContent.SetActive(true);
                hasDataContent.SetActive(false);
                clearButton.gameObject.SetActive(false);
            }
            // there is data for this profileId
            else
            {
                hasData = true;
                noDataContent.SetActive(false);
                hasDataContent.SetActive(true);
                clearButton.gameObject.SetActive(true);
            }
        }

        public string GetProfileId()
        {
            return this.profileId;
        }

        public void SetInteractable(bool interactable)
        {
            saveSlotButton.interactable = interactable;
            clearButton.interactable = interactable;
        }
    }

}