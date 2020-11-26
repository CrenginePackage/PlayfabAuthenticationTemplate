using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Crengine.Ai.FSM
{
    public abstract class State : MonoBehaviour
    {
        public void Start()
        {
            OnStart();
        }
        public void Update()
        {
            OnUpdate();
        }
        public void OnDestroy()
        {
            OnEnd();
        }
        public abstract void OnStart();

        public abstract void OnUpdate();

        public abstract void OnEnd();

        public virtual T ChangeState<T>() where T:State
        {
            var FSM = GetComponent<FiniteStateMechine>();
            T nextState = FSM.gameObject.AddComponent<T>();
            Destroy(this);
            return nextState;
        }
    }


}
