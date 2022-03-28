using UnityEngine;

public class LifeScript : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D obj)
    {
        Debug.Log(obj.gameObject.name);
        Destroy(obj.gameObject);
    }
}
