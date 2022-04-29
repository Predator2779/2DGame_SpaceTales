using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour
{
    #region Переменные

    [Header("Score")]
    [SerializeField] private Text _scoreText;                   /// Поле текст.
    public int _score = 0;                                      /// Опыт игрока.

    [Header("PlayerControl")]
    public float _drag = 10.0f;                                 /// Лин.сопротивление(для ускорения).
    public float _runSpeedX;                                    /// Коэф. ускорения.
    public float _walkSpeed = 30.0f;                            /// Скорость ходьбы игрока.

    /// Переменные GameManager.
    public bool _playing = false;
    public bool _pause = false;

    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _namePanel;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _cloneGame;
    [SerializeField] private GameObject _player;
    [SerializeField] private Text _name;
    [SerializeField] private Text _scoreBoardText;

    #endregion

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        ManageGame();
    }

    /// <summary>
    /// Контроль игры.
    /// </summary>
    private void ManageGame()
    {
        MonitorScore();
        EscapeButton();
        Pause();
        Lose();
    }

    /// <summary>
    /// Управление.
    /// </summary>
    private void EscapeButton()
    {
        if (Input.GetKey(KeyCode.Escape) && _playing)
            _pause = true;
    }

    /// <summary>
    /// Вывод очков на экран
    /// </summary>
    private void MonitorScore()
    {
        if (_playing && !_pause)
        {
            _scoreText = GameObject.Find("Score").GetComponent<Text>();
            _scoreText.text = $"XP: {_score}";
        }
    }

    /// <summary>
    /// Начало игры.
    /// </summary>
    public void Begin()
    {
        _menuPanel.SetActive(false);
        _mainCamera.SetActive(false);
        _playing = true;

        _cloneGame = Instantiate(_game);
    }

    /// <summary>
    /// В меню.
    /// </summary>
    public void ToMenu()
    {
        _playing = false;
        _pause = false;

        Destroy(_cloneGame);

        _pausePanel.SetActive(false);
        _menuPanel.SetActive(true);
        _mainCamera.SetActive(true);
    }

    /// <summary>
    /// Пауза.
    /// </summary>
    private void Pause()
    {
        if (_playing && _pause)
        {
            Time.timeScale = 0;
            _mainCamera.SetActive(true);
            _pausePanel.SetActive(true);
            _cloneGame.SetActive(false);
        }
    }

    /// <summary>
    /// Продолжить игру.
    /// </summary>
    public void Continue()
    {
        _pause = false;

        _mainCamera.SetActive(false);
        _pausePanel.SetActive(false);
        _cloneGame.SetActive(true);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Условия поражения.
    /// </summary>
    private void Lose()
    {
        if (_playing && !_pause)
        {
            _player = GameObject.Find("Player").gameObject;

            if (_player != null)
                if (_player.GetComponent<HealthScript>()._death)
                {
                    Destroy(_cloneGame);
                    _mainCamera.SetActive(true);
                    _namePanel.SetActive(true);
                    _playing = false;
                }
        }
    }

    /// <summary>
    /// Попасть на доску почета.
    /// </summary>
    public void InputName()
    {
        _scoreBoardText.text += $"{_name.text}: {_score}\n";
        _score = 0;

        _namePanel.SetActive(false);
        _menuPanel.SetActive(true);
    }

    /// <summary>
    /// Выход из игры.
    /// </summary>
    public void ExitApp()
    {
        Application.Quit();
    } 
}
