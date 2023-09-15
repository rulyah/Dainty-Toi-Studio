using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public Sprite sprite;
        public int taskCount;
        public int taskId;
        public int levelId;
    }
}