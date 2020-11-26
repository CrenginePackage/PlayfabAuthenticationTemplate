using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMenuPosition : MonoBehaviour
{
    [SerializeField] private Transform userScreenOffset;

    private void Update()
    {
        userScreenOffset.position = this.transform.position - Vector3.up * this.transform.position.y;
    }
}
