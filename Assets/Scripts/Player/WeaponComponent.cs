using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class WeaponComponent : MonoBehaviour
    {
        protected ItemData weaponData;

        private SpriteRenderer weaponSpriteRenderer;

        [SerializeField]
        private SpriteRenderer baseSpriteRenderer;

        private int currentComboIndex = 0;
        private int currentAttackIndex = 0;

        private void Awake()
        {
            weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetWeapon(ItemData weaponData)
        {
            if (this.weaponData == null || this.weaponData.Id != weaponData.Id)
            {
                this.weaponData = weaponData;

                currentAttackIndex = 0;
                currentComboIndex = 0;
            }
        }

        public void Update()
        {
            weaponSpriteRenderer.flipX = baseSpriteRenderer.flipX;
        }
        public void OnPlayerSpriteChanged(Sprite playerSprite)
        {
            Debug.Log("Entered sprite changer...........................................................");

            if (weaponData.comboAnimations.Length > 0)
            {
                if (currentComboIndex < weaponData.comboAnimations.Length)
                {
                    ComboAnimations currentCombo = weaponData.comboAnimations[currentComboIndex];

                    if (currentAttackIndex < currentCombo.animations.Length)
                    {
                        weaponSpriteRenderer.sprite = currentCombo.animations[currentAttackIndex];

                        currentAttackIndex++;
                    }
                }
            }
        }

        public void SetComboIndex(int index)
        {
            if (index >= 0 && index < weaponData.comboAnimations.Length)
            {
                currentComboIndex = index;
            }
        }

        public void ResetAttackIndex()
        {
            currentAttackIndex = 0;
            currentComboIndex = 0;
        }
    }
}
