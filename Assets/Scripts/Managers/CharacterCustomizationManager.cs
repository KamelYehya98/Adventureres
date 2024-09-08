using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CharacterCustomizationManager : MonoBehaviour
    {
        [Header("Sprite to Change")]
        public SpriteRenderer bodyPart;

        [Header("Sprites to Cycle Through")]
        public List<Sprite> options = new();

        private int _currentOption = 0;

        public void NextOption()
        {
            _currentOption++;

            if(_currentOption >= options.Count)
            {
                _currentOption = 0;
            }

            bodyPart.sprite = options[_currentOption];
        }

        public void PrevioudOption()
        {
            _currentOption--;

            if(_currentOption <= 0) 
            {
                _currentOption = options.Count - 1;
            }

            bodyPart.sprite = options[_currentOption];
        }
    }
}
