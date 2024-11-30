using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        public ItemData weaponData;

        public SpriteRenderer weaponSpriteRenderer;
        public SpriteRenderer baseSpriteRenderer;
        public Collider2D weaponCollider;

        public int currentComboIndex = 0;
        public int currentAttackIndex = 0;

        public void ResetAttacksIndices()
        {
            currentAttackIndex = 0;
            currentComboIndex = 0;
        }

        public void ResetAttackIndex()
        {
            currentAttackIndex = 0;
        }

        public void SetWeapon(ItemData itemData)
        {
            weaponData = itemData;
        }

        public bool IsDifferentWeapon(string newWeaponId)
        {
            return weaponData == null || weaponData.Id != newWeaponId;
        }

        public bool HasWeapon()
        {
            return weaponData != null;
        }

        public ItemType GetWeaponType()
        {
            if (HasWeapon())
            {
                return weaponData.type;
            }

            return ItemType.None;
        }

        public void SetComboIndex(int index)
        {
            if (index >= 0 && index < weaponData.comboAnimations.Length)
            {
                currentComboIndex = index;
            }
        }

        public void OnPlayerSpriteChanged(Sprite playerSprite)
        {
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
    }
}