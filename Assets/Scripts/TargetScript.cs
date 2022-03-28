using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] [Range(-10.0f, 10.0f)] private float _speed = 1.0f;

    [SerializeField] private bool _Destroy = true;

    void Update()
    {
        Rotation();

        if (_Destroy)
            DestroyTarget();
    }

    private void Rotation() => transform.Rotate(0, 0, _speed * Time.deltaTime * 100.0f);

    private void DestroyTarget()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "TargetSprite")
                Destroy(child.gameObject);
            else
            {
                Vector2 movingVec = new Vector2(0, 0);
                transform.Translate(movingVec, Space.World);
                child.parent = null;
                Vector2 explosionVec = child.transform.position - transform.position;
                child.gameObject.AddComponent<Rigidbody2D>();
                child.GetComponent<Rigidbody2D>().gravityScale = 10.0f;
                child.GetComponent<Rigidbody2D>().mass = 10.0f;
                child.GetComponent<Rigidbody2D>().AddForce(explosionVec * 50.0f, ForceMode2D.Impulse);
            }
        }

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D knife)
    {
        Transform freezeTransform = knife.transform;
        Destroy(knife.transform.GetComponent<Rigidbody2D>());
        Destroy(knife.transform.GetComponent<KnifeBehaviour>());
        knife.transform.SetParent(this.transform);
        knife.transform.position = freezeTransform.transform.position;
        knife.transform.rotation = freezeTransform.transform.rotation;
    }
}
