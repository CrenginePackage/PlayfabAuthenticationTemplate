using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected PlayerMove playerMove;
    public Animator animator { get; set; }

    protected FootIKHumanoid footIKHumanoid;
    //[SerializeField] protected Emotion emotion;

    [SerializeField] protected bool blockrotationPlayer = false;
    [Range(0, 1f)]
    [SerializeField] protected float horizontalAnimationSmoothness = 0.2f;
    [Range(0, 1f)]
    [SerializeField] protected float verticalAnimationSmoothness = 0.2f;
    [Range(0, 1f)]
    [SerializeField] protected float rotationDamp = 0.1f;
    [Range(0, 1f)]
    [SerializeField] protected float rotationSpeed = 0.1f;
    [Range(0, 1f)]
    [SerializeField] protected float startAnimationTime = 0.3f;

    protected float moveMagnitude;
    protected Vector3 moveDirection;

    public virtual void SetAnimator(Animator _animator)
    {
        footIKHumanoid = _animator.GetComponent<FootIKHumanoid>();

        animator = _animator;
        footIKHumanoid.animator = _animator;
        //emotion.animator = _animator;
    }

    protected void LateUpdate()
    {
        if (animator == null) return;

        InputMagnitude(playerMove.inputDirection.x, playerMove.inputDirection.z, playerMove.moveSpeed);
    }

    public virtual void InputMagnitude(float _inputX, float _inputZ, float _moveSpeed)
    {
        animator.SetFloat("horizontal", _inputX, horizontalAnimationSmoothness, Time.deltaTime);
        animator.SetFloat("vertical", _inputZ, verticalAnimationSmoothness, Time.deltaTime);

        moveMagnitude = new Vector2(_inputX, _inputZ).sqrMagnitude;
        moveMagnitude = Mathf.Clamp(moveMagnitude, 0, 1f);

        if (moveMagnitude > rotationDamp)
        {
            animator.SetFloat("inputMagnitude", moveMagnitude, startAnimationTime, Time.deltaTime);
            MoveAndRotate(_inputX, _inputZ, _moveSpeed);
            return;
        }
        animator.SetFloat("inputMagnitude", moveMagnitude, startAnimationTime, Time.deltaTime);
    }

    private void MoveAndRotate(float _inputX, float _inputZ, float _moveSpeed)
    {
        var forward = playerCamera.transform.forward;
        var right = playerCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * _inputZ + right * _inputX;

        if (blockrotationPlayer == false)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);
    }
}
