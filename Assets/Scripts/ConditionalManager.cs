using UnityEngine;
using UnityEngine.UI;

public class ConditionalManager : MonoBehaviour
{
    #region Переменные

    [Header("Score")]
    [SerializeField] private Text _scoreText;                   /// Поле текст.
    public int _score = 0;                                      /// Опыт игрока.

    [Header("PlayerControl")]
    public float _drag = 10.0f;                                 /// Лин.сопротивление(для ускорения).
    public float _runSpeedX;                                    /// Коэф. ускорения.
    public float _walkSpeed = 30.0f;                            /// Скорость ходьбы игрока.

    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _inputPanel;
    [SerializeField] private GameObject _winningPanel;
    [SerializeField] private GameObject _losingPanel;
    [SerializeField] private GameObject _defaultPanel;
    [SerializeField] private GameObject _game;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _enemiesArr;
    [SerializeField] private HealthScript _playerHealth;
    [SerializeField] private GameObject _aim;
    [SerializeField] private Text _name;
    [SerializeField] private Text _scoreBoardText;

    private GameObject _cloneGame;
    [SerializeField] private bool _playing = false;

    #endregion

    public enum GameStates { Playing, Pause, Menu, Winning, Losing };
    public GameStates CurrentGameState;

    /// <summary>
    /// Создание синглтона.
    /// </summary>
    private static ConditionalManager _instance = null;

    public static ConditionalManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        print(Time.timeScale);

        if (_playing)
        {
            GameConditions();
            EscapeButton();
            ScoreMonitoring();
        }
    }

    /// <summary>
    /// Условия выигрыша/поражения.
    /// </summary>
    private void GameConditions()
    {
        if (_enemiesArr.Length == 0)
            WinningButton();

        if (_playerHealth._death)
            LosingButton();
    }

    /// <summary>
    /// Смена состояния игры.
    /// </summary>
    private void ChangeGameState(GameStates state)
    {
        CurrentGameState = state;

        switch (CurrentGameState)
        {
            case GameStates.Playing:
                Beginnig();
                break;
            case GameStates.Pause:
                Pause();
                break;
            case GameStates.Menu:
                Menu();
                break;
            case GameStates.Winning:
                Winning();
                break;
            case GameStates.Losing:
                Losing();
                break;
            default:
                DefaultMenu();
                break;
        }
    }

    /// <summary>
    /// Начало игры.
    /// </summary>
    private void Beginnig()
    {
        _pausePanel.SetActive(false);
        _menuPanel.SetActive(false);
        _inputPanel.SetActive(false);
        _mainCamera.SetActive(false);

        if (_cloneGame == null)
        {
            _cloneGame = Instantiate(_game);

            InitialSetup();
        }

        _cloneGame.SetActive(true);

        AimVisible(true);

        Time.timeScale = 1;

        _playing = true;
    }

    /// <summary>
    /// Начальная настройка игры.
    /// </summary>
    private void InitialSetup()
    {
        if (_player == null)
            _player = GameObject.Find("Player").gameObject;

        if (_playerHealth == null)
            _playerHealth = _player.GetComponent<HealthScript>();

        if (_aim == null)
            _aim = GameObject.Find("Aim").gameObject;

        if (_scoreText == null)
            _scoreText = GameObject.Find("Score").GetComponent<Text>();

        if (_enemiesArr.Length == 0)
            _enemiesArr = GameObject.FindGameObjectsWithTag("Evil");
    }

    /// <summary>
    /// Пауза.
    /// </summary>
    private void Pause()
    {
        Time.timeScale = 0;

        AimVisible(false);
        _cloneGame.SetActive(true);
        _pausePanel.SetActive(true);
        _mainCamera.SetActive(true);
    }

    /// <summary>
    /// Меню.
    /// </summary>
    private void Menu()
    {
        Time.timeScale = 0;

        _playing = false;

        AimVisible(false);
        Destroy(_cloneGame);

        _pausePanel.SetActive(false);
        _menuPanel.SetActive(true);
        _mainCamera.SetActive(true);
    }

    /// <summary>
    ///  Меню победы.
    /// </summary>
    private void Winning()
    {
        Time.timeScale = 0;

        Destroy(_cloneGame);

        _winningPanel.SetActive(true);
        _mainCamera.SetActive(true);
    }

    /// <summary>
    ///  Меню поражения.
    /// </summary>
    private void Losing()
    {
        Time.timeScale = 0;

        Destroy(_cloneGame);

        _losingPanel.SetActive(true);
        _mainCamera.SetActive(true);
    }
    
    /// <summary>
    ///  default меню.
    /// </summary>
    private void DefaultMenu()
    {
        Time.timeScale = 0;

        AimVisible(false);
        Destroy(_cloneGame);

        _defaultPanel.SetActive(true);
        _mainCamera.SetActive(true);
    }

    /// <summary>
    /// Ввод имени.
    /// </summary>
    public void InputName()
    {
        Time.timeScale = 0;

        _menuPanel.SetActive(false);
        _inputPanel.SetActive(true);
        _mainCamera.SetActive(true);
    }

    /// <summary>
    /// Попасть на доску почета.
    /// </summary>
    public void GetInScoreBoard()
    {
        _scoreBoardText.text += $"{_name.text}: {_score}\n";
        _score = 0;

        _inputPanel.SetActive(false);
        _menuPanel.SetActive(true);
    }

    /// <summary>
    /// Вывод очков на экран
    /// </summary>
    private void ScoreMonitoring()
    {
        _scoreText.text = $"XP: {_score}";
    }

    /// <summary>
    /// Включение/выключение прицела.
    /// </summary>
    private void AimVisible(bool visible)
    {
        if (visible)
        {
            _aim.SetActive(true);
            Cursor.visible = false;
        }
        else
        {
            _aim.SetActive(false);
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// Escape.
    /// </summary>
    private void EscapeButton()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (CurrentGameState == GameStates.Playing)
            {
                ChangeGameState(GameStates.Pause);

                return;
            }

            if (CurrentGameState == GameStates.Pause)
                ChangeGameState(GameStates.Playing);
        }
    }


    public void BeginButton()
    {
        ChangeGameState(GameStates.Playing);
    }    
    
    public void PauseButton()
    {
        ChangeGameState(GameStates.Pause);
    }
     
    public void MenuButton()
    {
        ChangeGameState(GameStates.Menu);
    }
     
    public void WinningButton()
    {
        ChangeGameState(GameStates.Winning);
    } 
    
    public void LosingButton()
    {
        ChangeGameState(GameStates.Losing);
    }

    /// <summary>
    /// Выход из игры.
    /// </summary>
    public void ExitApp()
    {
        Application.Quit();
    } 
}
