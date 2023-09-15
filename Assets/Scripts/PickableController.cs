using System;
using System.Collections;
using Services.Factory;
using UnityEngine;
using Utils.FactoryTool;

public class PickableController : PoolableMonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TriggerListener _triggerListener;
    [SerializeField] private int _id;
    public bool isPickedUp { get; private set; }
    
    public event Action onPickablePickedUp;

    public void Init()
    {
        _rigidbody.isKinematic = true;
        _triggerListener.onTriggerEntered += OnTriggerEntered;
    }

    /*private void OnDestroy()
    {
        _triggerListener.onTriggerEntered -= OnTriggerEntered;
    }*/

    private void OnTriggerEntered(Collider other)
    {
        if (isPickedUp) return;
        
        if (other.CompareTag("Player"))
        {
            _rigidbody.isKinematic = false;
            isPickedUp = true;
            PlayerController player = other.GetComponent<PlayerController>();
            if(!player.isDead) StartCoroutine(PlayMagnetAnimation(player));
        }
    }
    
    private IEnumerator PlayMagnetAnimation(PlayerController player)
    {
        while (true)
        {
            yield return null;
            
            Vector3 direction = (player.transform.position - transform.position).normalized;
            direction.y = 0.0f;
            direction.Normalize();
            
            _rigidbody.velocity = direction * 4.0f;
            
            if (Vector3.Distance(player.transform.position, transform.position) < 0.5f)
            {
                onPickablePickedUp?.Invoke();
                Reset();
                FactoryService.instance.pickables[_id].Release(this);
                break;
            }
        }
    }

    private void Reset()
    {
        isPickedUp = false;
        transform.position = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        _triggerListener.onTriggerEntered -= OnTriggerEntered;
    }
}
