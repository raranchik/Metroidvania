using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CharacterControllers
{
    public class BaseController : MonoBehaviour
    {
        private float _speed = 3.5f;
        private float _jumpHeight = 5f;
        private float _lookDir;
        private bool _facingRight;
        private bool _onFloor;
        private bool _doJump;

        private Transform _transform;
        private Rigidbody2D _rb;
        private SpriteRenderer _sprRenderer;

        // Start is called before the first frame update
        void Start()
        {
            _transform = GetComponent<Transform>();
            _sprRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            _lookDir = Input.GetAxisRaw("Horizontal");

            if (_lookDir < 0.0f && !_facingRight) Flip();
            else if (_lookDir > 0.0f && _facingRight) Flip();

            if (Input.GetButtonDown("Jump") && _onFloor)
                _doJump = true;
        }

        private void FixedUpdate()
        {
            Move();
            if (_doJump)
                Jump();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Floor")))
                _onFloor = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Floor")))
                _onFloor = false;
        }

        private void Move()
        {
            Vector2 velocity = _rb.velocity;
            _rb.velocity = new Vector2(_lookDir * _speed, velocity.y);
        }

        private void Jump()
        {
            if (_onFloor && _doJump)
            {
                _rb.AddForce(new Vector2(0, _jumpHeight), ForceMode2D.Impulse);
                _doJump = false;
            }
        }

        private void Flip()
        {
            _facingRight = !_facingRight;
            _sprRenderer.flipX = _facingRight;
        }
    }
}
