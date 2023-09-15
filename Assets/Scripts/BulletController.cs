using System;
using Services.Factory;
using UnityEngine;
using Utils.FactoryTool;

namespace DefaultNamespace
{
    public class BulletController : PoolableMonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private Vector3 _firstPos;
        private Vector3 _secondPos;
        private Vector3 _startPos;
        private int _damage;

        private bool _canDamage = true;

        public void Fire(Vector3 direction, float speed, int damage)
        {
            _firstPos = transform.position;
            _startPos = _firstPos;
            _damage = damage;
            if (_canDamage == false) _canDamage = true;
            transform.rotation = Quaternion.LookRotation(direction);
            _rigidbody.AddForce(direction * speed, ForceMode.Impulse);
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _canDamage = true;
        }

        private void Update()
        {
            if(!_canDamage) return;
            _secondPos = transform.position;
            if (Physics.Raycast(_secondPos, Vector3.Normalize(_secondPos - _firstPos), out var hit,
                    Vector3.Distance(_secondPos, _firstPos)))
            {
                _firstPos = _secondPos;
                
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    var player = hit.collider.gameObject.GetComponent<PlayerController>();
                    player.TakeDamage(_damage);
                    _canDamage = false;
                    Reset();
                    FactoryService.instance.bullets.Release(this);
                }
            }
            else
            {
                var distance = Vector3.Distance(_startPos, transform.position);
                if(distance > 15) FactoryService.instance.bullets.Release(this);
            }
        }
    }
}