using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private bool active = false;
    private void OnEnable()
    {
        active = true;
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                SceneManager.LoadScene(1);
                TotalScore.instance.totalScore = 0;
            }
        }
    }
}
