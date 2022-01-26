using Characters.Herochar;
using UnityEngine;

namespace Interaction.Checkpoint
{
    public class SavePointController : CheckpointBaseController
    {
        private BoxCollider2D _collider;
        private Animator _animator;

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

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.name.Contains("Herochar"))
            {
                CurrentState = InteractionStates.Enable;
                _herochar = col.GetComponent<HerocharController>();
                _herochar.DeathEvent.RemoveAllListeners();
                _herochar.DeathEvent.AddListener(OnInteractionTriggered);
            }
        }

    }

}