using Control;
using UI;
using UnityEngine;

namespace Interaction
{
    public class CoinController : InteractionWithAnimationController
    {
        protected override void Awake()
        {
            base.Awake();
            UIScoreBar.TotalPossibleScore++;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            string triggerObjName = other.name;
            if (triggerObjName.Contains("Herochar"))
            {
                CurrentState = InteractionStates.Enable;
                UIScoreBar.Instance.SetBarValue();
            }
        }

    }

}