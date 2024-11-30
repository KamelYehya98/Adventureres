using Assets.Scripts.Managers;
using Assets.Scripts.Player.States;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStateManager : MonoBehaviour
    {
        [SerializeField]
        public PlayerCoreController coreController;

        public StateMachine meleeStateMachine;

        public Collider2D hitbox;
        public GameObject Hiteffect;
        public SpriteRenderer spriteRendrer;

        public void Start()
        {
            meleeStateMachine.SetNextState(new IdleState());
        }
    }
}