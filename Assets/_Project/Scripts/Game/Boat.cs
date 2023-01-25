using System.Collections;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] float duration = .8f;

    public IEnumerator Move()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        Vector3 start = transform.position,
            end = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        float time = 0f, fac;

        while (time < duration)
        {
            time += Time.deltaTime;
            fac = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(Mathf.InverseLerp(0, duration, time));
            transform.position = Vector3.Lerp(start, end, fac);

            yield return wait;
        }
    }
}