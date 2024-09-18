using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    public class Menu : MonoBehaviour
    {
        [Header("First Selected Button")]
        [SerializeField] private Button firstSelected;

        protected virtual void OnEnable()
        {
            SetFirstSelected(firstSelected);
        }

        public void SetFirstSelected(Button firstSelectedButton)
        {
            if (firstSelectedButton != null)
            {
                firstSelectedButton.Select();
            }
        }
    }
}
