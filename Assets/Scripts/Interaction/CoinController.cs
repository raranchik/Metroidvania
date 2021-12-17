using UI;
using UnityEngine;

namespace Interaction
{
    public class CoinController : InteractionBaseController
    {
        private BoxCollider2D _boxCol2D;
        private Animator _animator;
        public override InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                if (value is InteractionStates.Enable)
                {
                    _boxCol2D.enabled = false;
                    _animator.SetInteger("AnimationState", (int) InteractionStates.Enable);
                    _currentState = value;
                }
            }
        }

        private void Awake()
        {
            _boxCol2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            string triggerObjTag = other.tag;
            if (triggerObjTag.Equals("Player"))
            {
                CurrentState = InteractionStates.Enable;
                UIScoreBar.Instance.SetBarValue();
            }
        }

    }

}