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
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = direction * speed;
        }

        private void OnCollisionEnter(Collision collision)
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