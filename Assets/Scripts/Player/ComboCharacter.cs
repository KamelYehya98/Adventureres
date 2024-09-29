using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ComboCharacter : MonoBehaviour
    {
        private StateMachine meleeStateMachine;
        private PlayerInputController playerControls;

        [SerializeField] public Collider2D hitbox;
        [SerializeField] public GameObject Hiteffect;

        // Start is called before the first frame update
        void Awake()
        {
            meleeStateMachine = GetComponent<StateMachine>();
            playerControls = GetComponent<PlayerInputController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (meleeStateMachine != null)
            {
                if (meleeStateMachine.CurrentState != null)
                {
                    if (playerControls.attackInput > 0 && meleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
                    {
                        meleeStateMachine.SetNextState(new GroundEntryState());
                    }
                }
            }
        }
    }
}
