using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Player")]
        public float playerMaxMoveSpeed;
        public float playerAcceleration;
        public int playerMaxHealth;
        public Vector3 playerSpawnPoint;
        
        [Space(10)]
        [Header("Enemy")]
        public int enemyCount;
        public float enemyMaxMoveSpeed;
        public float enemyAcceleration;
        public float enemyRotationSpeed;
        public float enemyAttackRange;
        public float enemyAttackCooldown;
        public int enemyDamage;
        public float bulletSpeed;
    }
}