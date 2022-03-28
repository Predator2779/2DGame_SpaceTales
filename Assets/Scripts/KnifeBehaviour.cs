using UnityEngine;

public class KnifeBehaviour : MonoBehaviour
{
    private bool _rotate = false;
    private Transform fromObj;

    private void Update()
    {
        if (_rotate)
            Rebound(fromObj);
        Removing();
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.gameObject.tag == "Knife")
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            fromObj = obj.transform;
            _rotate = true;
        }
    }

    private void Rebound(Transform obj)
    {
        Destroy(gameObject.GetComponent<Collider2D>());
        transform.Translate(Vector2.up * Time.deltaTime * 30.0f, Space.World);
        transform.Rotate(0, 0, Time.deltaTime * 1500.0f);
    }

    private void Removing()
    {
        float distance = Vector2.Distance(Vector2.zero, transform.position);

        if (distance > 8.0f)
            Destroy(this.gameObject);
    }
}
