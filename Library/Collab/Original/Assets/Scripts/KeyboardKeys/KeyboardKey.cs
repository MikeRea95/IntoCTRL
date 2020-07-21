using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardKey : MonoBehaviour
{
    public KeyCode myCode;

    public List<KeyboardKey> adjacentKeys = new List<KeyboardKey>();

    public bool isWall;

    private Image keyCap;
    private Event keyDown;
    private Powerup myPowerup;

    private void Awake() {
        keyCap = GetComponent<Image>();
        myPowerup = GetComponentInChildren<Powerup>();
        if (isWall) {
            SetKeyState(KeyState.Wall);
        }
        else {
            SetKeyState(KeyState.Default);
        }
    }

    public bool HasPowerup() {
        if(myPowerup != null) {
            return true;
        }

        return false;
    }

    public Powerup GetPowerup() {
        if (myPowerup == null) {
            return null;
        }
        else {
            Powerup result = myPowerup;
            Destroy(myPowerup.gameObject);
            myPowerup = null;
            return result;
        }
    }

    public void AddIfNotExisting(KeyboardKey newNeighbor) {
        if (!adjacentKeys.Contains(newNeighbor)) {
            adjacentKeys.Add(newNeighbor);
        }
    }

    public void SetKeyState(KeyState state) {
        Color colorFromHex;
        switch (state) {
            case KeyState.Default:
                keyCap.color = new Color(1,1,1, 0.5f);
                break;
            case KeyState.Adjacent:
                ColorUtility.TryParseHtmlString("#F0A400", out colorFromHex);
                keyCap.color = colorFromHex;
                break;
            case KeyState.Wall:
                keyCap.color = Color.blue;
                break;
        }
    }

    private IEnumerator KeyPressAnimation()
    {
        //Get original key position
        Vector3 keyPos = transform.localPosition;
        //Set key position to be slightly different
        Vector3 newPos = transform.localPosition + new Vector3(-3f,-3f,0f);
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