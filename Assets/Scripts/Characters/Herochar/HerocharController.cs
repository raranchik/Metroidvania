using System;
using UnityEngine;
using System.Collections;
using Control;
using Interaction;
using UI;

namespace Characters.Herochar
{
    public class HerocharController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 3.5f;
        [SerializeField]
        private float _jumpHeight = 5f;
        [SerializeField]
        private int _maxHealth = 5;
        [SerializeField]
        private float _timeImmortality = 1.0f;
        [SerializeField]
        private float _dashPower = 7f;
        [SerializeField]
        private float _dashTimeReload = 1.0f;
        [SerializeField]
        private int _maxCountLife = 3;
        
        private float _dirX;
        private bool _onFloor;
        private bool _doJump;
        private bool _doDash;
        private bool _canDash = true;
        private bool _isImmortal;
        private bool _switchesLever = false;
        private bool _isPushingForward = false;
        private Vector2 _direction = Vector2.right;
        private int _currentHealth;
        private int _currentCountLife;

        private Transform _transform;
        private Rigidbody2D _rb;
        private SpriteRenderer _sprRenderer;
        private Animator _animator;
        private Transform _handForInteractions;
        private CapsuleCollider2D _capsuleCol;

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
        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _sprRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _capsuleCol = GetComponent<CapsuleCollider2D>();
            _animator = GetComponent<Animator>();

            _handForInteractions = transform.Find("Hand").transform;

            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            UIHealthBar.Instance.SetBarMaxValue(_maxHealth);
            UILifeBar.Instance.SetBarValue(_maxCountLife);
            UIScoreBar.Instance.SetInitialValue();
        }

        // Update is called once per frame
        private void Update()
        {
            if (PauseControl.GameIsPaused()) return;

            _dirX = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
                _doDash = true;

            if (((Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0) | Input.GetButtonDown("Jump")) && _onFloor)
                _doJump = true;

            if (Input.GetKey(KeyCode.V) && _onFloor)
                _switchesLever = true;

            UpdateAnimationState();
        }

        private void FixedUpdate()
        {
            if (PauseControl.GameIsPaused()) return;

            CheckGround();

            OnMove();
            if (_switchesLever)
                OnLeverSwitch();
            if (_doDash)
                OnDash();
            if (_doJump)
                OnJump();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            // if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Traps")))
            //     OnHit();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.tag.Equals("Stone"))
            {
                Bounds bounds = _capsuleCol.bounds;
                Vector2 rayPoint = bounds.center;
                Vector2 offsetRayPoint = _direction.Equals(Vector2.right) ? new Vector2(bounds.extents.x, 0f) : new Vector2(-bounds.extents.x, 0f);
                RaycastHit2D hit = Physics2D.Raycast(rayPoint + offsetRayPoint, _direction, .1f, LayerMask.GetMask("Interaction"));
                _isPushingForward = hit.collider != null && _dirX != 0f && _onFloor;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.tag.Equals("Stone"))
                _isPushingForward = false;
        }

        private void CheckGround()
        {
            Physics2D.queriesHitTriggers = false;
            Vector2 position = _transform.position;
            Vector2 colliderSize = _capsuleCol.size;

            Collider2D[] colliders = Physics2D.OverlapBoxAll(position, new Vector2(colliderSize.x - .2f, .1f), 0f);
            foreach (Collider2D collider in colliders)
            {
                int layer = collider.gameObject.layer;
                if (GetFloorLayers().Contains(layer))
                {
                    Physics2D.queriesHitTriggers = _onFloor = true;
                    return;
                }
            }

            _onFloor = false;
        }

        private static ArrayList GetFloorLayers()
        {
            ArrayList floorLayers = new ArrayList();
            floorLayers.AddRange(new int[]
            {
                LayerMask.NameToLayer("Interaction"),
                LayerMask.NameToLayer("Floor")
            });

            return floorLayers;
        }

        private void OnMove()
        {
            
            Vector2 velocity = _rb.velocity;
            AdjustVelocity(_dirX * _speed, velocity.y);
        }

        private void OnJump()
        {
            _rb.AddForce(new Vector2(0, _jumpHeight), ForceMode2D.Impulse);
            _doJump = false;
        }

        private void OnDash()
        {
            AdjustVelocity();
            _rb.AddForce(_direction * _dashPower, ForceMode2D.Impulse);

            RaycastHit2D hit = GetRayHitByLayerName("Interaction");
            if (hit.collider != null)
            {
                GameObject interactionObj = hit.collider.gameObject;
                Vector2 velocity = _rb.velocity;
                if (interactionObj.tag.Equals("Stone")) AdjustVelocity(0f, velocity.y);
            }

            _doDash = false;
            StartCoroutine(DashReload());
        }

        private void OnLeverSwitch()
        {
            AdjustVelocity();

            RaycastHit2D hit = GetRayHitByLayerName("Interaction");
            if (hit.collider != null)
            {
                GameObject interactionObj = hit.collider.gameObject;
                if (interactionObj.tag.Equals("Lever"))
                {
                    Debug.Log(interactionObj.name);
                    var leverController = interactionObj.GetComponent<LevelerController>();
                    leverController.CurrentState = InteractionBaseController.InteractionStates.Enable;
                }
            }

            _switchesLever = false;
        }

        public void OnHit()
        {
            if (_isImmortal) return;

            _currentHealth -= 1;
            if (_currentHealth == 0)
            {
                // Death();
            }
            UIHealthBar.Instance.SetBarValue(_currentHealth);

            Debug.Log(_currentHealth);
            StartCoroutine(Immortality());
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }

        private void UpdateAnimationState()
        {
            AnimationStates state = AnimationStates.Idle;

            if (_dirX < 0)
            {
                _direction = Vector2.left;
                _transform.localScale = new Vector3(-1, 1);
                state = AnimationStates.Run;
            }
            else if (_dirX > 0)
            {
                _direction = Vector2.right;
                _transform.localScale = new Vector3(1, 1);
                state = AnimationStates.Run;
            }

            if (_rb.velocity.y > .1f) state = AnimationStates.Jump;
            else if (_rb.velocity.y < -.1f) state = AnimationStates.Falling;

            if (_isImmortal) state = AnimationStates.Hit;

            if (_switchesLever) state = AnimationStates.Attack1;

            if (_isPushingForward) state = AnimationStates.PushingForward;

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

        private RaycastHit2D GetRayHitByLayerName(string layerName, float distance = 1.1f, bool checkBottom = false)
        {
            Vector2 inspectionPointPos = _handForInteractions.position;
            Vector2 dir = _direction;
            return Physics2D.BoxCast(inspectionPointPos, Vector2.one, 0f, dir, .1f, LayerMask.GetMask(layerName));
        }

        private void AdjustVelocity(in float x = 0f, in float y = 0f)
        {
            Vector2 newVelocity = new Vector2(x, y);
            _rb.velocity = newVelocity;
        }

    }

}
