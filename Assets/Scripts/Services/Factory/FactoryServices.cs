using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Utils.FactoryTool;

namespace Services.Factory
{
    public class FactoryService : MonoBehaviour
    {
        public static FactoryService instance { get; private set; }

        [SerializeField] private List<PickableController> _pickables;
        [SerializeField] private BulletController _bulletPrefab;
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private EnemyController _enemyPrefab;
        [SerializeField] private CameraController _cameraPrefab;
        
        public List<Factory<PickableController>> pickables { get; private set; }
        public Factory<BulletController> bullets { get; private set; }
        public Factory<PlayerController> player { get; private set; }
        public Factory<EnemyController> enemy { get; private set; }
        public Factory<CameraController> camera { get; private set; }
        private void Awake()
        {
            instance = this;
            player = new Factory<PlayerController>(_playerPrefab, 1);
            camera = new Factory<CameraController>(_cameraPrefab, 1);
            enemy = new Factory<EnemyController>(_enemyPrefab, 10);
            bullets = new Factory<BulletController>(_bulletPrefab, 10);
            
            pickables = new List<Factory<PickableController>>(_pickables.Count);
            for(var i = 0; i < _pickables.Count; i++)
            {
                var pickable = new Factory<PickableController>(_pickables[i], 50);
                pickables.Add(pickable);
            }
        }
        
        private void OnDestroy()
        {
            player.Dispose();
            enemy.Dispose();
            bullets.Dispose();
            camera.Dispose();
            
            foreach (var pickable in pickables)
            {
                pickable.Dispose();
            }
        }
    }
}
