using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }
    
    private const string currentLevel = "CURRENT_LEVEL";
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void SaveCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(currentLevel, level);
    }

    public int GetCurrentLvl()
    {
        return PlayerPrefs.GetInt(currentLevel, 0);
    }
}