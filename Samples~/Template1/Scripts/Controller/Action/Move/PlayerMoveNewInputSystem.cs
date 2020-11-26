using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Crengine;

public class PlayerMoveNewInputSystem : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed;

    private void Update()
    {
        var moveDirection = playerInput.actions["move"].ReadValue<Vector2>();
        var lookDirection = playerInput.actions["look"].ReadValue<Vector2>();

        Move(moveDirection);
        Look(lookDirection);
    }

    private void Move(Vector2 _direction)
    {
        
    }

    private void Look(Vector2 _direction)
    {

    }
}