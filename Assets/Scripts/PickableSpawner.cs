using System.Collections;
using System.Collections.Generic;
using Configs;
using Services.Factory;
using UnityEngine;

public class PickableSpawner : MonoBehaviour
{
    [SerializeField] private SpawnerConfig _config;
    
    public PickableController currentPickable { get; private set; }
    
    private float _spawnInterval;
    private Vector3 _spawnOffset;
    
    private void Start()
    {
        _spawnInterval = _config.spawnInterval;
        _spawnOffset = _config.spawnOffset;
        StartCoroutine(SpawnPickables());
    }
    
    private IEnumerator SpawnPickables()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            
            if (currentPickable != null) continue;
            
            int index = Random.Range(0, FactoryService.instance.pickables.Count);
            var pickable = FactoryService.instance.pickables[index].Produce();
            pickable.Init();
            pickable.transform.position = transform.position + _spawnOffset;
            pickable.transform.rotation = Quaternion.identity;
            currentPickable = pickable;
            pickable.onPickablePickedUp += OnPickablePickedUp;
        }
    }
    
    private void OnPickablePickedUp()
    {
        currentPickable.onPickablePickedUp -= OnPickablePickedUp;
        currentPickable = null;
    }
}
