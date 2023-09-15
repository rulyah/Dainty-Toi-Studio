using UnityEngine;
using UnityEngine.UI;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public Sprite sprite;
        public int count;
        public int id;
        public int levelId;
    }
}