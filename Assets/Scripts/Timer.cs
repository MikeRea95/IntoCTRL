using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float levelTime = 60f;
    [SerializeField]
    private bool timeOut;

    private float currentTime;

    void Awake()
    {
        currentTime = levelTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeOut && !PlayerBoons.instance.hasPowerup[PowerupType.Time])
        {
            currentTime -= Time.deltaTime;
        }

        if(currentTime <= 0f)
        {
            TimeOut();
        }
    }

    public float GetStartTime()
    {
        return levelTime;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
    }

    public void TimeOut()
    {
        timeOut = true;
    }

    public void TimeIn()
    {
        timeOut = false;
    }
}
