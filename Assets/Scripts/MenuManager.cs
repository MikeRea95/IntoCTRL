using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject exitText;

    public void Exit()
    {
#if UNITY_WEBGL
        exitText.SetActive(true);
#else
        Application.Quit();
#endif
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
