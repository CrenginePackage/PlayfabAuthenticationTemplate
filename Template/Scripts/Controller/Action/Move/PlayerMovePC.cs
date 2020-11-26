using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovePC : PlayerMove
{
    [SerializeField] private KeyCode jetpackToggleKey;

    protected override Vector3 ConvertInputDirection(Vector3 input)
    {
        //return transform.right * input.x + transform.forward * input.z + transform.up * input.y;
        return transform.right * input.x + transform.forward * input.y + transform.up * input.z;
    }

    protected override Vector3 GetInputDirection()
    {
        //return (Input.GetAxis("Horizontal") * Vector3.right + Input.GetAxis("Vertical") * Vector3.forward).normalized;
        return Vector3.zero;
    }

    protected override bool IsJetpackDownPressed()
    {
        //return Input.GetKey(KeyCode.Q);
        return false;
    }

    protected override bool IsJetpackToggle()
    {
        //return Input.GetKeyDown(jetpackToggleKey);
        return false;
    }

    protected override bool IsJetpackUpPressed()
    {
        //return Input.GetKey(KeyCode.E);
        return false;
    }

    protected override bool IsCrouchToggle()
    {
        //return Input.GetKeyDown(KeyCode.V);
        return false;
    }
}
