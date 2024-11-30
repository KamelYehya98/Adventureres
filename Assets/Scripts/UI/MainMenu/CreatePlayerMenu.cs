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

        public GameManager gameManager;

        private string _playerNameText;

        public void Start()
        {
            if(string.IsNullOrEmpty(playerNameInput.textComponent.text))
            {
                startGameBtn.enabled = false;
            }
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

            gameManager.AddNewPlayerToGameData(_playerNameText);
        }
    }
}
