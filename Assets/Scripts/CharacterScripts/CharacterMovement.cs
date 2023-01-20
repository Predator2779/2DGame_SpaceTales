using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    /// <summary>
    /// ���������� ������ ������.
    /// </summary>
    /// <param name="rbody">Rigidbody �������</param>
    /// <param name="currentSpeed">������� ��������</param>
    /// <param name="maxSpeed">����������� ��������</param>
    /// <param name="acceleration">���������</param>
    /// <returns>���������� ������� ��������.</returns>
    public float MoveForward(Rigidbody2D rbody, float currentSpeed, float maxSpeed, float acceleration)
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration;
        }
        else
        {
            currentSpeed = maxSpeed;
        }

        Vector2 motionVector = rbody.transform.up * currentSpeed * rbody.angularDrag * rbody.drag;

        rbody.AddForce(motionVector);;

        return currentSpeed;
    }

    /// <summary>
    /// ������� ������� � ������� ����.
    /// </summary>
    /// <param name="target">����</param>
    public void RotateToTarget(Transform target)
    {
        /// ���� ����� ����� � ����� ��������.
        var angle = Vector2.Angle(Vector2.right, target.position - transform.position);

        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < target.position.y ? (angle - 90.0f) : (-angle - 90.0f));
    }

    /// <summary>
    /// ������� ������� �� ��������� ����.
    /// </summary>
    /// <param name="angle">����</param>
    public void TurnOnAngle(float angle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    /// <summary>
    /// ���������� ��������� �� ����.
    /// </summary>
    /// <param name="ourObject">������, ������� ������ ���������� ���������</param>
    /// <param name="target">���� �������</param>
    /// <param name="distance">��������� �� �������</param>
    /// <returns>���������� ����������, ������������ ���������. 
    /// 0: ������ �� �����. 1: ��������� ������. -1: ��������� �����.</returns>
    public int KeepDistanseFromTarget(Transform ourObject, Transform target, float distance)
    {
        float currentDistance = Vector2.Distance(transform.position, target.position);

        /// ��������� ������� ������� ������ �������.
        float requiredDistance = target.localScale.y * distance;
        /// ����������� � 1/4 ����������� ���������.
        float variation = requiredDistance * 0.25f;

        float minDistance = requiredDistance - variation;
        float maxDistance = requiredDistance + variation;

        /// ���� ���������� 0: ������ ����� ������ �� ����� (������ ���������).
        /// ���� ���������� 1: ������ ����� ��������� ������, �� ���������� ������ ���������.
        /// ���� ���������� -1: ������ ����� ��������� �����, �� ���������� ������ ���������.
        if (minDistance < currentDistance && currentDistance < maxDistance)
        {
            return 0;
        }
        else if (currentDistance > maxDistance)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
