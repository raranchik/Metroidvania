using System;
using UnityEngine;

namespace Interaction
{
    public abstract class InteractionWithAnimationController : InteractionBaseController
    {
        protected Animator _animator;
        protected Collider2D _collider;

        public override InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                if (value is InteractionStates.Enable)
                {
                    _collider.enabled = false;
                    _animator.SetInteger("AnimationState", (int) InteractionStates.Enable);
                    _currentState = value;
                }
            }
        }

        protected virtual void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
        }

    }

}