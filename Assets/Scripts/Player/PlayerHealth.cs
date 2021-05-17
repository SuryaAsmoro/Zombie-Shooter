using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    public GameObject textHolder;


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;

    bool isDead;
    bool damaged;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = startingHealth;
    }

    private void Update()
    {
        if (damaged)
            damageImage.color = flashColor;
        else
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);

        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
            Death();
    }

    void Death()
    {
        isDead = true;

        playerShooting.DisableEffects();

        anim.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void HealDamage(int amount)
    {
        currentHealth += amount;

        GameObject healText = Instantiate(textHolder, this.gameObject.transform.GetChild(3).transform);
        healText.GetComponentInChildren<TMPro.TextMeshPro>().SetText("+" + amount);

        if (currentHealth > startingHealth)
            currentHealth = startingHealth;

        healthSlider.value = currentHealth;
    }

    #region Power Heal
    [Header("Power Heal")]
    public int healPower = 30;
    public float healDuration = 3.0f;

    public bool IsInHealBuff;

    public void BeginHealPlayer()
    {
        StartCoroutine(HealPlayer());
    }

    IEnumerator HealPlayer()
    {
        IsInHealBuff = true;
        Debug.Log("Healing");
        float duration = healDuration;

        while (duration > 0)
        {
            HealDamage(healPower / (int)healDuration);

            duration -= 1f;
            yield return new WaitForSeconds(1);
        }

        IsInHealBuff = false;
    }

    #endregion
}
