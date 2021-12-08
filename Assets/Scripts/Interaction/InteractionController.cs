using System;
using UnityEngine;

namespace Interaction
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField]
        private Sprite enableSprite;
        [SerializeField]
        private GameObject associatedWith;

        private SpriteRenderer _sprRenderer;
        private BoxCollider2D _boxCol2D;

        private InteractionStates _currentState = InteractionStates.Disable;

        public InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                _boxCol2D.enabled = false;
                _sprRenderer.sprite = enableSprite;
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
        }
    }
}
