using UnityEngine;

namespace Interaction
{
    public class LeverController : MonoBehaviour
    {
        [SerializeField]
        private Sprite disableSprite;

        private SpriteRenderer _sprRenderer;

        private LeverStates defaultLeverState = LeverStates.Disable;

        private enum LeverStates
        {
            Disable = 0,
            Enable = 1
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
