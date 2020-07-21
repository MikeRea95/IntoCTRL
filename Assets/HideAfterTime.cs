using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAfterTime : MonoBehaviour
{
    [SerializeField]
    private float timeToHide;

    RectTransform rect;

    private void Awake() {
        rect = transform as RectTransform;
    }

    private void OnEnable() {
        rect.anchoredPosition = new Vector2(0, -100);
        rect.DOAnchorPos(Vector2.zero, timeToHide).SetEase(Ease.OutQuint).OnComplete(()=>
        StartCoroutine(waitToLeave()));
    }

    private IEnumerator waitToLeave() {
        yield return new WaitForSeconds(timeToHide/2f);
        rect.DOAnchorPos(new Vector2(0,-100), timeToHide).SetEase(Ease.InQuint).OnComplete(() =>
        gameObject.SetActive(false));
    }
}