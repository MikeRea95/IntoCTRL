using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    public static Screenshake instance;

    private Vector3 startPos;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        transform.localPosition = startPos;
    }

    public void Shake(float intensity)
    {
        StartCoroutine(DoShake(intensity/5f,intensity));
    }

    private IEnumerator DoShake(float duration, float intensity)
    {
        transform.localPosition = startPos;
        duration = 0.4f;
        while(duration > 0f)
        {
            yield return new WaitForEndOfFrame();
            duration -= Time.deltaTime;
            //Do a shake
            transform.localPosition = startPos + Random.insideUnitSphere * intensity;
        }
        transform.localPosition = startPos;
    }
}
