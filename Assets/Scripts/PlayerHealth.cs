using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //References
    public Image healthBar;
    public ParticleSystem deathParticles;
    public GameObject shieldBar;

    //Actual Stats
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int healthPoints;

    //References
    private Image playerSprite;

    public static PlayerHealth instance;

    private void Awake()
    {
        instance = this;
        playerSprite = GetComponent<Image>();
    }
    private void Start()
    {
        healthPoints = maxHealth;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        healthBar.fillAmount = (float)healthPoints / (float)maxHealth;
    }

    public void ShowDefense() {
        shieldBar.SetActive(true);
    }

    public void HideDefense() {
        shieldBar.SetActive(false);
    }

    public void ApplyDamage(int amount)
    {
        if (shieldBar.activeSelf) {
            amount = 0;
            HideDefense();
        }
        if (amount != 0) {
            healthPoints -= amount;
            Screenshake.instance.Shake((float)amount * 20f);
            StartCoroutine(DamageFlash());
            UpdateHealth();
        }

        GetComponent<PlayerController>().waitingForEnemyAttack = false;

        if (Dead())
        {
            Death();
        }
    }

    public void RefillHealth()
    {
        healthPoints = maxHealth;
        UpdateHealth();
    }

    private IEnumerator DamageFlash()
    {
        float timer = 1f;
        while(timer > 0f)
        {
            if(Mathf.FloorToInt(timer*10) % 2 == 0)
            {
                playerSprite.color = new Color(0f,0f,0f,0f);
            }
            else
            {
                playerSprite.color = Color.red;
            }
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        playerSprite.color = Color.white;
    }

    private bool Dead()
    {
        return (healthPoints <= 0);
    }

    private void Death()
    {
        //The hero has fallen
        //Shake the screen, turn off the sprite, and pop death particles
        Screenshake.instance.Shake(100f);
        playerSprite.enabled = false;
        deathParticles.Play();
        Destroy(gameObject, 3f);
        //Put a reference to the game manager here once it is extant
        GameManager.instance.GameOver();
    }
}
