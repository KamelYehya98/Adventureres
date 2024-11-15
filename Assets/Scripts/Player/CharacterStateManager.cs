using Assets.Scripts.Managers;
using Assets.Scripts.Player.States;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CharacterStateManager : MonoBehaviour
    {
        public StateMachine meleeStateMachine;
        private PlayerInputController playerControls;

        public Collider2D hitbox;
        public GameObject Hiteffect;
        public WeaponController WeaponController;

        // Start is called before the first frame update
        public void Awake()
        {
            meleeStateMachine = GetComponent<StateMachine>();
            playerControls = GetComponent<PlayerInputController>();
            WeaponController = GetComponentInChildren<WeaponController>();
        }

        public void Start()
        {
            meleeStateMachine.SetNextState(new IdleCombatState());
        }
    }
}