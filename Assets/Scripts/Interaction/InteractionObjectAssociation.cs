using UnityEngine;

namespace Interaction
{
    public abstract class InteractionObjectAssociation : InteractionBaseController
    {
        [SerializeField]
        protected Sprite _enableSprite;
        [SerializeField]
        protected Sprite _disableSprite;
        [SerializeField]
        protected InteractionBaseController _associatedWith;

        protected SpriteRenderer _spriteRenderer;
        protected BoxCollider2D _collider;

        public override InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;

                if (value is InteractionStates.Enable)
                {
                    _spriteRenderer.sprite = _enableSprite;
                    _associatedWith.CurrentState = InteractionStates.Enable;
                }
                else if (value is InteractionStates.Disable)
                {
                    _spriteRenderer.sprite = _disableSprite;
                    _associatedWith.CurrentState = InteractionStates.Disable;
                }
            }
        }

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();

            _currentState = InteractionStates.Disable;
        }

    }

}