using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Crengine;

#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

public abstract class PlayerMove : MonoBehaviour
{
    // Base Components
    [Header("Base Components")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] Transform orignalParent;
    public CharacterController playerController;
    public Camera playerCamera;
    [SerializeField] private UIAlarmComment uiAlarmComment;

    // Movement
    [Header("Player Movement")]
    public float moveSpeed = 3f;
    public float moveGravity = -9.81f;
    public Vector3 inputDirection { get; set; }
    protected Vector3 _velocity;

    public bool isHold { get; set; } = false;
    public bool disableInputDirection { get; set; } = false;
    public bool disableMove { get; set; }

    [Header("Player Ground Check")]
    public Transform GroundCheck;
    public float GroundDistance = .1f;
    public LayerMask GroundMask;

    protected bool _isGrounded;
    protected bool _isGravityFree = false;

    [Header("Player Crouch")]
    [Range(0,1)]
    public float crouchSpeedRatio = .5f;
    public bool isCrouching { get; set; }

    // Crouch
    [HideInInspector] public bool _isCrouch = false;
    [HideInInspector] public Animator animator;

    // Block locomote while boarding
    //public PlayerBoard playerBoard;

    protected virtual void Update()
    {
        if (disableMove) return;

        if (isHold) return;

        var move = playerInput.actions["move"].ReadValue<Vector2>();
        var look = playerInput.actions["look"].ReadValue<Vector2>();

        AddGravity();

        GetMovement(move);

        AddJump(); 

        AddJetpack();

        AddCrouch();
    }

    public void SetLocation(Vector3 _position, Quaternion _rotation)
    {
        isHold = true;
        inputDirection = Vector3.zero;

        playerController.transform.position = _position;
        playerController.transform.rotation = _rotation;

        playerController.detectCollisions = false;
        _isGravityFree = true;
    }

    public void ReturnToParent(Vector3 _position, Quaternion _rotation)
    {
        isHold = false;
        inputDirection = Vector3.zero;

        playerController.transform.position = _position;
        playerController.transform.rotation = _rotation;

        playerController.detectCollisions = true;
        _isGravityFree = false;
    }

    private void SetParent(Transform _parentObject)
    {
        SetParent(_parentObject, _parentObject.position, _parentObject.rotation);
    }

    private void SetParent(Transform _parentObject, Vector3 _position, Quaternion _rotation)
    {
        playerController.transform.SetParent(_parentObject);
        playerController.transform.position = _position;
        playerController.transform.rotation = _rotation;
    }

    #region Movement

    protected void GetMovement(Vector2 _direction)
    {
        //if (playerBoard.isBoarding) return;

        //inputDirection = Vector3.zero;
        inputDirection = _direction;

        //if (!disableInputDirection)
        //{
        //    inputDirection = GetInputDirectionWithJetpack(GetInputDirection());
        //}

        //playerController.Move(ConvertInputDirection(inputDirection) * GetMovementSpeed() * (isCrouching ? crouchSpeedRatio : 1) * Time.deltaTime);
        playerController.Move(ConvertInputDirection(inputDirection) * moveSpeed * (isCrouching ? crouchSpeedRatio : 1) * Time.deltaTime);

    }

    protected float GetMovementSpeed()
    {
        return moveSpeed * PlayerPrefs.GetFloat("MoveSpeed", 0.8f);
    }

    protected void AddGravity()
    {
        _isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (_isGravityFree) return;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }

    protected void AddJump()
    {
        if (_isGravityFree) return;

        //if (IsJumpPressed() && _isGrounded)
        //{
        //    if (!disableInputDirection)
        //    {
        //        _velocity.y = Mathf.Sqrt(3f * -2f * moveGravity);
        //    }
        //}

        _velocity.y += moveGravity * Time.deltaTime;

        playerController.Move(_velocity * Time.deltaTime);
    }

    #endregion

    #region Jetpack
    protected void AddJetpack()
    {
        //if (playerBoard.isBoarding) return;

        if (IsJetpackToggle())
        {
            if (!disableInputDirection)
            {
                _isGravityFree = !_isGravityFree;
                if (_isGravityFree)
                {
                    OnJetpackActivated();
                }
                else
                {
                    OnJetpackDeactivated();
                }
            }
        }
    }

    protected virtual void OnJetpackActivated()
    {
        Debug.Log("Activated");

        //uiAlarmComment.OpenAlarm(LocalizeManager.instance.LocalizeScriptMessage("제트팩"),
        //    LocalizeManager.instance.LocalizeScriptMessage("제트팩이 활성화 되었습니다."));
    }

    protected virtual void OnJetpackDeactivated()
    {
        Debug.Log("Deactivated");

        //uiAlarmComment.OpenAlarm(LocalizeManager.instance.LocalizeScriptMessage("제트팩"),
        //    LocalizeManager.instance.LocalizeScriptMessage("제트팩 기능을 종료했습니다."));        
    }
    #endregion

    protected virtual void OnCrouchActivated()
    {
        Debug.Log("Crouch Start");
    }

    protected virtual void OnCrouchDeactivated()
    {
        Debug.Log("Crouch End");
    }


    protected abstract Vector3 GetInputDirection();

    protected abstract Vector3 ConvertInputDirection(Vector3 input);

    //protected abstract bool IsJumpPressed();

    protected Vector3 GetInputDirectionWithJetpack(Vector3 inputDirection)
    {
        return inputDirection + Vector3.up * (_isGravityFree ? ((IsJetpackUpPressed() ? 1 : 0) + (IsJetpackDownPressed() ? -1 : 0)) : 0);
    }

    protected abstract bool IsJetpackToggle();

    protected abstract bool IsJetpackUpPressed();

    protected abstract bool IsJetpackDownPressed();



    #region crouch

    protected void AddCrouch()
    {
        //if (playerBoard.isBoarding) return;
        if (IsCrouchToggle())
        {
            animator.ResetTrigger("Crouch");
            animator.ResetTrigger("Stand");
            _isCrouch = !_isCrouch;

            if (_isCrouch)
            {
                Crouchding();
            }
            else
            {
                Standing();
            }
        }
    }

    public void Crouchding()
    {
        animator.SetTrigger("Crouch");
        playerCamera.transform.localPosition = new Vector3(0, 0.75f, 0.6f);
    }

    public void Standing()
    {
        animator.SetTrigger("Stand");
        playerCamera.transform.localPosition = new Vector3(0, 1.5f, 0.25f);
    }

    protected abstract bool IsCrouchToggle();

    #endregion

}
