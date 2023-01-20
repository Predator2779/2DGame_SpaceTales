using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CharacterMovement))]
public class EnemyStateChanger : MonoBehaviour
{
    /// Создать родительский класс Character.
    /// Создать метод Death.
    
    public Transform _target;

    [SerializeField] private ConditionalManager _conditionalManager;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private VisibilityZone _vision;

    [SerializeField] private int _level = 1;

    [SerializeField] private float _currentSpeed;
    [SerializeField] [Range(0.01f, 2.0f)] private float _acceleration;
    [SerializeField] [Range(0.01f, 2.0f)] private float _walkSpeed;
    [SerializeField] [Range(0.01f, 2.0f)] private float _sprintSpeed;

    [SerializeField] private HealthScript _health;
    [SerializeField] private HealthScript _shieldHealth;
    [SerializeField] private AttackScript _gun;
    [SerializeField] private AttackScript _secondGun;


    private enum EnemyStates { Pursuit, Patrol, Idle, Death, Default };
    [SerializeField] private EnemyStates _currentState;

    [SerializeField] private float _distanceToTarget = 80.0f;
    [SerializeField] private float _distanceOpenFire = 200.0f;
    [SerializeField] private bool _coroutineisWorked = false;

    private CharacterMovement _characterMovement;
    private Rigidbody2D _rbody;
    private float _koefShieldDefense;

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
        _characterMovement = GetComponent<CharacterMovement>();

        CheckStateConditions();
        CheckEmptyFields();
    }

    private void Update()
    {
        if (Time.timeScale > 0) /// пофиксиить зависимость от течения времени.
        {
            CheckTarget();
            SetState(_currentState);;
        }
    }

    private void CheckEmptyFields()
    {
        if (_conditionalManager == null)
            _conditionalManager = GameObject.Find("ConditionalManager").GetComponent<ConditionalManager>();

        _health = gameObject.GetComponent<HealthScript>();
        _shieldHealth = gameObject.transform.Find("Shield").GetComponent<HealthScript>();

        _gun = gameObject.transform.Find("BlasterGun").GetComponent<AttackScript>();
        _secondGun = gameObject.transform.Find("SecondBlasterGun").GetComponent<AttackScript>();
    }

    #region Change States

    /// <summary>
    /// Смена состояний, в зависимости от наличия цели.
    /// </summary>
    private void CheckStateConditions()
    {
        if (_target != null)
        {
            _currentState = EnemyStates.Pursuit;
        }
        else
        {
            int randoChance = Random.Range(0, 101);

            if (randoChance < 51)
            {
                _currentState = EnemyStates.Patrol;
            }
            else
            {
                _currentState = EnemyStates.Idle;
            }
        }
    }

    private void CheckTarget()/////event!!s
    {
        if (_vision.Target != null)
        {
            _target = _vision.Target;
        }
    }

    /// <summary>
    /// Смена состояний врага.
    /// </summary>
    /// <param name="state"></param>
    private void SetState(EnemyStates state)///death
    {
        switch (state)
        {
            case EnemyStates.Pursuit:
                PursuitTarget(_target);
                break;
            case EnemyStates.Patrol:
                PatrolArea();
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
    ///  Корутина смена состояния по таймеру.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeStateAfterTime(float time)
    {
        _coroutineisWorked = true;

        yield return new WaitForSecondsRealtime(time);

        _coroutineisWorked = false;

        CheckStateConditions();

        yield return null;
    }

    /// <summary>
    /// Преследование цели.
    /// </summary>
    /// <param name="target">Цель</param>
    private void PursuitTarget(Transform target)
    {
        if (_target != null)
        {
            float koefDistanceFromTarget =
                _characterMovement.KeepDistanseFromTarget(transform, target, _distanceToTarget);

            float maxSpeed = _sprintSpeed * koefDistanceFromTarget;
            float currentDistance = Vector2.Distance(transform.position, target.transform.position);

            _characterMovement.RotateToTarget(target);
            _currentSpeed = _characterMovement.MoveForward(_rbody, _currentSpeed, maxSpeed, _acceleration);

            OpenFire(currentDistance < _distanceOpenFire);
        }
        else
        {
            OpenFire(false);
            CheckStateConditions();
        }
    }

    /// <summary>
    /// Патрулирование.
    /// </summary>
    private void PatrolArea()
    {
        if (!_coroutineisWorked)
        {
            var randomAngle = Random.Range(0.0f, 360.0f);

            _characterMovement.TurnOnAngle(randomAngle);

            int randomTime = Random.Range(0, 3);

            StartCoroutine(ChangeStateAfterTime(randomTime));
        }

        _currentSpeed = _characterMovement.MoveForward(_rbody, _currentSpeed, _walkSpeed, _acceleration);
    }

    /// <summary>
    /// Бездействие.
    /// </summary>
    private void Idle()
    {
        if (!_coroutineisWorked)
        {
            int randomTime = Random.Range(0, 7);

            StartCoroutine(ChangeStateAfterTime(randomTime));
        }

        _currentSpeed = 0.0f;
    }

    #endregion

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
    /// Начисление опыта игроку.
    /// </summary>
    private void SetScore()
    {
        var score = Random.Range(65, 100) * _level;

        _conditionalManager._score += score;            /////////создать метод прибавления опыта в менеджере или в игроке
    }
}