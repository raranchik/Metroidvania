using Characters.Herochar;
using UnityEngine;

namespace Interaction.Checkpoint
{
    public abstract class CheckpointBaseController : InteractionBaseController, ITriggeredInteraction
    {
        protected Transform _transform;
        protected HerocharController _herochar;

        protected virtual void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public virtual void OnInteractionTriggered()
        {
            Vector2 savePointPos = _transform.position;
            _herochar.transform.position = savePointPos;
            _herochar.IsRespawn = true;
        }

    }

}