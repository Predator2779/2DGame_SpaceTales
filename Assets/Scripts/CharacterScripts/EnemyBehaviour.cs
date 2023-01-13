using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform _target;

    [SerializeField] private ConditionalManager _conditionalManager;
    [SerializeField] private EnemyData _enemyData;

    [SerializeField] private int _level = 1;

    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _koefSpeed;

    [SerializeField] private HealthScript _health;
    [SerializeField] private HealthScript _shieldHealth;
    [SerializeField] private AttackScript _gun;
    [SerializeField] private AttackScript _secondGun;


    private enum EnemyStates { Pursuit, Patrol, Idle, Death, Default };
    [SerializeField] private EnemyStates _currentEnemyState;

    [SerializeField] private float _distanceToTarget = 80.0f; /////
    [SerializeField] private bool _coroutineisWorked = false;

    private Rigidbody2D _rbody;
    private float _koefShieldDefense;
    private float _koefDistanceFromTarget;

    public int Level { get => _level; set { _level = _enemyData._level; } }
    public float KoefShieldDefense
    {
        get => _koefShieldDefense;
        set
        {
            if (_enemyData._isShielded)
            {
                _koefShieldDefense = _enemyData._koefShieldDefense;
            }
        }
    }

    private void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();

        if (_conditionalManager == null)
            _conditionalManager = GameObject.Find("ConditionalManager").GetComponent<ConditionalManager>();

        _health = gameObject.GetComponent<HealthScript>();
        _shieldHealth = gameObject.transform.Find("Shield").GetComponent<HealthScript>();

        _gun = gameObject.transform.Find("BlasterGun").GetComponent<AttackScript>();
        _secondGun = gameObject.transform.Find("SecondBlasterGun").GetComponent<AttackScript>();
    }

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        if (Time.timeScale > 0)
        {
            EnemyStateMachine(_currentEnemyState);;
        }
    }

    /// <summary>
    /// Логика врага.
    /// </summary>
    private void EnemyLogic()
    {
        if (_target != null)
        {
            _currentEnemyState = EnemyStates.Pursuit;
        }
        else
        {
            int randoChance = Random.Range(0, 101);

            if (randoChance < 51)
            {
                _currentEnemyState = EnemyStates.Patrol;
            }
            else
            {
                _currentEnemyState = EnemyStates.Idle;
            }
        }
    }

    /// <summary>
    /// Смена состояний врага.
    /// </summary>
    /// <param name="state"></param>
    private void EnemyStateMachine(EnemyStates state)
    {
        switch (state)
        {
            case EnemyStates.Pursuit:
                Pursuit(_target);
                break;
            case EnemyStates.Patrol:
                Patrol();
                break;
            case EnemyStates.Idle:
                Idle();
                break;
            case EnemyStates.Death:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Преследование цели.
    /// </summary>
    /// <param name="target">Цель</param>
    private void Pursuit(Transform target)////////////////////attack distance
    {
        if (_target != null)
        {
            Rotate(target);
            KeepDistanseFromTarget();
            Move(_sprintSpeed * _koefDistanceFromTarget);
            OpenFire(true);
        }
        else
        {
            OpenFire(false);
            EnemyLogic();
        }
    }

    /// <summary>
    /// Патрулирование.
    /// </summary>
    private void Patrol()
    {
        if (!_coroutineisWorked)
        {
            var randAngle = Random.Range(0.0f, 360.0f);

            TurnOn(randAngle);

            int randoTime = Random.Range(0, 4);

            StartCoroutine(ChangeStateAfterTime(randoTime));
        }

        Move(_walkSpeed);
    }

    /// <summary>
    /// Бездействие.
    /// </summary>
    private void Idle()
    {
        if (!_coroutineisWorked)
        {
            int randoTime = Random.Range(0, 7);

            StartCoroutine(ChangeStateAfterTime(randoTime));
        }

        _currentSpeed = 0.0f;
    }

    /// <summary>
    /// Открыть огонь.
    /// </summary>
    private void OpenFire(bool permission)
    {
        if (_gun != null)
            _gun._fire = permission;

        if (_secondGun != null)
            _secondGun._fire = permission;
    }

    /// <summary>
    /// Перемещение объекта.
    /// </summary>
    private void Move(float speed)
    {
        float maxSpeed = speed * _koefSpeed;

        if (_currentSpeed >= maxSpeed)
        {
            _currentSpeed = maxSpeed;
        }
        else
        {
            _currentSpeed += _acceleration;
        }

        Vector2 motionVector = transform.up * _currentSpeed * _rbody.angularDrag * _rbody.drag;

        _rbody.AddForce(motionVector);

        Debug.DrawRay(transform.position, motionVector, Color.red);
    }

    /// <summary>
    /// Поворот объекта в сторону цели.
    /// </summary>
    /// <param name="target">Цель</param>
    private void Rotate(Transform target)
    {
        /// Угол между целью и нашим объектом.
        var angle = Vector2.Angle(Vector2.right, target.position - transform.position);

        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < target.position.y ? (angle - 90.0f) : (-angle - 90.0f));
    }

    /// <summary>
    /// Поворот объекта на указанный угол.
    /// </summary>
    /// <param name="target">Цель</param>
    private void TurnOn(float angle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    /// <summary>
    /// Удерживать дистанцию до игрока.
    /// </summary>
    private void KeepDistanseFromTarget()
    {
        float distance = Vector2.Distance(transform.position, _target.transform.position);
        float distanceToTarget = _target.localScale.y * _distanceToTarget;
        float variation = distanceToTarget * 0.25f;
        float minDistance = distanceToTarget - variation;
        float maxDistance = distanceToTarget + variation;

        if (minDistance < distance && distance < maxDistance)
        {
            _koefDistanceFromTarget = 0;
        }
        else if (distance > maxDistance)
        {
            _koefDistanceFromTarget = 1;
        }
        else if (distance < minDistance)
        {
            _koefDistanceFromTarget = -1;
        }
    }

    /// <summary>
    /// Опыт игроку.
    /// </summary>
    private void RandoScore()
    {
        var score = Random.Range(65, 100) * _level;

        _conditionalManager._score += score;            /////////создать метод прибавления опыта в менеджере
    }

    /// <summary>
    ///  Корутина смена состояния по таймеру.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeStateAfterTime(float time)
    {
        _coroutineisWorked = true;  

        yield return new WaitForSecondsRealtime(time);

        _coroutineisWorked = false;

        EnemyLogic();

        yield return null;
    }
}
