using UnityEngine;

public class VisibilityZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            transform.parent.GetComponent<EnemyBehaviour>()._target = obj.transform;
            transform.parent.GetComponent<EnemyBehaviour>()._patrol = false;
        }
    }
}
