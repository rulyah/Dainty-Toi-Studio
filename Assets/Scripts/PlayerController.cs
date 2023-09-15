using Cinemachine;
using Configs;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private PlayerConfig _playerConfig;

    private float _maxMoveSpeed;
    private float _acceleration;
    private PlayerModel _playerModel;
    
    private float _moveSpeed = 0.0f;
    private static readonly int _speed = Animator.StringToHash("Speed");

    private void Start()
    {
        _maxMoveSpeed = _playerConfig.maxMoveSpeed;
        _acceleration = _playerConfig.acceleration;
        _playerModel = new PlayerModel(_playerConfig.maxHealth);
    }

    private void Update()
    {
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
