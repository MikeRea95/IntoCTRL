using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private AudioClip mainMenuAudio;
    [SerializeField]
    private List<AudioClip> gameAudio = new List<AudioClip>();

    private AudioSource aud;
    private int index;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        index = Random.Range(0, 2);
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGameAudio()
    {
        aud.clip = gameAudio[index];
        aud.time = 0f;
        aud.Play();
    }

    private void Update()
    {
        if(aud.time >= aud.clip.length)
        {
            //song is over. change it
            switch (index)
            {
                case (0):
                    index = 1;
                    break;
                case (1):
                    index = 2;
                    break;
            }
            StartGameAudio();
        }
    }
}
