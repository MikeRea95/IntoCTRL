using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class RansomwareCombat : MonoBehaviour, IEnemyCombat {

    [SerializeField]
    private int health;
    private KeyboardKey myKey;
    private Animator anim;

    private Transform keyboardTransform;

    private void Awake() {
        anim = GetComponent<Animator>();
        keyboardTransform = GetComponentInParent<Screenshake>().transform;
    }

    public void AddKeyReference(KeyboardKey key) {
        myKey = key;
    }

    public void Die() {
        if (keyboardTransform.eulerAngles != Vector3.zero) {
            keyboardTransform.DORotate(Vector3.zero, 1f).SetEase(Ease.InOutBack);
        }
        transform.GetChild(1).GetComponent<ScoreFloatText>().Enable(500);
        PlayerHealth.instance.ApplyDamage(0);
        Transform particles = transform.GetChild(0);
        particles.gameObject.SetActive(true);
        particles.parent = transform.parent;
        particles.localPosition = Vector3.zero;
        particles.localScale = Vector3.one;
        GameManager.instance.AddScore(500);
        Destroy(gameObject);
    }

    public void DieWithoutPoints() {
        if (keyboardTransform.eulerAngles != Vector3.zero) {
            keyboardTransform.DORotate(Vector3.zero, 1f).SetEase(Ease.InOutBack);
        }
        Transform particles = transform.GetChild(0);
        particles.gameObject.SetActive(true);
        particles.parent = transform.parent;
        particles.localPosition = Vector3.zero;
        particles.localScale = Vector3.one;

        Destroy(gameObject);
    }

    public void EnemyAttacks() {
        if (PlayerBoons.instance.hasPowerup[PowerupType.Time]) {
            PlayerHealth.instance.ApplyDamage(0);
            return;
        }
        StartCoroutine(waitForAttackAnim());
    }

    private IEnumerator waitForAttackAnim() {
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(0.2f);
        PlayerHealth.instance.ApplyDamage(1);
        anim.SetBool("Attack", false);
        RotateScreen();
    }

    private void RotateScreen() {
        if (keyboardTransform.eulerAngles != Vector3.zero) {
            return;
        }
        float x = 0f;
        float y = 0f;
        float z = 0f;
        do {
            x = Random.Range(0, 2) == 1 ? 180f : 0f;
            y = Random.Range(0, 2) == 1 ? 180f : 0f;
            z = Random.Range(0, 2) == 1 ? 180f : 0f;
        } while ((x == 0f && y == 0f && z == 0f) ||
        (x == 180f && y == 180f && z == 180f));
        keyboardTransform.DORotate(new Vector3(x, y, 0), 1f).SetEase(Ease.InOutBack);
    }

    private IEnumerator waitToUndoRotate() {
        yield return new WaitForSeconds(5f);
        keyboardTransform.DORotate(Vector3.zero, 1f).SetEase(Ease.InOutBack);
    }

    public string Name() {
        return gameObject.name;
    }

    public bool PlayerAttacks() {
        if(PlayerHealth.instance.transform.position.x > transform.position.x) {
            transform.DORotate(new Vector3(0, 180, 0), 0.2f);
        }
        else {
            transform.DORotate(Vector3.zero, 0.2f);
        }

        health--;
        if (PlayerBoons.instance.hasPowerup[PowerupType.Attack]) {
            health--;
        }
        if (health <= 0) {
            Die();
            return true;
        }
        else {
            EnemyAttacks();
            return false;
        }
    }

    public void Hide() {
        GetComponent<Image>().enabled = false;
    }

    public void Show() {
        GetComponent<Image>().enabled = true;
    }
}