using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{

    void Start()
    {
        this.transform.SetParent(null);
        DontDestroyOnLoad(this.gameObject);        
    }

}
