using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardKey : MonoBehaviour
{
    public KeyCode myCode;

    public List<KeyboardKey> adjacentKeys = new List<KeyboardKey>();

    public bool isWall;
    public bool isHealing;

    //Needs to be public so player can access it
    [HideInInspector]
    public Vector3 originalPos;

    private Image keyCap;
    private Vector3 keyPos;

    private Powerup myPowerup;
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject healingParticles;

    private IEnemyCombat enemy;

    private Color partialWhite = new Color(1, 1, 1, 0.25f);
    private Color adjacent;

    [HideInInspector]
    public bool seen = false;

    bool playerIsHere = false;

    private void Awake() {
        ColorUtility.TryParseHtmlString("#F0A400", out adjacent);

        text = GetComponentInChildren<TextMeshProUGUI>();
        keyCap = GetComponent<Image>();
        myPowerup = GetComponentInChildren<Powerup>();
        enemy = GetComponentInChildren<IEnemyCombat>();
        //Get original key position
        keyPos = transform.localPosition;
        originalPos = transform.position;
    }

    private void Start() {
        if (HasPowerup()) {
            myPowerup.SetVisible(false);
        }
        SetKeyState(KeyState.Unseen);
    }

    public void PlayerIsHere(bool truthy) {
        playerIsHere = truthy;
        if (truthy) {
            SetKeyState(KeyState.Seen);

            if (isHealing)
            {
                HealPlayer();
            }

        }
    }

    public bool IsEmpty() {
        if(isWall || HasEnemy() || HasPowerup() || playerIsHere) {
            return false;
        }
        return true;
    }

    public bool HasEnemy() {
        if(GetComponentInChildren<IEnemyCombat>() == null) {
            return false;
        }

        enemy = GetComponentInChildren<IEnemyCombat>();
        enemy.AddKeyReference(this);

        return true;
    }

    public IEnemyCombat GetEnemy() {
        return enemy;
    }

    public bool HasPowerup() {
        if(myPowerup == null) {
            return false;
        }

        return true;
    }

    public void HealPlayer()
    {
        PlayerHealth.instance.RefillHealth();
        isHealing = false;
        if(healingParticles != null)
        {
            healingParticles.SetActive(false);
        }
    }

    public Powerup GetPowerup() {
        print("Do get!");
        Powerup result = myPowerup;
        Destroy(myPowerup.gameObject);
        text.enabled = true;
        return result;
    }

    public void AddIfNotExisting(KeyboardKey newNeighbor) {
        if (!adjacentKeys.Contains(newNeighbor)) {
            adjacentKeys.Add(newNeighbor);
        }
    }

    public void SetKeyState(KeyState state) {
        switch (state) {
            case KeyState.Unseen:
                keyCap.color = partialWhite;
                text.color = partialWhite;
                if (HasEnemy()) {
                    enemy.Hide();
                }
                break;
            case KeyState.Seen:
                if (isWall) {
                    keyCap.color = Color.blue;
                    text.color = Color.blue;
                }
                else {
                    keyCap.color = Color.white;
                    text.color = Color.white;
                }
                break;
            case KeyState.Adjacent:
                seen = true;
                if (isWall) {
                    keyCap.color = Color.blue;
                    text.color = Color.blue;
                }
                else {
                    keyCap.color = adjacent;
                    text.color = adjacent;
                    if (HasPowerup()) {
                        text.enabled = false;
                        myPowerup.SetVisible(true);
                    }
                }
                if (HasEnemy()) {
                    enemy.Show();
                }
                break;
            case KeyState.Rainbow:
                if (HasEnemy()) {
                    enemy.DieWithoutPoints();
                }
                StartCoroutine(Rainbow());
                break;
        }
    }

    private float rainbowDelay = 0.15f;

    private IEnumerator Rainbow() {
        while (true) {
            keyCap.color = Color.red;
            text.color = Color.red;
            yield return new WaitForSeconds(rainbowDelay);
            keyCap.color = new Color(1, 165f / 255f, 0);
            text.color = new Color(1, 165f / 255f, 0);
            yield return new WaitForSeconds(rainbowDelay);
            keyCap.color = Color.yellow;
            text.color = Color.yellow;
            yield return new WaitForSeconds(rainbowDelay);
            keyCap.color = Color.green;
            text.color = Color.green;
            yield return new WaitForSeconds(rainbowDelay);
            keyCap.color = Color.blue;
            text.color = Color.blue;
            yield return new WaitForSeconds(rainbowDelay);
            keyCap.color = new Color(75f / 255f, 0, 130f / 255f);
            text.color = new Color(75f / 255f, 0, 130f / 255f);
            yield return new WaitForSeconds(rainbowDelay);
            keyCap.color = new Color(128f / 255f, 0, 128f / 255f);
            text.color = new Color(128f / 255f, 0, 128f / 255f);
            yield return new WaitForSeconds(rainbowDelay);
        }
    }

    private float moveX = 3f;

    private IEnumerator KeyPressAnimation()
    {
        transform.localPosition = keyPos;
        originalPos = transform.position;
        //Set key position to be slightly different
        Vector3 newPos;
        if(seen && isWall) {
            newPos = transform.localPosition + new Vector3(moveX, 0, 0f);
            moveX *= -1;
        }
        else { 
            newPos = transform.localPosition + new Vector3(-3f,-3f,0f);
        }
        transform.localPosition = newPos;
        //Set up a timer
        float timer = 0.2f;
        while(timer > 0f)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
            transform.localPosition = Vector3.Lerp(keyPos,newPos,timer/0.2f);
        }
    }

    private void Update()
    {
        if (PlayerController.done) {
            return;
        }
        if (Input.GetKeyDown(myCode))
        {
            StartCoroutine(KeyPressAnimation());
        }
    }

    void OnDrawGizmosSelected() {
        foreach (KeyboardKey key in adjacentKeys) {
            if (key.adjacentKeys.Contains(this)) {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, key.transform.position);
            }
            else {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, key.transform.position);
            }
        }
    }
}