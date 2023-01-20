using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    /// <summary>
    /// Перемешает объект вперед.
    /// </summary>
    /// <param name="rbody">Rigidbody объекта</param>
    /// <param name="currentSpeed">Текущая скорость</param>
    /// <param name="maxSpeed">Достигаемая скорость</param>
    /// <param name="acceleration">Ускорение</param>
    /// <returns>Возвращает текущую скорость.</returns>
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
    /// Поворот объекта в сторону цели.
    /// </summary>
    /// <param name="target">Цель</param>
    public void RotateToTarget(Transform target)
    {
        /// Угол между целью и нашим объектом.
        var angle = Vector2.Angle(Vector2.right, target.position - transform.position);

        transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < target.position.y ? (angle - 90.0f) : (-angle - 90.0f));
    }

    /// <summary>
    /// Поворот объекта на указанный угол.
    /// </summary>
    /// <param name="angle">Угол</param>
    public void TurnOnAngle(float angle)
    {
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    /// <summary>
    /// Удерживать дистанцию до цели.
    /// </summary>
    /// <param name="ourObject">Объект, который должен удерживать дистанцию</param>
    /// <param name="target">Цель объекта</param>
    /// <param name="distance">Дистанция до объекта</param>
    /// <returns>Возвращает коэфициент, регулирующий дистанцию. 
    /// 0: стоять на месте. 1: двигаться вперед. -1: двигаться назад.</returns>
    public int KeepDistanseFromTarget(Transform ourObject, Transform target, float distance)
    {
        float currentDistance = Vector2.Distance(transform.position, target.position);

        /// Учитываем влияние размера нашего объекта.
        float requiredDistance = target.localScale.y * distance;
        /// Погрешность в 1/4 необходимой дистанции.
        float variation = requiredDistance * 0.25f;

        float minDistance = requiredDistance - variation;
        float maxDistance = requiredDistance + variation;

        /// Если возвращает 0: объект будет стоять на месте (нужная дистанция).
        /// Если возвращает 1: объект будет двигаться вперед, до достижения нужной дистанции.
        /// Если возвращает -1: объект будет двигаться назад, до достижения нужной дистанции.
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
