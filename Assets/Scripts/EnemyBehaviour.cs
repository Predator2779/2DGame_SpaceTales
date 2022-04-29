using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform _target;
    public bool _patrol = false;

    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _distanceToPlayer = 30.0f;
    [SerializeField] private float _timePatrol = 5.0f;
    [SerializeField] private int _level = 1;

    private bool _randomVector = true;
    private bool _moving = false;
    private float _speed = 0;

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        EnemyLogic();
    }

    /// <summary>
    /// ������ �������.
    /// </summary>
    private void EnemyLogic()
    {
        if (_target != null)                     /// ���� ���.
        {
            PursuitPlayer(_target);
            AttackTarget();
        }
        else _speed = 0.2f;

        var health = gameObject.GetComponent<HealthScript>();
        var shieldHealth = gameObject.transform.Find("Shield").GetComponent<HealthScript>();

        if (health._points != 100.0f || shieldHealth._points != 100.0f)            /// ���� ���.
        {
            _target = GameObject.Find("Player").transform;

            PursuitPlayer(_target);
            AttackTarget();
        }

        if (_target == null || _patrol)
            Patrol();

        Death();
    }

    /// <summary>
    /// ��������������.
    /// </summary>
    private void Patrol()
    {
        _walkSpeed = 0.1f;

        _patrol = true;
        _moving = true;

        Accelerate();
        Moving();

        if (_randomVector)
        {
            StartCoroutine(PatrolTimer());

            var randAngle = Random.Range(0.0f, 360.0f);
            var rot = transform.rotation;

            rot.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, randAngle);

            transform.rotation = rot;

            _randomVector = false;
        }
    }

    /// <summary>
    ///  ������ ��������������.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PatrolTimer()
    {
        var time = _timePatrol;

        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        _randomVector = true;
        yield return null;
    }

    /// <summary>
    /// ������������� ����.
    /// </summary>
    /// <param name="target">����</param>
    private void PursuitPlayer(Transform target)
    {
        _walkSpeed = 0.8f;
        _moving = true;

        RotateToTarget(target);
        DistanceToPlayer();
        Accelerate();
        Moving();
    }

    /// <summary>
    /// ����� ����.
    /// </summary>
    /// <param name="target"></param>
    private void AttackTarget()
    {
        var gun = gameObject.transform.Find("BlasterGun").GetComponent<AttackScript>();
        gun._fire = true;

        AttackScript secondGun;
        if (secondGun = gameObject.transform.Find("SecondBlasterGun").GetComponent<AttackScript>())
            secondGun._fire = true;
    }

    /// <summary>
    /// ����������� �������.
    /// </summary>
    private void Moving()
    {
        if (_moving)
        {
            transform.localPosition += transform.up * _speed;
        }
        else _speed = 0;
    }

    /// <summary>
    /// ������� ������� � ������� ����.
    /// </summary>
    /// <param name="target">����</param>
    private void RotateToTarget(Transform target)
    {
        var angle = Vector2.Angle(Vector2.right, target.position - transform.position); /// ���� ����� ����� � ����� ��������.
        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < target.position.y ? (angle - 90.0f) : (-angle - 90.0f));
    }

    /// <summary>
    /// ������� ���������� ��������.
    /// </summary>
    private void Accelerate()
    {
        if (_speed >= _walkSpeed) _speed = _walkSpeed;
        else _speed += 0.07f;
    }

    /// <summary>
    /// ���������� ��������� �� ������.
    /// </summary>
    private void DistanceToPlayer()
    {
        float distance = Vector2.Distance(transform.position, _target.transform.position);

        if (distance <= _distanceToPlayer)
            _moving = false;
    }

    /// <summary>
    /// ������.
    /// </summary>
    private void Death()
    {
        var health = gameObject.GetComponent<HealthScript>();

        if (health._death)
        {
            RandoScore();
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// ���� ������.
    /// </summary>
    private void RandoScore()
    {
        var score = Random.Range(65, 100) * _level;

        GameObject.Find("GameManager").GetComponent<ManagerScript>()._score += score;
    }
}
