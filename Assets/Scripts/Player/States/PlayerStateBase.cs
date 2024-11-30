using Assets.Scripts.Managers;
using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateBase : State
    {
        protected PlayerStateManager playerStateManager;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            playerStateManager = GetComponent<PlayerStateManager>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void ResetAttackIndex()
        {
            playerStateManager.coreController.weaponController.ResetAttackIndex();
        }

        public void ResetAttacksIndicies()
        {
            playerStateManager.coreController.weaponController.ResetAttacksIndices();
        }

        public void SetComboIndex(int index)
        {
            playerStateManager.coreController.weaponController.SetComboIndex(index);
        }

        public void EndAttackAnimation()
        {
            playerStateManager.coreController.animationManager.EndAttackAnimation();
        }

        public void StartAttackAnimation(string attackName)
        {
            playerStateManager.coreController.animationManager.StartAttackAnimation(attackName);
        }

        public void OnAttackStopForceTrigger()
        {
            playerStateManager.coreController.animationTriggers.OnAttackStopForceTrigger();
        }

        public void CheckMeleeEntryState()
        {
            if (playerStateManager.coreController.inputController.GetAttackInput() > 0)
            {
                if (playerStateManager.coreController.weaponController.HasWeapon())
                {
                    ItemType itemType = playerStateManager.coreController.weaponController.GetWeaponType();

                    if (itemType != ItemType.None)
                    {
                        Debug.Log("Item type not null");

                        if (itemType == ItemType.Sword)
                        {
                            Debug.Log("Initiating sword attack");

                            stateMachine.SetNextState(new MeleeEntryState());
                        }
                    }
                }
            }
        }
    }
}
