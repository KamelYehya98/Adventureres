using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    public class MainMenu : Menu
    {
        [Header("Menu Navigation")]
        [SerializeField] public SaveSlotsMenu saveSlotsMenu;
        [SerializeField] public CreatePlayerMenu createPlayerMenu;

        [Header("Menu Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;

        private void Start()
        {
            DisableButtonsDependingOnData();
        }

        private void DisableButtonsDependingOnData()
        {
            var allProfiles = DataPersistenceManager.Instance.GetAllProfilesGameData();

            if (!DataPersistenceManager.Instance.HasGameData() && (allProfiles == null || allProfiles.Count == 0))
            {
                loadGameButton.interactable = false;
            }
        }

        public void OnNewGameClicked()
        {
            createPlayerMenu.ActivateMenu();
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked()
        {
            this.DeactivateMenu();

            saveSlotsMenu.ActivateMenu(true);
        }

        private void DisableMenuButtons()
        {
            newGameButton.interactable = false;
        }

        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);
            DisableButtonsDependingOnData();
        }

        public void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}