using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalScore;

    public int Score;
    [SerializeField]
    private EnemyController EC;
    [SerializeField]
    private PlayerController PC;
    [SerializeField]
    private PlayerHealth PH;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI totalScoreText;
    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private List<KeyboardKey> keysToFade = new List<KeyboardKey>();
    [SerializeField]
    private List<TextMeshProUGUI> keysTextToFade = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<KeyAndText> keysAndText = new List<KeyAndText>();

    public bool gameOver = false;
    public bool staggerFading = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        AddScore(0);
        ResetKeys();
    }

    private void ResetKeys()
    {
        keysToFade.Clear();
        keysTextToFade.Clear();
        keysAndText.Clear();
        keysToFade.AddRange(GameObject.FindObjectsOfType<KeyboardKey>());
        for(int i = 0; i < keysToFade.Count; i++)
        {
            keysTextToFade.Add(keysToFade[i].GetComponentInChildren<TextMeshProUGUI>());
        }
        for (int i = 0; i < keysToFade.Count; i++)
        {
            KeyAndText newEntry = new KeyAndText();
            newEntry.keyImage = keysToFade[i].GetComponent<Image>();
            newEntry.tmpugui = keysTextToFade[i];
            keysAndText.Add(newEntry);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer.GetCurrentTime() <= 0f)
        {
            TimeUp();
        }

        if (gameOver)
        {
            GameOver();
        }

        totalScoreText.text = TotalScore.instance.totalScore.ToString();
    }

    public void UpdateTurn()
    {
        
    }

    public void GameOver()
    {
        if (!staggerFading)
        {
            foreach (KeyboardKey key in keysToFade)
            {
                key.GetComponent<Image>().DOFade(0f, 1f);
            }

            foreach (TextMeshProUGUI text in keysTextToFade)
            {
                text.DOFade(0f, 1f);
            }
        }
        else
        {
            for (int i = 0; i < keysAndText.Count; i++)
            {
                float delay = Random.Range(0, 3f);
                print(delay);
                StartCoroutine(FadeImage(delay, keysAndText[i].keyImage));
                StartCoroutine(FadeText(delay, keysAndText[i].tmpugui));
            }
        }
        //gameOverScreen.GetComponent<TextMeshProUGUI>().DOFade(1f, 2f);
        gameOverScreen.SetActive(true);
    }

    private IEnumerator FadeImage(float delay,Image img)
    {
        yield return new WaitForSeconds(delay);

        while(img.color.a > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            img.color -= new Color(0f,0f,0f,0.05f);
        }

    }

    private IEnumerator FadeText(float delay, TextMeshProUGUI tmpugu)
    {
        yield return new WaitForSeconds(delay);
        while (tmpugu.color.a > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            tmpugu.color -= new Color(0f, 0f, 0f, 0.05f);
        }
    }

    public void EndLevel()
    {
        timer.TimeOut();
    }

    public void TimeUp()
    {
        PH.ApplyDamage(100);
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if (PlayerBoons.instance.hasPowerup[PowerupType.Score]) {
            Score += amount;
        }

        scoreText.text = Score.ToString();
    }
}

[System.Serializable]
public class KeyAndText
{
    public Image keyImage;
    public TextMeshProUGUI tmpugui;
}
