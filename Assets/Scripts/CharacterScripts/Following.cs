using UnityEngine;

public class Following : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private bool _followForTarget = true;
    [SerializeField] private bool _rotateWithTarget = false;
 
    void Update()
    {
        if (_followForTarget)
        {
            transform.position = _target.position;
        }

        if (_rotateWithTarget)
        {
            transform.rotation = _target.rotation;
        }
    }
}
