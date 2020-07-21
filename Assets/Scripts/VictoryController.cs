using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryController : MonoBehaviour
{
    //roygbiv
    public List<KeyboardKey> keys = new List<KeyboardKey>();
    //success
    public GameObject player;
    public GameObject fadeOut;
    public GameObject successScreen;
    //scoring
    public GameObject scoreScreen;
    public TextMeshProUGUI timeBonus;
    private int timeNum = 0;
    public TextMeshProUGUI score;
    private int scoreNum = 0;
    public TextMeshProUGUI total;
    private int totalNum = 0;

    public IEnumerator LevelComplete() {
        GameManager.instance.EndLevel();
        StartCoroutine(LoadNextLevel());
        for (int i = 0; i < 5; i++) {
            keys[i].SetKeyState(KeyState.Rainbow);
        }
        yield return new WaitForSeconds(0.15f);
        int j = 5;
        while (j < 52) {
            for (int k = j; j < k + 4 && j < 52; j++) {
                keys[j].SetKeyState(KeyState.Rainbow);
            }
            yield return new WaitForSeconds(0.15f);
        }
        keys[52].SetKeyState(KeyState.Rainbow);
        keys[53].SetKeyState(KeyState.Rainbow);
    }

    private IEnumerator LoadNextLevel()
    {
        //Play victory animation
        successScreen.SetActive(true);
        player.GetComponent<Animator>().SetBool("Success", true);
        scoreScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        //Tally score
        while(timeNum < DisplayTimer.instance.GetSeconds())
        {
            yield return new WaitForSeconds(0.5f * (1f / DisplayTimer.instance.GetSeconds()));
            timeNum += 1;
            timeBonus.text = timeNum.ToString();
        }
        while (scoreNum < GameManager.instance.Score)
        {
            yield return new WaitForSeconds(0.05f * (1f/ GameManager.instance.Score));
            scoreNum += 1;
            score.text = scoreNum.ToString();
        }
        yield return new WaitForSeconds(0.5f);
        while (timeNum > 0)
        {
            yield return new WaitForSeconds(0.5f * (1f / DisplayTimer.instance.GetSeconds()));
            totalNum += 10;
            timeNum -= 1;
            timeBonus.text = timeNum.ToString();
            total.text = totalNum.ToString();
        }
        while (scoreNum > 0)
        {
            yield return new WaitForSeconds(0.05f * (1f / GameManager.instance.Score));
            totalNum += 1;
            scoreNum -= 1;
            total.text = totalNum.ToString();
            score.text = scoreNum.ToString();
        }
        TotalScore.instance.totalScore += totalNum;
        totalNum = 0;
        //Fade out
        fadeOut.GetComponent<Animator>().SetBool("FadeIn", false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}