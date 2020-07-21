using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private KeyboardKey currentKey;
    [SerializeField]
    [Tooltip("Keep this pretty low, around 0.1 seconds.")]
    private float timeToMove = .1f;

    public VictoryController victoryController;
    private bool moving = false;
    private KeyboardKey bufferedKey = null;
    private AudioSource audioSource; //For playing footstep sounds
    private Animator anim; //For displaying idle and walk animations
    private PlayerBoons boons;

    public static bool done = false;

    public bool waitingForEnemyAttack;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        boons = GetComponent<PlayerBoons>();
        transform.position = currentKey.transform.position;
        currentKey.PlayerIsHere(true);
        ShowAdjacentSpaces();
    }

    private void Update() {
        if (waitingForEnemyAttack || done) {
            return;
        }
        if(bufferedKey != null) {
            BeginMovement(bufferedKey);
        }
        else if (!moving) {
            foreach (KeyboardKey key in currentKey.adjacentKeys) {
                if (!key.isWall && Input.GetKeyDown(key.myCode)) {
                    BeginMovement(key);
                    break;
                }
            }
        }
    }

    private void BeginMovement(KeyboardKey nextKey) {
        if (nextKey.transform.position.x < transform.position.x) //Player is to the right of the key. Face left.
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (nextKey.transform.position.x > transform.position.x) //Player is to the left of the key. Face right.
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (nextKey.myCode == KeyCode.LeftControl) {
            done = true;
            StartCoroutine(victoryController.LevelComplete());
        }
        audioSource.Play();
        HideAdjacentSpaces();
        if (nextKey.HasPowerup()) {
            boons.AddBoon(nextKey.GetPowerup().type);
        }
        else if (nextKey.HasEnemy()) {
            waitingForEnemyAttack = true;
            StartCoroutine(PlayAttackAnimation());
            if (!nextKey.GetEnemy().PlayerAttacks()) {
                ShowAdjacentSpaces();
                return;
            }
            else {
                HideAdjacentSpaces();
            }
        }
        moving = true;
        currentKey.PlayerIsHere(false);
        currentKey = nextKey;
        currentKey.PlayerIsHere(true);

        UpdateAnimation(); //set the animation to moving
        //transform.DOMove(currentKey.transform.position, timeToMove).SetEase(Ease.Linear).OnComplete(() => moving = false);
        transform.DOMove(currentKey.GetComponent<KeyboardKey>().originalPos, timeToMove).SetEase(Ease.Linear).OnComplete(() => moving = false);
        StartCoroutine(AcceptBufferedInput());
    }

    private IEnumerator AcceptBufferedInput() {
        bufferedKey = null;
        yield return new WaitForSeconds(timeToMove * 0.75f);
        ShowAdjacentSpaces();
        float countdown = timeToMove * 0.25f;

        // Key to hold on to, but don't let the player go there yet.
        KeyboardKey keyToBuffer = null;
        while (countdown > 0) {
            if(keyToBuffer == null){
                foreach (KeyboardKey key in currentKey.adjacentKeys) {
                    if (Input.GetKeyDown(key.myCode)) {
                        keyToBuffer = key;
                    }
                }
            }
            countdown -= Time.deltaTime;
            yield return null;
        }

        // Let the player go there now.
        bufferedKey = keyToBuffer;
        // Stop movement if there is no buffered key
        if(bufferedKey == null)
        {
            UpdateAnimation();
        }
    }

    private void HideAdjacentSpaces() {
        foreach (KeyboardKey key in currentKey.adjacentKeys) {
            key.SetKeyState(KeyState.Seen);
        }
    }

    private void ShowAdjacentSpaces() {
        if (done) {
            return;
        }
        print(currentKey.adjacentKeys.Count);
        foreach (KeyboardKey key in currentKey.adjacentKeys) {
            key.SetKeyState(KeyState.Adjacent);
        }
    }

    private IEnumerator PlayAttackAnimation()
    {
        anim.SetBool("Punch", true);
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Punch", false);
    }

    private void UpdateAnimation()
    {
        anim.SetBool("Moving", moving);
        if (!moving)
        {
            return;
        }
    }
}