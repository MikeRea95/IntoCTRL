using UnityEngine;
using UnityEngine.UI;

public class CorruptionCombat : MonoBehaviour, IEnemyCombat {

    [SerializeField]
    private int health;
    KeyboardKey myKey;

    public void AddKeyReference(KeyboardKey key) {
        myKey = key;
    }

    public void Die() {
        transform.GetChild(1).GetComponent<ScoreFloatText>().Enable(100);
        Transform particles = transform.GetChild(0);
        particles.gameObject.SetActive(true);
        particles.parent = transform.parent;
        particles.localPosition = Vector3.zero;
        particles.localScale = Vector3.one;
        GameManager.instance.AddScore(100);
        Destroy(gameObject);
    }

    public void DieWithoutPoints() {
        Transform particles = transform.GetChild(0);
        particles.gameObject.SetActive(true);
        particles.parent = transform.parent;
        particles.localPosition = Vector3.zero;
        particles.localScale = Vector3.one;
        Destroy(gameObject);
    }

    public void EnemyAttacks() {
        PlayerHealth.instance.ApplyDamage(0);
    }

    public string Name() {
        return gameObject.name;
    }

    public bool PlayerAttacks() {
        EnemyAttacks();
        health--;
        if (PlayerBoons.instance.hasPowerup[PowerupType.Attack]) {
            health--;
        }
        if (health <= 0) {
            Die();
            return true;
        }

        return false;
    }

    public void Hide() {
        GetComponent<Image>().enabled = false;
    }

    public void Show() {
        GetComponent<Image>().enabled = true;
    }
}