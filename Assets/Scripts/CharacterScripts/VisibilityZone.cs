using UnityEngine;

public class VisibilityZone : MonoBehaviour
{
    public Transform Target;

    private void OnTriggerStay2D(Collider2D obj) /////�������� �������.
    {
        if (obj.gameObject.tag == "Player")
        {
            Target = obj.transform;
        }
    }
}
