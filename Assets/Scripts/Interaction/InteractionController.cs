using System;
using UnityEngine;

namespace Interaction
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField]
        private Sprite _enableSprite;
        [SerializeField]
        private GameObject _associatedWith;
        [SerializeField]
        private bool _hasAnimation = false;

        private SpriteRenderer _sprRenderer;
        private BoxCollider2D _boxCol2D;
        private Animator _animator;

        private InteractionStates _currentState = InteractionStates.Disable;

        public InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                if (value is InteractionStates.Enable)
                {
                    _boxCol2D.enabled = false;
                }

                if (_hasAnimation)
                {
                    _animator.SetInteger("AnimationState", (int) InteractionStates.Enable);
                }
                else
                {
                    _sprRenderer.sprite = _enableSprite;
                }

                _currentState = value;
            }
        }

        public enum InteractionStates
        {
            Disable = 0,
            Enable = 1
        }

        private void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
            _boxCol2D = GetComponent<BoxCollider2D>();

            if (_hasAnimation)
            {
                _animator = GetComponent<Animator>();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasAnimation && _currentState is InteractionStates.Disable)
            {
                Debug.Log("wewe");
                string triggerObjTag = other.tag;
                if (triggerObjTag.Equals("Player"))
                {
                    CurrentState = InteractionStates.Enable;
                }
            }
        }

    }

}
