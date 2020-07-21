using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoons : MonoBehaviour {

    public Dictionary<PowerupType, bool> hasPowerup = new Dictionary<PowerupType, bool>();
    public Dictionary<PowerupType, float> timeLeft = new Dictionary<PowerupType, float>();
    [SerializeField]
    private float attackTimer;
    [SerializeField]
    private float defenseTimer;
    [SerializeField]
    private float timeTimer;
    [SerializeField]
    private float scoreTimer;

    public static PlayerBoons instance;

    private void Awake() {
        instance = this;
        hasPowerup.Add(PowerupType.Attack, false);
        timeLeft.Add(PowerupType.Attack, 0f);
        hasPowerup.Add(PowerupType.Defense, false);
        timeLeft.Add(PowerupType.Defense, 0f);
        hasPowerup.Add(PowerupType.Time, false);
        timeLeft.Add(PowerupType.Time, 0f);
        hasPowerup.Add(PowerupType.Score, false);
        timeLeft.Add(PowerupType.Score, 0f);
    }

    public float GetTimeLeftPercentage(PowerupType type) {
        switch (type) {
            case PowerupType.Attack:
                return timeLeft[type] / attackTimer;
            case PowerupType.Defense:
                return timeLeft[type] / defenseTimer;
            case PowerupType.Time:
                return timeLeft[type] / timeTimer;
            case PowerupType.Score:
                return timeLeft[type] / scoreTimer;
            default:
                return 0;
        }
    }

    public void AddBoon(PowerupType type) {
        if(type == PowerupType.Defense) {
            PlayerHealth.instance.ShowDefense();
        }
        hasPowerup[type] = true;
        StartCoroutine(countdownOnBoon(type));
    }

    private IEnumerator countdownOnBoon(PowerupType type) {
        switch (type) {
            case PowerupType.Attack:
                timeLeft[type] = attackTimer;
                break;
            case PowerupType.Defense:
                timeLeft[type] = defenseTimer;
                break;
            case PowerupType.Time:
                timeLeft[type] = timeTimer;
                break;
            case PowerupType.Score:
                timeLeft[type] = scoreTimer;
                break;
            default:
                timeLeft[type] = 0f;
                break;
        }

        do { 
            timeLeft[type] -= Time.deltaTime;
            yield return null;
        } while(hasPowerup[type] && timeLeft[type] > 0);

        timeLeft[type] = 0f;
        hasPowerup[type] = false;
        if(type == PowerupType.Defense) {
            PlayerHealth.instance.HideDefense();
        }
    }
}
