using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RandomBlink : MonoBehaviour
{
    private List<Image> imgs = new List<Image>();
    private Color zero = new Color(0, 0, 0, 0);

    private Coroutine blink;

    private void Awake() {
        imgs = GetComponentsInChildren<Image>().ToList();
        if (imgs.Contains(GetComponent<Image>())) {
            imgs.Remove(GetComponent<Image>());
        }
        foreach (Image img in imgs) {
            img.color = zero;
        }
    }

    private void OnEnable() {
        blink = StartCoroutine(Blink());
    }

    private void OnDisable() {
        StopCoroutine(blink);
    }

    private IEnumerator Blink() {
        int index = Random.Range(0, imgs.Count);
        imgs[index].color = Color.white;
        yield return new WaitForSeconds(0.35f);
        imgs[index].color = zero;
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.1f, 3f));
            index = Random.Range(0, imgs.Count);
            imgs[index].color = Color.white;
            yield return new WaitForSeconds(0.5f);
            imgs[index].color = zero;
        }
    }
}
