using System;
using Cinemachine;
using Configs;
using UnityEngine;
using Utils.FactoryTool;

public class PlayerController : PoolableMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private float _maxMoveSpeed;
    private float _acceleration;
    private PlayerModel _model;
    private CinemachineVirtualCamera _virtualCamera;

    
    
    private float _moveSpeed = 0.0f;
    private static readonly int _speed = Animator.StringToHash("Speed");
    private static readonly int _isFailed = Animator.StringToHash("isFailed");
    private static readonly int _gotHit = Animator.StringToHash("gotHit");

    public bool isDead { get; private set; }
    public event Action onDeath;
    
    public void SetupParameters(float maxMoveSpeed, float acceleration, int maxHealth, CameraController virtualCamera)
    {
        _maxMoveSpeed = maxMoveSpeed;
        _acceleration = acceleration;
        _virtualCamera = virtualCamera.camera;
        _model = new PlayerModel(maxHealth);
        isDead = false;
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

    private void Update()
    {
        if(isDead) return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);

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
