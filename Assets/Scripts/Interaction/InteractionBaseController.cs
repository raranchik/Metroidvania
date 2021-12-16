using System;
using UnityEngine;

namespace Interaction
{
    public abstract class InteractionBaseController : MonoBehaviour
    {
        protected InteractionStates _currentState;

        public abstract InteractionStates CurrentState { get; set; }

        public enum InteractionStates
        {
            Disable = 0,
            Enable = 1
        }

    }

}