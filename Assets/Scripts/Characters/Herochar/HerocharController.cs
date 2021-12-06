using UnityEngine;
using System.Collections;

namespace Characters.Herochar
{
    public class HerocharController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 3.5f;
        [SerializeField]
        private float _jumpHeight = 5f;
        [SerializeField]
        private int _health = 5;
        [SerializeField]
        private float _timeImmortality = 1.0f;
        [SerializeField]
        private float _dashPower = 7f;
        [SerializeField]
        private float _dashTimeReload = 1.0f;
        
        private float _dirX;
        private bool _onFloor;
        private bool _doJump;
        private bool _doDash;
        private bool _canDash = true;
        private bool _isImmortal;
        private bool _lookRight = true;
        private bool _switchesLever = false;

        private Transform _transform;
        private Rigidbody2D _rb;
        private SpriteRenderer _sprRenderer;
        private Animator _animator;
        private BoxCollider2D _interactionCollider;

        private enum AnimationStates
        {
            Idle = 0,
            Run = 1,
            Jump = 2,
            JumpDouble = 3,
            Falling = 4,
            Landing = 5,
            Hit = 6,
            PushingForward = 7,
            Attack1 = 8,
            Attack2Sword = 9,
            Death = 10,
            Dash = 11
        }

        // Start is called before the first frame update
        void Start()
        {
            _transform = GetComponent<Transform>();
            _sprRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _interactionCollider = _transform.Find("Interaction").GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            _dirX = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
                _doDash = true;

            if (((Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0) | Input.GetButtonDown("Jump")) && _onFloor)
                _doJump = true;

            if (Input.GetKey(KeyCode.V))
                _switchesLever = true;

            UpdateAnimationState();
        }

        private void FixedUpdate()
        {
            Move();

            if (_doDash)
                Dash();

            if (_doJump && _onFloor)
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

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Traps")))
                Hit();
        }

        private void Move()
        {
            Vector2 velocity = _rb.velocity;
            _rb.velocity = new Vector2(_dirX * _speed, velocity.y);
        }

        private void Jump()
        {
            _rb.AddForce(new Vector2(0, _jumpHeight), ForceMode2D.Impulse);
            _doJump = false;
        }

        private void Dash()
        {
            _doDash = false;
            _rb.velocity = Vector2.zero;

            Vector2 dirVector = _lookRight ? Vector2.right : Vector2.left;
            _rb.AddForce(dirVector * _dashPower, ForceMode2D.Impulse);
            StartCoroutine(DashReload());
        }

        private void LeverSwitch()
        {
            // _switchesLever = false;
        }

        private void Hit()
        {
            if (_isImmortal) return;

            _health -= 1;
            if (_health == 0)
                Death();

            Debug.Log(_health);
            StartCoroutine(Immortality());
        }

        private void Death()
        {
            Destroy(gameObject);
        }

        private void UpdateAnimationState()
        {
            AnimationStates state = AnimationStates.Idle;

            if (_dirX < 0)
            {
                _lookRight = false;
                _sprRenderer.flipX = !_lookRight;
                state = AnimationStates.Run;
            }
            else if (_dirX > 0)
            {
                _lookRight = true;
                _sprRenderer.flipX = !_lookRight;
                state = AnimationStates.Run;
            }

            if (_rb.velocity.y > .1f) state = AnimationStates.Jump;
            else if (_rb.velocity.y < -.1f) state = AnimationStates.Falling;

            if (_isImmortal) state = AnimationStates.Hit;

            if (_switchesLever)
            {
                state = AnimationStates.Attack1;
                _switchesLever = false;
            }

            Debug.Log(state);
            _animator.SetInteger("AnimationState", (int) state);
        }

        private IEnumerator Immortality()
        {
            _isImmortal = true;
            yield return new WaitForSeconds(_timeImmortality);
            _isImmortal = false;
        }

        private IEnumerator DashReload()
        {
            _canDash = false;
            yield return new WaitForSeconds(_dashTimeReload);
            _canDash = true;
        }
    }
}
