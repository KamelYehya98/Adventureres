using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public static class PlayerInputInvoker
    {
        #region Fields
        public static event Action<Vector2> PlayerMovement;
        public static event Action PlayerAttack;
        public static event Action playerDodge;
        #endregion

        #region Extension Methods
        public static void Move(this PlayerInputReader inputScript, Vector2 direction) => PlayerMovement?.Invoke(direction);
        public static void Attack(this PlayerInputReader inputScript) => PlayerAttack?.Invoke();
        public static void Dodge(this PlayerInputReader inputScript) => playerDodge?.Invoke();
        #endregion
    }

}
