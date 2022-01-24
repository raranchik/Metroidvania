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
                _currentState = value;
                if (value is InteractionStates.Enable)
                {
                    _collider.enabled = false;
                    _animator.SetInteger("AnimationState", (int) InteractionStates.Enable);
                }
                else if (value is InteractionStates.Disable)
                {
                    _collider.enabled = true;
                    _animator.SetInteger("AnimationState", (int) InteractionStates.Disable);
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