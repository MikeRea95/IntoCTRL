using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrojanCombat : MonoBehaviour, IEnemyCombat
{
    [SerializeField]
    private int health;

    private KeyboardKey myKey;

    public string Name() {
        return gameObject.name;
    }

    public void EnemyAttacks() {
        if (PlayerBoons.instance.hasPowerup[PowerupType.Time]) {
            PlayerHealth.instance.ApplyDamage(0);
            return;
        }
        StartCoroutine(waitToAttack());
    }

    private IEnumerator waitToAttack() {
        yield return new WaitForSeconds(0.21f);
        transform.DOLocalMoveX(transform.eulerAngles.y == 0 ? 3f : -3f, 0.1f).SetEase(Ease.InQuint).OnComplete(() => Attack());
    }

    private void Attack() {
        PlayerHealth.instance.ApplyDamage(1);
        transform.DOLocalMoveX(0, 0.2f).SetEase(Ease.OutQuint);
    }

    public bool PlayerAttacks() {
        if(PlayerHealth.instance.transform.position.x > transform.position.x) {
            transform.DORotate(Vector3.zero, 0.2f);
        }
        else {
            transform.DORotate(new Vector3(0, 180f, 0), 0.2f);
        }

        health--;
        if (PlayerBoons.instance.hasPowerup[PowerupType.Attack]) {
            health--;
        }
        if(health <= 0) {
            Die();
            return true;
        }
        else {
            EnemyAttacks();
            return false;
        }
    }

    public void Die() {
        List<KeyboardKey> possibleSpawns = new List<KeyboardKey>();
        foreach (KeyboardKey key in myKey.adjacentKeys) {
            if (key.IsEmpty()) {
                possibleSpawns.Add(key);
            }
        }
        if(possibleSpawns.Count < 3) {
            foreach(KeyboardKey key in possibleSpawns) {
                GameObject newEnemy = Instantiate(Resources.Load("EnemyPrefabs/Corruption", typeof(GameObject))) as GameObject;
                newEnemy.transform.parent = key.transform;
                newEnemy.transform.position = transform.position;
                newEnemy.transform.localScale = (Resources.Load("EnemyPrefabs/Corruption", typeof(Transform)) as Transform).localScale;
                newEnemy.transform.DOMove(key.originalPos, 1f).SetEase(Ease.OutQuad).OnComplete(() => RestorePlayerControls());
            }
        }
        else {
            int numToSpawn = 0;
            int total = Mathf.Min(SceneManager.GetActiveScene().buildIndex % 3 + 2, possibleSpawns.Count);
            print(total);
            while(numToSpawn < total) {
                List<KeyboardKey> toRemove = new List<KeyboardKey>();
                foreach(KeyboardKey key in possibleSpawns) {
                    if(Random.Range(0, 2) == 1) {
                        GameObject newEnemy = Instantiate(Resources.Load("EnemyPrefabs/Corruption", typeof(GameObject))) as GameObject;
                        newEnemy.transform.parent = key.transform;
                        newEnemy.transform.position = myKey.originalPos;
                        newEnemy.transform.localScale = (Resources.Load("EnemyPrefabs/Corruption", typeof(Transform)) as Transform).localScale;
                        newEnemy.transform.DOMove(key.originalPos, 1f).SetEase(Ease.InOutCirc).OnComplete(()=>RestorePlayerControls());
                        toRemove.Add(key);
                        numToSpawn++;
                        if(numToSpawn == total) {
                            break;
                        }
                    }
                }
                foreach(KeyboardKey key in toRemove) {
                    possibleSpawns.Remove(key);
                }
            }
        }

        transform.GetChild(1).GetComponent<ScoreFloatText>().Enable(300);

        Transform particles = transform.GetChild(0);
        particles.gameObject.SetActive(true);
        particles.parent = transform.parent;
        particles.localPosition = Vector3.zero;
        particles.localScale = Vector3.one;
        GameManager.instance.AddScore(300);
        Destroy(gameObject);
    }

    private void RestorePlayerControls() {
        PlayerHealth.instance.ApplyDamage(0);
    }

    public void AddKeyReference(KeyboardKey newKey) {
        myKey = newKey;
    }

    public void DieWithoutPoints() {
        Transform particles = transform.GetChild(0);
        particles.gameObject.SetActive(true);
        particles.parent = transform.parent;
        particles.localPosition = Vector3.zero;
        particles.localScale = Vector3.one;

        Destroy(gameObject);
    }

    public void Hide() {
        GetComponent<Image>().enabled = false;
    }

    public void Show() {
        GetComponent<Image>().enabled = true;
    }
}