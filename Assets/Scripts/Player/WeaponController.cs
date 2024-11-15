using Assets.Scripts.Enemiies;
using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class WeaponController : MonoBehaviour
    {
        public ItemData weaponData;

        private SpriteRenderer weaponSpriteRenderer;

        [SerializeField]
        private SpriteRenderer baseSpriteRenderer;

        [SerializeField]
        public PlayerCoreController playerCoreController;

        private Collider2D weaponCollider;

        public int currentComboIndex = 0;
        public int currentAttackIndex = 0;
        private void Awake()
        {
            weaponSpriteRenderer = GetComponent<SpriteRenderer>();
            weaponCollider = GetComponent<Collider2D>();
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

        public void FixedUpdate()
        {
            weaponSpriteRenderer.flipX = baseSpriteRenderer.flipX;
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.LogWarning("Entered collision with weapon");

            if (other.gameObject.TryGetComponent(out TeamComponent teamComponent) && 
                    other.gameObject.TryGetComponent(out Enemy enemy))
            {
                if (teamComponent != null && teamComponent.teamIndex == TeamIndex.Enemy && enemy != null && playerCoreController != null)
                {
                }
            }
        }

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.TryGetComponent(out TeamComponent teamComponent))
        //    {
        //        if (teamComponent != null && teamComponent.teamIndex == TeamIndex.Enemy)
        //        {
        //            rb.bodyType = RigidbodyType2D.Dynamic;
        //        }
        //    }
        //}
    }
}
