using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpywareCombat : MonoBehaviour, IEnemyCombat {

    [SerializeField]
    private int health;
    private KeyboardKey myKey;

    private bool invisible = true;

    public void AddKeyReference(KeyboardKey key) {
        myKey = key;
    }

    public void Die() {
        transform.GetChild(1).GetComponent<ScoreFloatText>().Enable(1000);
        PlayerHealth.instance.ApplyDamage(0);
        Transform particles = transform.GetChild(0);
        particles.gameObject.SetActive(true);
        particles.parent = transform.parent;
        particles.localPosition = Vector3.zero;
        particles.localScale = Vector3.one;
        GameManager.instance.AddScore(1000);
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
        if (PlayerBoons.instance.hasPowerup[PowerupType.Time]) {
            PlayerHealth.instance.ApplyDamage(0);
            return;
        }
        PlayerHealth.instance.ApplyDamage(1);
    }

    private void AttackAndRun() {
        if (PlayerBoons.instance.hasPowerup[PowerupType.Time]) {
            PlayerHealth.instance.ApplyDamage(0);
            return;
        }
        PlayerHealth.instance.ApplyDamage(1);

        List<DesiredDirection> directions = new List<DesiredDirection>();

        Transform player = PlayerHealth.instance.transform;
        if (player.position.x > transform.position.x) {
            directions.Add(DesiredDirection.Left);
        }
        else {
            directions.Add(DesiredDirection.Right);
        }
        // Don't add up/down if the y positions are equal.
        if(player.position.y > transform.position.y) {
            directions.Add(DesiredDirection.Down);
        }
        else if(player.position.y < transform.position.y) {
            directions.Add(DesiredDirection.Up);
        }

        KeyboardKey keyToGoTo = myKey;
        foreach(KeyboardKey key in myKey.adjacentKeys) {
            if (!key.IsEmpty() || key.myCode == KeyCode.LeftControl) {
                continue;
            }
            List<DesiredDirection> potentialDirections = new List<DesiredDirection>();
            if(directions.Contains(DesiredDirection.Up) && key.transform.position.y > myKey.transform.position.y) {
                potentialDirections.Add(DesiredDirection.Up);
            }
            else if(directions.Contains(DesiredDirection.Down) && key.transform.position.y < myKey.transform.position.y) {
                potentialDirections.Add(DesiredDirection.Down);
            }

            if(directions.Contains(DesiredDirection.Left) && key.transform.position.x <= myKey.transform.position.x) {
                potentialDirections.Add(DesiredDirection.Left);
            }
            else if(directions.Contains(DesiredDirection.Right) && key.transform.position.x > myKey.transform.position.x) {
                potentialDirections.Add(DesiredDirection.Right);
            }

            if(potentialDirections.Count == 2) {
                keyToGoTo = key;
                break;
            }
            else if (potentialDirections.Count == 1){
                keyToGoTo = key;
            }
        }

        KeyboardKey secondKeyToGoTo = keyToGoTo;

        foreach (KeyboardKey key in keyToGoTo.adjacentKeys) {
            if (!key.IsEmpty() || key.myCode == KeyCode.LeftControl) {
                continue;
            }
            List<DesiredDirection> potentialDirections = new List<DesiredDirection>();
            if (directions.Contains(DesiredDirection.Up) && key.transform.position.y > keyToGoTo.transform.position.y) {
                potentialDirections.Add(DesiredDirection.Up);
            }
            else if (directions.Contains(DesiredDirection.Down) && key.transform.position.y < keyToGoTo.transform.position.y) {
                potentialDirections.Add(DesiredDirection.Down);
            }

            if (directions.Contains(DesiredDirection.Left) && key.transform.position.x <= keyToGoTo.transform.position.x) {
                potentialDirections.Add(DesiredDirection.Left);
            }
            else if (directions.Contains(DesiredDirection.Right) && key.transform.position.x > keyToGoTo.transform.position.x) {
                potentialDirections.Add(DesiredDirection.Right);
            }

            if (potentialDirections.Count == 2) {
                secondKeyToGoTo = key;
                break;
            }
            else if (potentialDirections.Count == 1) {
                secondKeyToGoTo = key;
            }
        }

        transform.DOMove(keyToGoTo.transform.position, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        transform.DOMove(secondKeyToGoTo.transform.position, 0.2f).SetEase(Ease.Linear).OnComplete(() => MoveParentOnRetreat(secondKeyToGoTo)));
    }

    private void MoveParentOnRetreat(KeyboardKey newKey) {
        transform.parent = newKey.transform;
        myKey = newKey;

        if (!myKey.seen) {
            Hide();
        }
    }

    public string Name() {
        return gameObject.name;
    }

    public bool PlayerAttacks() {
        if (invisible) {
            invisible = false;
            Show();
            AttackAndRun();
            return true;
        }
        else {
            health--;
            if (PlayerBoons.instance.hasPowerup[PowerupType.Attack]) {
                health--;
            }

            if(health <= 0) {
                Die();
                return true;
            }
            else {
                AttackAndRun();
                return false;
            }
        }
    }

    public void Hide() {
        GetComponent<Image>().enabled = false;
    }

    public void Show() {
        GetComponent<Image>().enabled = true;
        if (invisible) {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
        }
        else {
            GetComponent<Image>().color = Color.white;
        }
    }
}

enum DesiredDirection {
    Up,
    Down,
    Left,
    Right
}