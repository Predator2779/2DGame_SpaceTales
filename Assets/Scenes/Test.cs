using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        StartCoroutine(Timer(5));
    }

    /// <summary>
    ///  Таймер.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Timer(float time)
    {
        while (time > 0.0f)
        {
            print(time);

            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }
}
