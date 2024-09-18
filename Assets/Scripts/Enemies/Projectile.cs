using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts.Enemiies
{
    public class Projectile : MonoBehaviour
    {
        public float speed;
        public float damage;
        private Vector3 _direction;
        private float _lifetime = 2.0f; // Time in seconds before the projectile is destroyed

        public void Initialize(Transform targetTransform)
        {
            _direction = (targetTransform.position - transform.position).normalized;
            Destroy(gameObject, _lifetime);
        }

        private void Update()
        {
            transform.position += speed * Time.deltaTime * _direction;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("Projectile collided with: " + other.gameObject.name);

            if (other.TryGetComponent<PlayerController>(out var player))
            {
                //Debug.Log("Projectile collided with player");
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
        }
    }

}
