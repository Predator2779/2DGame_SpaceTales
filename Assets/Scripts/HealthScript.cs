using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public bool _death = false;
    public float _koefDefense = 1.0f;
    [Range(0.0f, 100.0f)] public float _points = 100.0f;

    [SerializeField] private Scrollbar _pointBar;

    /// <summary>
    /// Update.
    /// </summary>
    private void Update()
    {
        Health();
        Death();
    }

    /// <summary>
    /// Çהמנמגüו.
    /// </summary>
    public void Health()
    {
        _pointBar.size = _points * 0.01f;
    }

    /// <summary>
    /// Óסכמגטÿ סלונעט.
    /// </summary>
    public void Death()
    {
        if (_points <= 0.0f)
        {
            _pointBar.transform.Find("Sliding Area").gameObject.SetActive(false);
            _death = true;
        }
        else
        {
            _pointBar.transform.Find("Sliding Area").gameObject.SetActive(true);
            _death = false;
        }
    }
}
