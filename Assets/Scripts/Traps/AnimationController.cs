using UnityEngine;

namespace Traps
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField]
        private PolygonCollider2D[] _collider2Ds;

        private int _currеntColliderNum;
        
        public void SetColliderForAnimation(int numCollider = 0)
        {
            _collider2Ds[_currеntColliderNum].enabled = false;
            _currеntColliderNum = numCollider;
            _collider2Ds[_currеntColliderNum].enabled = true;
        }
    }
}
