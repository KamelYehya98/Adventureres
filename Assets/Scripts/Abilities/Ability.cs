using UnityEngine;

namespace Assets.Scripts.Abilities
{

}
public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldown;
    public int manaCost;
    private float lastUseTime;

    public bool CanActivate()
    {
        return Time.time >= lastUseTime + cooldown;
    }

    public void Activate(GameObject user)
    {
        if (CanActivate())
        {
            lastUseTime = Time.time;
            PerformAbility(user);
        }
    }

    protected abstract void PerformAbility(GameObject user);
}
