using UnityEngine;

namespace Assets.Scripts.Abilities
{
    [CreateAssetMenu(fileName = "FireballAbility", menuName = "RPG/Abilities/Fireball")]
    public class FireballAbility : Ability
    {
        public GameObject fireballPrefab;
        public float damage;

        protected override void PerformAbility(GameObject user)
        {
            GameObject fireball = Instantiate(fireballPrefab, user.transform.position, Quaternion.identity);
            Fireball fireballScript = fireball.GetComponent<Fireball>();
            fireballScript.damage = damage;
            fireballScript.Launch(user.transform.forward);
        }
    }

}