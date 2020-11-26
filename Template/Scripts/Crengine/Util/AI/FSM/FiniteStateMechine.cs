using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Crengine.Ai.FSM
{
    public abstract class FiniteStateMechine : MonoBehaviour
    {
        public State TargetState;

        protected abstract void SetFirstState();
        
        
        private void Start()
        {
            SetFirstState();
        }
    }

}
