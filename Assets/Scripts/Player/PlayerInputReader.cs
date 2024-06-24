using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
   private void OnMove(InputValue value)
   {
        this.Move(value.Get<Vector2>());
   }
   private void OnAttack(InputValue value)
   {
        this.Attack();
   }
   private void OnDodge(InputValue value)
   {
        this.Dodge();
   }
}
