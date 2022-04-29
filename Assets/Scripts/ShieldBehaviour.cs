using UnityEngine;
using UnityEngine.UI;

public class ShieldBehaviour : HealthScript
{
    public bool _hit = false;

    /// <summary>
    /// Update.
    /// </summary>
    void Update()
    {
        PositionShield();
        ActivityShield();
        Health();
        Death();
        DestroyShield();
    }

    /// <summary>
    /// �������� ������� ����.
    /// </summary>
    private void PositionShield()
    {
        var carrier = gameObject.transform.parent.gameObject;

        transform.position = carrier.transform.position;
    }

    /// <summary>
    /// ��������� � ���������� ����.
    /// </summary>
    private void ActivityShield()
    {
        var color = gameObject.GetComponent<SpriteRenderer>().color;

        if (_hit)
        {
            color.a = 0.235f;

            _hit = false;
        }
        else
            color.a = 0.0f;

        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    /// <summary>
    /// ���������� ����.
    /// </summary>
    private void DestroyShield()
    {
        if (_death)
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
