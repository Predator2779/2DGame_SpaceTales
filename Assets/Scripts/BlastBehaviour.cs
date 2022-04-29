using UnityEngine;

public class BlastBehaviour : MonoBehaviour
{
    public float _koefDamage = 1.0f;

    /// <summary>
    /// Попадание.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var randValue = Random.Range(0.25f, 1.0f);

        HealthScript health;

        if (health = collision.gameObject.GetComponent<HealthScript>())
        {
            var damage = randValue / health._koefDefense * _koefDamage;
            health._points -= damage;
        }

        ShieldBehaviour shield;

        if (shield = collision.gameObject.GetComponent<ShieldBehaviour>())
        {
            shield._hit = true;
        }

        Destroy(this.gameObject);
    }
}
