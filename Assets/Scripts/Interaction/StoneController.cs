using UnityEngine;

namespace Interaction
{
    public class StoneController : MonoBehaviour
    {
        [SerializeField]
        private float distanceToBreakingPoint;

        private Transform _transform;

        private Vector2 _originPoint;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            _originPoint = _transform.position;
        }

        private void Update()
        {
            if (distanceToBreakingPoint == 0f)
                return;

            Vector2 currentPos = _transform.position;
            float distanceFromOriginPoint = Vector2.Distance(currentPos, _originPoint);
            if (distanceFromOriginPoint > distanceToBreakingPoint)
                _transform.position = _originPoint;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            GameObject collisionObject = col.gameObject;
            string objName = collisionObject.name;
            if (!objName.Equals("Button"))
                return;

            Vector2 stonePosition = _transform.position;

            ContactPoint2D[] contactPoints = col.contacts;
            float maxPointHeight = GetMaxPointHeight(contactPoints);

            Vector2 dir = GetDirection(collisionObject.transform.position, stonePosition);

            _transform.position = new Vector3(stonePosition.x + 0.02f * dir.x, maxPointHeight);
        }

        private Vector2 GetDirection(in Vector2 buttonPos, in Vector2 stonePos)
        {
            return (buttonPos - stonePos).normalized;
        }

        private float GetMaxPointHeight(in ContactPoint2D[] contactPoints)
        {
            float maxPointHeight = float.MinValue;
            foreach (var contactPoint in contactPoints)
            {
                float pointHeight = contactPoint.point.y;
                if (maxPointHeight < pointHeight)
                    maxPointHeight = pointHeight;
            }

            return maxPointHeight;
        }

    }

}
