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
        private PolygonCollider2D _polyCol2d;

        private InteractionStates _currentState = InteractionStates.Disable;

        public InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                _polyCol2d.enabled = false;
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
            _polyCol2d = GetComponent<PolygonCollider2D>();
        }
    }
}
