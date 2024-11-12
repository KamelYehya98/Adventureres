using Assets.Scripts.Managers;
using Assets.Scripts.Player.States;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CharacterStateManager : MonoBehaviour
    {
        public StateMachine meleeStateMachine;
        private PlayerInputController playerControls;

        [SerializeField] public Collider2D hitbox;
        [SerializeField] public GameObject Hiteffect;

        // Start is called before the first frame update
        void Awake()
        {
            meleeStateMachine = GetComponent<StateMachine>();
            playerControls = GetComponent<PlayerInputController>();
        }

        private void Start()
        {
            meleeStateMachine.SetNextState(new IdleCombatState());
        }
    }
}