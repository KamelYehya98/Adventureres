using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    public class CreatePlayerMenu : Menu
    {
        [Header("Menu Buttons")]
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private Button startGameBtn;
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameObject backButtonObject;

        private string _playerNameText;
        private PlayerDataManager _playerDataManager;

        private void Start()
        {
            if(string.IsNullOrEmpty(playerNameInput.textComponent.text))
            {
                startGameBtn.enabled = false;
            }

            _playerDataManager = FindObjectOfType<PlayerDataManager>();
        }

        private void HandleBackButton()
        {
            DeactivateMenu();

            backButtonObject.SetActive(false);

            mainMenu.ActivateMenu();
        }

        public void ActivateMenu()
        {
            this.gameObject.SetActive(true);

            Button backButton = backButtonObject.GetComponent<Button>();

            if (backButton != null)
            {
                backButtonObject.SetActive(true);
                backButton.enabled = true;
                backButton.onClick.AddListener(() => HandleBackButton());
            }
        }

        public void DeactivateMenu() 
        {
            this.gameObject.SetActive(false);
        }

        public void OnInputTextUpdate()
        {
            _playerNameText = playerNameInput.textComponent.text;
        }

        public void OnStartGameClicked()
        {
            DeactivateMenu();

            SceneManager.LoadSceneAsync("SampleScene");

            _playerDataManager.AddNewPlayer(_playerNameText);
        }
    }
}
