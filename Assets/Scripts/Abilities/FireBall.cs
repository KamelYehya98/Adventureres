using Assets.Scripts.Enemiies;
using UnityEngine;

namespace Assets.Scripts.Abilities
{
    public class Fireball : MonoBehaviour
    {
        public float speed;
        public float damage;

        public void Launch(Vector3 direction)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = direction * speed;

            RotateFireball(direction);
        }

        void RotateFireball(Vector3 direction)
        {
            if (direction == Vector3.up)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90); // Up direction, no rotation needed
            }
            else if (direction == Vector3.down)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90); // Down direction
            }
            else if (direction == Vector3.left)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Left direction
            }
            else if (direction == Vector3.right)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Right direction
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}