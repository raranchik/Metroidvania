using Control;
using UnityEngine;

namespace Interaction.Checkpoint
{
    public class ExitPointController : InteractionBaseController
    {
        public override InteractionStates CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                if (value.Equals(InteractionStates.Enable))
                {
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    GameControl.Instance.gameEndEvent.Invoke("Win");
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject obj = col.gameObject;
            string gameObjectName = obj.name;
            if (gameObjectName.Contains("Herochar"))
            {
                CurrentState = InteractionStates.Enable;
            }
        }
    }

}