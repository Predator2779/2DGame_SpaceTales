using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float _realSpeed = 0;

    private ManagerScript _gameManager;
    private Rigidbody2D _rBody;

    /// <summary>
    /// Start.
    /// </summary>
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<ManagerScript>();
        _rBody = gameObject.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        CameraControl();
        MouseControl();
        Shooting();
    }

    /// <summary>
    /// FixedUpdate.
    /// </summary>
    private void FixedUpdate()
    {
        WalkControl();
    }

    /// <summary>
    /// Слежение камеры за игроком.
    /// </summary>
    private void CameraControl()
    {
        var position = new Vector2(transform.position.x, transform.position.y);
        GameObject.Find("PlayerCamera").transform.position = position;
    }

    /// <summary>
    /// Слежение игрока за мышкой.
    /// </summary>
    private void MouseControl()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //положение мыши из экранных в мировые координаты
        var angle = Vector2.Angle(Vector2.right, mousePosition - transform.position); //угол между вектором от объекта к мыше и осью х

        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePosition.y ? (angle - 90.0f) : (-angle - 90.0f));
    }

    /// <summary>
    /// Стрельба.
    /// </summary>
    private void Shooting()
    {
        var gun = gameObject.transform.Find("BlasterGun").GetComponent<AttackScript>();

        if (Input.GetMouseButton(0)) gun._fire = true;
    }

    /// <summary>
    /// Перемещение игрока по карте.
    /// </summary>
    private void WalkControl()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _realSpeed = _gameManager._walkSpeed;
            _rBody.AddForce(Vector2.up * _realSpeed * 1000.0f);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) _realSpeed = 0;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _realSpeed = _gameManager._walkSpeed;
            _rBody.AddForce(-Vector2.up * _realSpeed * 1000.0f);
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) _realSpeed = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _realSpeed = _gameManager._walkSpeed;
            _rBody.AddForce(-Vector2.right * _realSpeed * 1000.0f);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) _realSpeed = 0;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _realSpeed = _gameManager._walkSpeed;
            _rBody.AddForce(Vector2.right * _realSpeed * 1000.0f);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) _realSpeed = 0;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _rBody.drag = _gameManager._drag * (1 / _gameManager._runSpeedX);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) _rBody.drag = _gameManager._drag;
    }
}
