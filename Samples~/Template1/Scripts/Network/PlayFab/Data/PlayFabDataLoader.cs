﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabDataLoader<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                else
                {
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    protected void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


}