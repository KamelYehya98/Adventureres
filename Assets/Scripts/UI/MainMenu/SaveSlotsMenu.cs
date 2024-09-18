using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    public class SaveSlotsMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] private MainMenu mainMenu;

        [Header("Menu Buttons")]
        [SerializeField] private GameObject backButtonObject;

        [Header("Confirmation Popup")]
        [SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;

        [Header("Vertical Group Content")]
        [SerializeField] private Button content;
        [SerializeField] private Transform ContentParent;

        private bool isLoadingGame = true;

        private List<Button> saveSlots;

        private void Awake()
        {
            //saveSlots = this.GetComponentsInChildren<SaveSlot>();
        }

        public void OnSaveSlotClicked(GameData gameData)
        {
            // Disable all buttons
            if(gameData != null)
            {
                DisableSaveSlotsButtons();

                Debug.Log("Selected profile id to load: " + gameData.Id.ToString());

                DataPersistenceManager.Instance.ChangeSelectedProfileId(gameData.Id.ToString());

                DataPersistenceManager.Instance.LoadGame();

                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                Debug.LogError("Gamedata is null, unable to load game for the selected profile id");
            }
        }

        public void OnClearClicked(SaveSlot saveSlot)
        {
            DisableSaveSlotsButtons();

            confirmationPopupMenu.ActivateMenu(
                "Are you sure you want to delete this saved data?",
                // function to execute if we select 'yes'
                () => {
                    DataPersistenceManager.Instance.DeleteProfileData(saveSlot.GetProfileId());
                    ActivateMenu(isLoadingGame);
                },
                // function to execute if we select 'cancel'
                () => {
                    ActivateMenu(isLoadingGame);
                }
            );
        }

        public void OnBackClicked()
        {
            mainMenu.ActivateMenu();
            this.DeactivateMenu();
        }

        private void HandleBackButton()
        {
            DeactivateMenu();

            backButtonObject.SetActive(false);

            mainMenu.ActivateMenu();
        }

        public void ActivateMenu(bool isLoadingGame)
        {
            // set this menu to be active
            this.gameObject.SetActive(true);

            // set mode
            this.isLoadingGame = isLoadingGame;

            Button backButton = backButtonObject.GetComponent<Button>();

            if (backButton != null)
            {
                backButtonObject.SetActive(true);
                backButton.enabled = true;
                backButton.onClick.AddListener(() => HandleBackButton());
            }

            if(saveSlots == null || saveSlots.Count == 0)
            {
                saveSlots = new();
                // load all of the profiles that exist
                Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

                if (profilesGameData == null || profilesGameData.Count == 0)
                {
                    Debug.LogError("profiles game data is empty");
                }

                // loop through each save slot in the UI and set the content appropriately
                foreach (var saveSlot in profilesGameData)
                {
                    Button saveSlotBtn = Instantiate(content, ContentParent);

                    saveSlotBtn.onClick.AddListener(() => OnSaveSlotClicked(saveSlot.Value));

                    TextMeshProUGUI text = saveSlotBtn.GetComponentInChildren<TextMeshProUGUI>(true);

                    if (text == null)
                    {
                        Debug.LogError("Text component null");
                    }
                    // Set the text of the new element
                    saveSlotBtn.GetComponentInChildren<TextMeshProUGUI>().text = saveSlot.Key;

                    saveSlots.Add(saveSlotBtn);
                }

                EnableSaveSlotsButtons();
            }

           
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }

        private void DisableSaveSlotsButtons()
        {
            if (saveSlots != null && saveSlots.Count > 0)
            {
                foreach (Button saveSlot in saveSlots)
                {
                    saveSlot.interactable = false;
                }
            }
        }

        private void EnableSaveSlotsButtons()
        {
            if (saveSlots != null && saveSlots.Count > 0)
            {
                foreach (Button saveSlot in saveSlots)
                {
                    saveSlot.interactable = true;
                }
            }
        }
    }
}