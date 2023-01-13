using UnityEngine;

public class AimControl : MonoBehaviour
{
    /// <summary>
    /// Update.
    /// </summary>
    private void FixedUpdate()
    {
        AimPosition();
        AimLimit();
    }

    /// <summary>
    /// Позиция прицела.
    /// </summary>
    private void AimPosition()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = mousePos;
    }

    /// <summary>
    /// Ограничение позиции по экрану.
    /// </summary>
    private void AimLimit()
    {
        var minPos = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        var maxPos = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (transform.position.x > maxPos.x)
            transform.position = new Vector2(maxPos.x, transform.position.y);

        if (transform.position.y > maxPos.y)
            transform.position = new Vector2(transform.position.x, maxPos.y);

        if (transform.position.x < minPos.x)
            transform.position = new Vector2(minPos.x, transform.position.y);

        if (transform.position.y < minPos.y)
            transform.position = new Vector2(transform.position.x, minPos.y);
    }
}
