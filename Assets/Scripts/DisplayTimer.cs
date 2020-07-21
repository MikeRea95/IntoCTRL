using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayTimer : MonoBehaviour
{
    public static DisplayTimer instance;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Timer levelTime;

    float minutes;
    float seconds;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        minutes = levelTime.GetStartTime() / 60f;
        seconds = levelTime.GetStartTime() % 60;
    }

    // Update is called once per frame
    void Update()
    {
        minutes = (int)(levelTime.GetCurrentTime() / 60f);
        seconds = (int)(levelTime.GetCurrentTime() % 60);

        timerText.text = minutes + ":" + seconds.ToString("00");
    }

    public int GetSeconds()
    {
        return Mathf.FloorToInt(seconds);
    }
}
