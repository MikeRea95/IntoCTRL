using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreFloatText : MonoBehaviour
{
    private bool alive = true;

    private int score;

    private void Update() {
        // Negate any screen flipping or sprite flipping
        transform.eulerAngles = Vector3.zero;
    }

    public void Enable(int _score) {
        gameObject.SetActive(true);
        score = _score;
        if (PlayerBoons.instance.hasPowerup[PowerupType.Score]) {
            score += _score;
            transform.localScale = new Vector3(3, 3, 3);
            transform.DOMove(transform.position + Vector3.up * 50f, 0.75f).SetEase(Ease.InOutQuad).OnComplete(() => alive = false);
            StartCoroutine(fancyScore());
        }
        else {
            transform.DOMove(transform.position + Vector3.up * 50f, 0.75f).SetEase(Ease.InOutQuad).OnComplete(() => Destroy(gameObject));
        }

        GetComponent<TextMeshProUGUI>().text = "+" + score;
        transform.parent = transform.parent.parent.parent.parent;
    }

    private IEnumerator fancyScore() {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        transform.GetChild(0).gameObject.SetActive(true);
        TextMeshProUGUI lowerText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        lowerText.text = "+" + score;
        print(score);
        Color blue;
        ColorUtility.TryParseHtmlString("#A8D9E0", out blue);
        Color red;
        ColorUtility.TryParseHtmlString("#DF506C", out red);
        while (alive) {
            text.color = blue;
            lowerText.color = red;
            yield return new WaitForSeconds(0.1f);
            text.color = red;
            lowerText.color = blue;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
