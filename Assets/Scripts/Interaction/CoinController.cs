using UI;
using UnityEngine;

namespace Interaction
{
    public class CoinController : InteractionWithAnimationController
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            string triggerObjTag = other.tag;
            if (triggerObjTag.Equals("Player"))
            {
                CurrentState = InteractionStates.Enable;
                UIScoreBar.Instance.SetBarValue();
            }
        }

    }

}