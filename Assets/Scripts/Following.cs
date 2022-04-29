using UnityEngine;

public class Following : MonoBehaviour
{
    [SerializeField] private Transform _target;
 
    /// <summary>
    /// Update.
    /// </summary>
    void Update()
    {
        transform.position = _target.position;
    }
}
