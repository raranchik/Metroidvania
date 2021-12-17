using UnityEngine;

namespace Interaction.Traps
{
    public class SuspendedController : TrapBaseController
    {
        [SerializeField]
        private PolygonCollider2D[] _collider2Ds;

        private int _currеntColliderNum;

        private void SetColliderForAnimation(int numCollider = 0)
        {
            _collider2Ds[_currеntColliderNum].enabled = false;
            _currеntColliderNum = numCollider;
            _collider2Ds[_currеntColliderNum].enabled = true;
        }

        public override InteractionStates CurrentState { get; set; }

    }

}