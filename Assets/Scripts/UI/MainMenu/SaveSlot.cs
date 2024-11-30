using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Data;

namespace Assets.Scripts.UI.MainMenu
{
    public class SaveSlot : MonoBehaviour
    {
        [Header("Profile")]
        [SerializeField] private string _profileId = "";

        [Header("Content")]
        [SerializeField] private GameObject noDataContent;
        [SerializeField] private GameObject hasDataContent;
        [SerializeField] private TextMeshProUGUI percentageCompleteText;
        [SerializeField] private TextMeshProUGUI deathCountText;

        [Header("Clear Data Button")]
        [SerializeField] private Button clearButton;

        public bool hasData { get; private set; } = false;

        private Button saveSlotButton;

        public void Awake()
        {
            saveSlotButton = this.GetComponent<Button>();
        }

        public void SetData(GameData data)
        {
            if (data == null)
            {
                hasData = false;
                noDataContent.SetActive(true);
                hasDataContent.SetActive(false);
                clearButton.gameObject.SetActive(false);
            }
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
            return this._profileId;
        }

        public void SetInteractable(bool interactable)
        {
            saveSlotButton.interactable = interactable;
            clearButton.interactable = interactable;
        }
    }

}
