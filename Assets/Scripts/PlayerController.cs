using System;
using Cinemachine;
using UnityEngine;
using Utils.FactoryTool;

public class PlayerController : PoolableMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private float _maxMoveSpeed;
    private float _acceleration;
    private bool _isInitialized = false;
    private PlayerModel _model;
    private CinemachineVirtualCamera _virtualCamera;
    private float _joystickHorizontal;
    private float _joystickVertical;

    
    
    private float _moveSpeed = 0.0f;
    private static readonly int _speed = Animator.StringToHash("Speed");
    private static readonly int _isFailed = Animator.StringToHash("isFailed");
    private static readonly int _gotHit = Animator.StringToHash("gotHit");

    public bool isDead { get; private set; }
    
    public event Action onDeath;
    public event Action<int> onPickup;
    
    public void SetupParameters(float maxMoveSpeed, float acceleration, int maxHealth, CameraController virtualCamera)
    {
        _maxMoveSpeed = maxMoveSpeed;
        _acceleration = acceleration;
        _virtualCamera = virtualCamera.camera;
        _model = new PlayerModel(maxHealth);
        isDead = false;
        _isInitialized = true;
    }

    public void TakeDamage(int damage)
    {
        if(isDead) return;
        _animator.SetTrigger(_gotHit);
        _model.TakeDamage(damage);
        
        if (_model.currentHealth <= 0)
        {
            isDead = true;
            _animator.SetBool(_isFailed, true);
            onDeath?.Invoke();
        }
    }

    public void SetInput(Vector2 input)
    {
        _joystickHorizontal = input.x;
        _joystickVertical = input.y;
    }

    private void Update()
    {
        if(isDead) return;
        if(!_isInitialized) return;
        
        //float horizontal = //Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_joystickHorizontal, 0, _joystickVertical);

        direction = _virtualCamera.transform.TransformDirection(direction);
        direction.y = 0.0f;
        direction.Normalize();
        
        if (direction.magnitude > 0.0f)
        {
            _moveSpeed += _acceleration * Time.deltaTime;
            _moveSpeed = Mathf.Clamp(_moveSpeed, 0, _maxMoveSpeed);
            _rigidbody.velocity = direction * _moveSpeed;
        }
        else
        {
            _moveSpeed = 0.0f;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
        
        _animator.SetFloat(_speed, _moveSpeed / _maxMoveSpeed);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
