using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Keep this pretty low, around 0.1 seconds.")]
    private float timeToMove = 0.1f;
    [SerializeField]
    private int health = 1;
    [SerializeField]
    private List<KeyboardKey> movementPattern = new List<KeyboardKey>();
    [SerializeField]
    private KeyboardKey currentKey;
    [SerializeField]
    private KeyboardKey nextKey;

    private Animator anim;
    private bool moving = false;
    /*
    // Start is called before the first frame update
    void Start()
    {
        currentKey = movementPattern[0];

        if (nextKey != null)
        {
            nextKey = movementPattern[1];
        }

        anim = GetComponent<Animator>();
        transform.position = currentKey.transform.position;
    }

    public void UpdatePosition()
    {
        moving = true;

        //if(currentKey == movementPattern[movementPattern.Count])
        //{
        //    nextKey = movementPattern[(movementPattern.IndexOf(currentKey) + 1) % ];
        //}

        if (nextKey != null)
        {
            transform.DOMove(nextKey.transform.position, timeToMove).SetEase(Ease.Linear).OnComplete(() => moving = false);

            currentKey = nextKey;

            nextKey = movementPattern[(movementPattern.IndexOf(currentKey) + 1) % movementPattern.Count];
        }

    }
    */

    public KeyboardKey GetCurrentKey()
    {
        return currentKey;
    }

    public int GetHealth()
    {
        return health;
    }

    public void DecreaseHealth()
    {
        health--;
    }
}
