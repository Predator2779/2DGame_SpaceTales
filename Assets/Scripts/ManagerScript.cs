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

    [Header("GameManager")]                                     /// Играет.
    public bool _playing = false;                               /// Пауза.
    public bool _paused = false;

    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _inputPanel;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _cloneGame;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _aim;
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
        Playing();
        Pause();
        EscapeButton();
    }

    /// <summary>
    /// Процессы во время игры.
    /// </summary>
    private void Playing()
    {
        if (_playing && !_paused)
        {
            MonitorScore();
            AimFind();
            AimEnable();
            LoseCondition();
        }
    }

    /// <summary>
    /// Пауза.
    /// </summary>
    private void Pause()
    {
        if (_playing && _paused)
        {
            Time.timeScale = 0;
            _mainCamera.SetActive(true);
            _pausePanel.SetActive(true);
            _cloneGame.SetActive(false);

            AimDisable();
        }
    }

    /// <summary>
    /// Управление.
    /// </summary>
    private void EscapeButton()
    {
        if (Input.GetKey(KeyCode.Escape) && _playing)
            _paused = true;
    }

    /// <summary>
    /// Вывод очков на экран
    /// </summary>
    private void MonitorScore()
    {
        _scoreText = GameObject.Find("Score").GetComponent<Text>();
        _scoreText.text = $"XP: {_score}";
    }

    /// <summary>
    /// Включение прицела.
    /// </summary>
    private void AimEnable()
    {
        _aim.SetActive(true);
        CursorVisible(false);
    }

    /// <summary>
    /// Выключение прицела.
    /// </summary>
    private void AimDisable()
    {
        _aim.SetActive(false);
        CursorVisible(true);
    }

    /// <summary>
    /// Поиск прицела на сцене.
    /// </summary>
    private void AimFind()
    {
        if (_aim == null)
            _aim = GameObject.Find("Aim").gameObject;
    }

    /// <summary>
    /// Включает/отключает курсор.
    /// </summary>
    /// <param name="visible">Видимость</param>
    private void CursorVisible(bool visible) => Cursor.visible = visible;

    /// <summary>
    /// Условия поражения.
    /// </summary>
    private void LoseCondition()
    {
        _player = GameObject.Find("Player").gameObject;

        if (_player != null && _player.GetComponent<HealthScript>()._death)
        {
            Destroy(_cloneGame);
            AimDisable();

            _mainCamera.SetActive(true);
            _inputPanel.SetActive(true);
            _playing = false;
        }
    }

    /// <summary>
    /// Попасть на доску почета.
    /// </summary>
    public void InputName()
    {
        _scoreBoardText.text += $"{_name.text}: {_score}\n";
        _score = 0;

        _inputPanel.SetActive(false);
        _menuPanel.SetActive(true);
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
        _paused = false;

        Destroy(_cloneGame);

        _pausePanel.SetActive(false);
        _menuPanel.SetActive(true);
        _mainCamera.SetActive(true);
        _cloneGame.SetActive(true);

        Time.timeScale = 1;
    }

    /// <summary>
    /// Продолжить игру.
    /// </summary>
    public void Continue()
    {
        _paused = false;

        _mainCamera.SetActive(false);
        _pausePanel.SetActive(false);
        _cloneGame.SetActive(true);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Выход из игры.
    /// </summary>
    public void ExitApp()
    {
        Application.Quit();
    } 
}
