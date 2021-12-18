using Characters.Herochar;
using UnityEngine;

namespace Interaction
{
    public class SavePointController : InteractionWithAnimationController, ITriggeredInteraction
    {
        private HerocharController _herochar;
        private Transform _transform;

        protected override void Awake()
        {
            base.Awake();
            _transform = GetComponent<Transform>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals("Player"))
            {
                CurrentState = InteractionStates.Enable;
                _herochar = col.GetComponent<HerocharController>();
            }
        }

        public void OnInteractionTriggered()
        {
            Vector2 savePointPos = _transform.position;
            _herochar.transform.position = savePointPos;
        }

    }

}