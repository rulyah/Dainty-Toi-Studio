using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/SpawnerConfig", fileName = "SpawnerConfig")]
    public class SpawnerConfig : ScriptableObject
    {
        public int spawnInterval;
        public Vector3 spawnOffset;
    }
}