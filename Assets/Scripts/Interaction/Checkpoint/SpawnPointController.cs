using Characters.Herochar;
using UnityEngine;

namespace Interaction.Checkpoint
{
    public class SpawnPointController : CheckpointBaseController
    {
        [SerializeField]
        private GameObject _herocharPrefab;

        public override InteractionStates CurrentState { get; set; }

        private void Start()
        {
            GameObject herocharObj = Instantiate(_herocharPrefab, _transform.position, _herocharPrefab.transform.rotation);
            _herochar = herocharObj.GetComponent<HerocharController>();
            _herochar.DeathEvent.AddListener(OnInteractionTriggered);
            _herochar.IsRespawn = true;
        }

    }

}