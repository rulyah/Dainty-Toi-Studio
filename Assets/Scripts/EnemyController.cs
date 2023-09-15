using System;
using System.Collections;
using Services.Factory;
using UnityEngine;
using UnityEngine.AI;
using Utils.FactoryTool;

public class EnemyController : PoolableMonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _weapon;

    private Vector3 _idleWeaponPos;
    private Vector3 _idleWeaponRot;
    private Vector3 _moveWeaponPos = new Vector3(0f, 0.35f, 0.06f);
    private Vector3 _moveWeaponRot = new Vector3(94.641f, 82.18f, 174.86f);
    
    private PlayerController _target;
    private float _attackRange;
    private float _rotationSpeed;
    private float _attackCooldown;
    private int _damage;
    private float _bulletSpeed;
    private float timeSinceLastAttack;
    private bool _isShooting;
    private bool _isChasing;

    private static readonly int _chase = Animator.StringToHash("Chase");
    private static readonly int _shooting = Animator.StringToHash("Shooting");

    private void Start()
    {
        _idleWeaponPos = _weapon.localPosition;
        _idleWeaponRot = _weapon.localEulerAngles;
    }

    public void SetupParameters(float maxMoveSpeed, float acceleration, float rotationSpeed, float attackRange, 
        float attackCooldown, int damage, float bulletSpeed, PlayerController target)
    {
        _navMeshAgent.speed = maxMoveSpeed;
        _navMeshAgent.acceleration = acceleration;
        _rotationSpeed = rotationSpeed;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _bulletSpeed = bulletSpeed;
        _damage = damage;
        _target = target;
    }

    void Update()
    {
        if (!_target.isDead)
        {
            if(_isShooting) return;
            
            float distance = Vector3.Distance(transform.position, _target.transform.position);
            
            Vector3 direction = _target.transform.position - transform.position;
            direction.y = 0;
            Quaternion rotationToPlayer = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToPlayer, Time.deltaTime * _rotationSpeed);
            
            if(distance > _attackRange) ChasePlayer();
            if(distance <= _attackRange && !_isShooting)
            {
                _navMeshAgent.SetDestination(transform.position);
                AttackPlayer();
            }
        }
        else
        {
            _animator.SetBool(_chase, false);
            _weapon.transform.localPosition = _idleWeaponPos;
            _weapon.transform.localEulerAngles = _idleWeaponRot;
        }
    }
    
    void ChasePlayer()
    {
        if (!_isChasing)
        {
            _isChasing = true;
            _weapon.transform.localPosition = _moveWeaponPos;
            _weapon.transform.localEulerAngles = _moveWeaponRot;
        }
        _animator.SetBool(_chase, true);
        _navMeshAgent.SetDestination(_target.transform.position);
    }

    void AttackPlayer()
    {
        if (timeSinceLastAttack >= _attackCooldown)
        {
            _isShooting = true;
            _animator.SetTrigger(_shooting);
            StartCoroutine(Delay(0.5f, () =>
            {
                {
                    var bullet = FactoryService.instance.bullets.Produce();
                    bullet.transform.position = _shootPoint.position;
                    var direction = (new Vector3(_target.transform.position.x, 
                        0.5f, _target.transform.position.z) - _shootPoint.position).normalized;
                    bullet.Fire(direction, _bulletSpeed, _damage);
                    StartCoroutine(Delay(0.5f, () =>
                    {
                        _isShooting = false;
                    }));
                }
            }));
            timeSinceLastAttack = 0f;
        }
        timeSinceLastAttack += Time.deltaTime;
    }
    
    private IEnumerator Delay(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action?.Invoke();
    }
}