using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        rb.linearVelocity = moveInput * moveSpeed;
        PlayerAppearance.Instance.playerFlipAction?.Invoke(Mathf.RoundToInt(moveInput.x));
        if (moveInput != Vector2.zero)
        {
            PlayerAppearance.Instance.playerAnimationAction?.Invoke(EntityMoves.Walk);
        }
        else
        {
            PlayerAppearance.Instance.playerAnimationAction?.Invoke(EntityMoves.Idle);
        }
    }
}
