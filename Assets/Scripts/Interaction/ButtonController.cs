using UnityEngine;

namespace Interaction
{
    public class ButtonController : InteractionObjectAssociation
    {
        private readonly Vector2 _buttonEnableBoxColliderSize = new Vector2(0.8f, 0.16f);
        private readonly Vector2 _buttonEnableBoxColliderOffset = new Vector2(0f, 0.084f);
        private readonly Vector2 _buttonDisableBoxColliderSize = new Vector2(0.8f, 0.25f);
        private readonly Vector2 _buttonDisableColliderOffset = new Vector2(0f, 0.127f);

        private float _timer;

        private void Update()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    CurrentState = InteractionStates.Disable;
                    _collider.size = _buttonDisableBoxColliderSize;
                    _collider.offset = _buttonDisableColliderOffset;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            GameObject collisionObject = col.gameObject;
            if (!ThisObjectCanPress(collisionObject))
                return;

            if (CurrentState == InteractionStates.Enable)
                return;

            CurrentState = InteractionStates.Enable;
            _collider.size = _buttonEnableBoxColliderSize;
            _collider.offset = _buttonEnableBoxColliderOffset;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            GameObject collisionObject = collision.gameObject;
            if (ThisObjectCanPress(collisionObject))
                _timer = 0.5f;
        }

        private bool ThisObjectCanPress(in GameObject objInspected)
        {
            string objTag = objInspected.tag;
            return !objTag.Equals("Stone") || !objTag.Equals("Player");
        }

    }

}