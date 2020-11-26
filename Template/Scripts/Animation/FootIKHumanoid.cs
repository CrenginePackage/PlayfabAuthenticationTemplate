using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIKHumanoid : MonoBehaviour
{
    public Animator animator { get; set; }

    private Vector3 leftFootPosition, rightFootPosition, leftFootIkPosition, rightFootIkPosition;
    private Quaternion leftFootIkRotation, rightFootIkRotation;
    private float lastPelvisPositionY, lastLeftFootPositionY, lastRightFootPositionY;

    [Header("Feet Grounder")]
    public bool enableFeetIK = true;
    [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 0.3f;
    [Range(0, 2)] [SerializeField] private float raycastDownDistance = 0.9f;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float pelvisOffset = 0f;
    [Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
    [Range(0, 1)] [SerializeField] private float feetToIkPositionSpeed = 0.5f;

    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

    public bool useProIkFeature = false;
    public bool showSolverDebug = true;


    //private void Start()
    //{
    //    animator = GetComponent<Animator>();
    //}

    #region FeetGrounding

    private void FixedUpdate()
    {
        if (enableFeetIK == false)
            return;
        if (animator == null)
            return;

        AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);
        AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);

        // find and raycast to the ground to find positions
        FeetPositionSolver(leftFootPosition, ref leftFootIkPosition, ref leftFootIkRotation); // handle the solver for left foot
        FeetPositionSolver(rightFootPosition, ref rightFootIkPosition, ref rightFootIkRotation); // handle the solver for right foot
    }
    

    private void OnAnimatorIK(int layerIndex)
    {
        if (enableFeetIK == false)
            return;
        if (animator == null)
            return;

        MovePelvisHeight();

        // right foot ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

        if (useProIkFeature)
        {
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(rightFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootIkPosition, rightFootIkRotation, ref lastRightFootPositionY);

        // left foot ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

        if (useProIkFeature)
        {
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(leftFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootIkPosition, leftFootIkRotation, ref lastLeftFootPositionY);
    }

    #endregion

    #region FeetGrondingMethods

    private void MoveFeetToIkPoint(AvatarIKGoal _foot, Vector3 _positionIkHolder, Quaternion _rotationIkHolder, ref float _lastFootPositionY)
    {
        Vector3 targetIkPosition = animator.GetIKPosition(_foot);

       
        if (_positionIkHolder != Vector3.zero)
        {
            targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
            _positionIkHolder = transform.InverseTransformPoint(_positionIkHolder);

            float yVariable = Mathf.Lerp(_lastFootPositionY, _positionIkHolder.y, feetToIkPositionSpeed);
            targetIkPosition.y += yVariable;

            _lastFootPositionY = yVariable;

            targetIkPosition = transform.TransformPoint(targetIkPosition);

            animator.SetIKRotation(_foot, _rotationIkHolder);
        }
        
        animator.SetIKPosition(_foot, targetIkPosition);
    }

    private void MovePelvisHeight()
    {
        if (rightFootIkPosition == Vector3.zero || leftFootIkPosition == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = animator.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = leftFootIkPosition.y - transform.position.y;
        float rightOffsetPosition = rightFootIkPosition.y - transform.position.y;

        float totaclOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totaclOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        animator.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = animator.bodyPosition.y;
    }

    private void FeetPositionSolver(Vector3 _fromSkyPosition, ref Vector3 _feetIkPositions, ref Quaternion _feetIkRotations)
    {
        RaycastHit feetOutHit;

        if (showSolverDebug)
            Debug.DrawLine(_fromSkyPosition, _fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);

        if (Physics.Raycast(_fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            _feetIkPositions = _fromSkyPosition;
            _feetIkPositions.y = feetOutHit.point.y + pelvisOffset;
            _feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

            return;
        }

        _feetIkPositions = Vector3.zero;
    }

    private void AdjustFeetTarget(ref Vector3 _feetPositions, HumanBodyBones _foot)
    {
        _feetPositions = animator.GetBoneTransform(_foot).position;
        _feetPositions.y = transform.position.y + heightFromGroundRaycast;
    }

    #endregion
}
