using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public float maxMoveSpeed;
        public float acceleration;
        public int maxHealth;
    }
}