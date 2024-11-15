using UnityEngine;

namespace Assets.Scripts.Player
{
    public class WeaponHitBoxFlip : MonoBehaviour
    {
        private SpriteRenderer _weaponSpriteRenderer;
        private Collider2D _weaponCollider;

        public void Awake()
        {
            _weaponCollider = GetComponent<Collider2D>();
            _weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void FixedUpdate()
        {
            _weaponCollider.offset = new Vector2((_weaponSpriteRenderer.flipX ? -1 : 1) * _weaponCollider.offset.x, _weaponCollider.offset.y);
        }
    }
}
