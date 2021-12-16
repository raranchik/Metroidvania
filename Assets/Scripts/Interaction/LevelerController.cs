using System;
using UnityEngine;

namespace Interaction
{
    public class LevelerController : InteractionBaseController
    {
        [SerializeField]
        private Sprite _enableSprite;
        [SerializeField]
        private InteractionBaseController _associatedWith;

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCol2D;

        public override InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                if (value is InteractionStates.Enable)
                {
                    _boxCol2D.enabled = false;
                    _spriteRenderer.sprite = _enableSprite;
                    _currentState = value;
                }
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCol2D = GetComponent<BoxCollider2D>();
        }

    }

}