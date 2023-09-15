using System;
using System.Collections;
using System.Collections.Generic;
using Configs;
using Services.Factory;
using UI;
using UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Core : MonoBehaviour
{
    public static Core instance { get; private set; }
    [SerializeField] private ScreenManager _screenManager;
    [SerializeField] private List<LevelConfig> _levelConfigs;
    [SerializeField] private GameConfig _config;

    private LevelConfig _currentLevelConfig;
    private PlayerController _player;
    private CameraController _camera;
    private List<EnemyController> _enemys = new();
    public CoreModel _model { get; private set; }
    public bool isGameOver { get; private set; }
    
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

    private void Start()
    {
        SetLvlConfig();
        _model = new CoreModel(_currentLevelConfig.taskCount, _currentLevelConfig.taskId);
        ShowMenuScreen();
        PlayerPrefs.DeleteAll();
    }

    private void ShowMenuScreen()
    {
        _screenManager.CloseLastScreen();
        SetLvlConfig();
        _model = new CoreModel(_currentLevelConfig.taskCount, _currentLevelConfig.taskId);
        var screen = (MenuScreen)_screenManager.OpenScreen(ScreenTypes.MENU);
        screen.SetText((_currentLevelConfig.levelId + 1).ToString());
        screen.onPlay += OnPlay;
        screen.onExit += OnExit;
    }

    private void OnPlay()
    {
        isGameOver = false;
        var previous = (MenuScreen)_screenManager.GetCurrentScreen();
        previous.onPlay -= OnPlay;
        previous.onExit -= OnExit;
        
        _screenManager.CloseLastScreen();
        SceneManager.LoadScene(sceneBuildIndex: 1);
        var screen = (GameScreen)_screenManager.OpenScreen(ScreenTypes.GAME);
        StartCoroutine(Delay(0.5f, () =>
        {
            PlayerInit();
            EnemyInit();
        }));
        
        screen.SetTaskImg(_currentLevelConfig.sprite);
        screen.SetTaskCount(_currentLevelConfig.taskCount);
        screen.onMenu += OnMenu;
    }
    
    private void OnMenu()
    {
        var screen = (Popup)_screenManager.OpenScreen(ScreenTypes.POPUP);
        Time.timeScale = 0;
        screen.onConfirm += OnConfirm;
        screen.onCancel += OnCancel;
    }

    private void OnConfirm()
    {
        var previous = (Popup)_screenManager.GetCurrentScreen();
        previous.onConfirm -= OnConfirm;
        previous.onCancel -= OnCancel;
        
        Time.timeScale = 1;
        _screenManager.CloseLastScreen();
        SceneManager.LoadScene(sceneBuildIndex: 0);
        ShowMenuScreen();
    }

    private void OnCancel()
    {
        var currentScreen = (Popup)_screenManager.GetCurrentScreen();
        currentScreen.onConfirm -= OnConfirm;
        currentScreen.onCancel -= OnCancel;
        Time.timeScale = 1;
        _screenManager.CloseLastScreen();
    }

    private void OnExit()
    {
        Application.Quit();
    }

    private void SetLvlConfig()
    {
        var lvl = SaveManager.instance.GetCurrentLvl();
        if (lvl >= _levelConfigs.Count)
        {
            lvl = 0;
        }
        _currentLevelConfig = _levelConfigs[lvl];
    }

    private void PlayerInit()
    {
        _player = FactoryService.instance.player.Produce();
        _player.transform.position = _config.playerSpawnPoint;
        CameraInit();
        _player.SetupParameters(_config.playerMaxMoveSpeed,_config.playerAcceleration, _config.playerMaxHealth, _camera);
        _player.onDeath += OnDeath;
        _player.onPickup += OnPickup;
    }

    private void EnemyInit()
    {
        for(var i = 0; i <= _config.enemyCount; i++)
        {
            var enemy = FactoryService.instance.enemy.Produce();
            enemy.SetupParameters(_config.enemyMaxMoveSpeed, _config.enemyAcceleration, _config.enemyRotationSpeed,
                _config.enemyAttackRange, _config.enemyAttackCooldown, _config.enemyDamage, _config.bulletSpeed, _player);
            enemy.transform.position = GetRandomPos();
            _enemys.Add(enemy);
        }
    }

    private void CameraInit()
    {
        _camera = FactoryService.instance.camera.Produce();
        _camera.SetTarget(_player.transform);
    }

    private Vector3 GetRandomPos()
    {
        var posX = Random.Range(-10, 10);
        var posZ = Random.Range(-10, 10);
        return new Vector3(posX, 0.0f, posZ);
    }

    private void OnDeath()
    {
        StartCoroutine(Delay(0.5f, () =>
        {
            var previous = (GameScreen)_screenManager.GetCurrentScreen();
            previous.onMenu -= OnMenu;
            _screenManager.CloseLastScreen();
            SceneManager.LoadScene(sceneBuildIndex: 0);
            var screen = (LoseScreen)ScreenManager.instance.OpenScreen(ScreenTypes.LOSE);
            screen.onMenu += OnLoseMenuClick;
        }));
    }

    private void OnVictory()
    {
        SaveManager.instance.SaveCurrentLevel(_currentLevelConfig.levelId + 1);
        var previous = (GameScreen)_screenManager.GetCurrentScreen();
        previous.onMenu -= OnMenu;
        _screenManager.CloseLastScreen();
        SceneManager.LoadScene(sceneBuildIndex: 0);
        var screen = (VictoryScreen)ScreenManager.instance.OpenScreen(ScreenTypes.VICTORY);
        screen.onMenu += OnVictoryMenuClick;
    }

    public void OnPickup(int id)
    {
        if(isGameOver) return;
        _model.ChangeCount(id);
        if (_model.taskFoodCount <= 0)
        {
            isGameOver = true;
            OnVictory();
        }
        else
        {
            var screen = (GameScreen)_screenManager.GetCurrentScreen();
            screen.SetTaskCount(_model.taskFoodCount);
        }
    }

    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnLoseMenuClick()
    {
        var previous = (LoseScreen)_screenManager.GetCurrentScreen();
        previous.onMenu -= OnLoseMenuClick;
        _screenManager.CloseLastScreen();
        SetLvlConfig();
        _model = new CoreModel(_currentLevelConfig.taskCount, _currentLevelConfig.taskId);
        var screen = (MenuScreen)_screenManager.OpenScreen(ScreenTypes.MENU);
        screen.SetText((_currentLevelConfig.levelId + 1).ToString());
        screen.onPlay += OnPlay;
    }

    private void OnVictoryMenuClick()
    {
        var previous = (VictoryScreen)_screenManager.GetCurrentScreen();
        previous.onMenu -= OnVictoryMenuClick;
        _screenManager.CloseLastScreen();
        SetLvlConfig();
        _model = new CoreModel(_currentLevelConfig.taskCount, _currentLevelConfig.taskId);
        var screen = (MenuScreen)_screenManager.OpenScreen(ScreenTypes.MENU);
        screen.SetText((_currentLevelConfig.levelId + 1).ToString());
        screen.onPlay += OnPlay;
    }

    public void GetInput(Vector2 input)
    {
        if(isGameOver) return;
        if(_player != null) _player.SetInput(input);
    }
    
    private IEnumerator Delay(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action?.Invoke();
    }
}
