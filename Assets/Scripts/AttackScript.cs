using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public bool _fire = false;                                            /// Выстрелить.

    [SerializeField] private ManagerScript _gameManager;                  /// GameManager.
    [SerializeField] private GameObject _typeGun;                         /// Тип оружия.
    [SerializeField] private GameObject _cloneBullet;                     /// Пуля.
    [SerializeField] private float _forceShot = 250.0f;                   /// Сила выстрела.
    [SerializeField] private float _damage = 1.0f;                        /// Урон пушки.
    [SerializeField] private float _backTime = 0.15f;                     /// Время отката.
    [SerializeField] private float _realBackTime = 0.15f;                 /// Реальное время отката.
    [SerializeField] private float _scaleBullet;                          /// Размер пули.
    [SerializeField] private bool _firePermission = true;                 /// Разрешение вести огонь.
    [SerializeField] private bool _shootBack = false;                     /// Откат выстрела.

    /// <summary>
    /// Start.
    /// </summary>
    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<ManagerScript>();
    }

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        Shot();
        ShootBackTimer();
    }

    /// <summary>
    /// Выстрел.
    /// </summary>
    public void Shot()
    {
        if (_firePermission && _fire)
        {
            _fire = false;

            _cloneBullet = Instantiate(_typeGun, transform.position, Quaternion.identity);
            _cloneBullet.transform.localScale = new Vector2(_scaleBullet, _scaleBullet);
            _cloneBullet.GetComponent<Rigidbody2D>().AddForce(transform.up * _forceShot, ForceMode2D.Impulse);
            _cloneBullet.GetComponent<BlastBehaviour>()._koefDamage = _damage;
            _cloneBullet.gameObject.layer = gameObject.layer;

            _realBackTime = _backTime;
            _shootBack = true;
        }
    }

    /// <summary>
    /// Таймер отката  выстрела.
    /// </summary>
    private void ShootBackTimer()
    {
        if (_shootBack)
        {
            _firePermission = false;

            _realBackTime -= Time.deltaTime;

            if (_realBackTime <= 0)
            {
                _firePermission = true;
                _shootBack = false;
                _realBackTime = _backTime;
            }
        }
    }
}
