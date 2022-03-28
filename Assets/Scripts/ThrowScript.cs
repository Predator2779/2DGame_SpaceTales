using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Collections;

public class ThrowScript : MonoBehaviour
{
    [SerializeField] private GameObject _knife;
    [SerializeField] private Text _timer;

    private GameObject _cloneKnife;
    private float _time = 0;
    private bool _fire = true;

    private void Start() => _cloneKnife = Instantiate(_knife, transform.position, transform.rotation);

    private void Update()
    {
        if (/*Input.touchCount == 1 || */Input.GetKey(KeyCode.Space) && _fire)
            Timer();
        if (Input.GetKeyUp(KeyCode.Space))
            Throw();
    }

    private void ThrowKnife() 
    {
        _fire = false;
        _cloneKnife.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 50.0f, ForceMode2D.Impulse);
        _cloneKnife.AddComponent<KnifeBehaviour>();
        StartCoroutine(SpawnKnife());
    }

    private void TwistedThrowKnife()
    {
        _fire = false;
        transform.Rotate(0, 0, Time.deltaTime * 1500.0f);
        _cloneKnife.GetComponent<Rigidbody2D>().AddForce(Vector2.zero * 1.0f, ForceMode2D.Impulse);        /////
        _cloneKnife.AddComponent<KnifeBehaviour>();
        StartCoroutine(SpawnKnife());
    }

    public void Timer()
    {
        _timer.text = _time.ToString();
        _time += Time.deltaTime;
    }

    public void Throw()
    {
        if (_time < 0.8f)
            ThrowKnife();
        else
            TwistedThrowKnife();
    }

    IEnumerator SpawnKnife()
    {
        yield return new WaitForSeconds(0.07f);
        _cloneKnife = Instantiate(_knife, transform.position, transform.rotation);
        _cloneKnife.transform.rotation = Quaternion.identity;
            //new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);
        _time = 0;
        _fire = true;
    }
}
