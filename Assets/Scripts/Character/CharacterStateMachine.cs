using MirAI.Animation;
using MirAI.FSM;
using UnityEngine;

namespace MirAI.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterStateMachine : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private SpriteAnimationPlayer _animationPlayer;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private string _idleClip = "Idle";
        [SerializeField] private string _runClip = "Run";
        [SerializeField] private string _jumpClip = "Jump";

        [Header("Movement")]
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.12f;
        [SerializeField] private LayerMask _groundMask;

        private readonly StateMachine _stateMachine = new();
        private IdleState _idleState;
        private RunState _runState;
        private JumpState _jumpState;

        private float _horizontalInput;
        private bool _jumpQueued;

        public float HorizontalInput => _horizontalInput;
        public bool IsGrounded => _groundCheck != null && Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);
        public SpriteAnimationPlayer AnimationPlayer => _animationPlayer;
        public string IdleClip => _idleClip;
        public string RunClip => _runClip;
        public string JumpClip => _jumpClip;
        public StateMachine Machine => _stateMachine;

        private void Awake()
        {
            if (_body == null)
            {
                _body = GetComponent<Rigidbody2D>();
            }

            if (_animationPlayer == null)
            {
                _animationPlayer = GetComponentInChildren<SpriteAnimationPlayer>();
            }

            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            _idleState = new IdleState(this);
            _runState = new RunState(this);
            _jumpState = new JumpState(this);
        }

        private void Start()
        {
            _stateMachine.ChangeState(_idleState);
        }

        private void Update()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                _jumpQueued = true;
            }

            _stateMachine.Tick();

            if (_spriteRenderer != null && Mathf.Abs(_horizontalInput) > 0.01f)
            {
                _spriteRenderer.flipX = _horizontalInput < 0f;
            }
        }

        private void FixedUpdate()
        {
            var velocity = _body.velocity;
            velocity.x = _horizontalInput * _moveSpeed;

            if (_jumpQueued && IsGrounded)
            {
                velocity.y = _jumpForce;
                _jumpQueued = false;
            }

            _body.velocity = velocity;
        }

        public void ChangeToIdle() => _stateMachine.ChangeState(_idleState);
        public void ChangeToRun() => _stateMachine.ChangeState(_runState);
        public void ChangeToJump() => _stateMachine.ChangeState(_jumpState);

        public bool ConsumeJumpQueued()
        {
            if (!_jumpQueued)
            {
                return false;
            }

            _jumpQueued = false;
            return true;
        }
    }
}
