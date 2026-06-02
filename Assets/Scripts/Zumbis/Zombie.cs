using UnityEngine;
using System;
using System.Collections;

public class Zombie : MonoBehaviour {
    public ZombieData attributes;

    protected float attackCooldownTimer;
    protected Animator animator;
    protected float timeUntilUpgrade, upgradeTime = 45f;
    protected float currentHealth;
    protected SliderBar healthBar;
    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite upgradedZombie;

    // Efeitos Sonoros
    [SerializeField] protected AudioSource upgradeSFX;
    [SerializeField] protected AudioSource deathSFX;
    [SerializeField] protected AudioSource attackSFX;
    [SerializeField] protected ParticleSystem evolutionEffect;
    public event Action<GameObject> OnTorreMorreu;

    // Modificações Visuais
    protected float fadeDuration = 0.5f;
    protected float damageFlashDuration = 0.1f;
    protected Color damageFlashColor = Color.red;
    protected SpriteRenderer spriteRenderer;
    protected PilhaDeCarne pilhaDeCarne;

    protected void Start() {
        animator = GetComponent<Animator>();
        
        upgradeSFX = GetComponent<AudioSource>();
        deathSFX = GetComponent<AudioSource>();

        healthBar = GetComponentInChildren<SliderBar>();
        currentHealth = attributes.maxHealth;
        healthBar.Set(attributes.maxHealth, currentHealth);
        timeUntilUpgrade = upgradeTime;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;

        GameObject pilhaDeCarneObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");

        if (pilhaDeCarneObject != null) {
            pilhaDeCarne = pilhaDeCarneObject.GetComponent<PilhaDeCarne>();
        }
    }

    protected virtual void Update() { }

    protected virtual void Attack(GameObject target) { }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthBar.UpdateSlider(currentHealth);

        StartCoroutine(DamageFlashEffect());

        if (currentHealth <= 0) {
            Die();
        }

    }

    protected void Die() {
        deathSFX.Play();
        StartCoroutine(SumirEDestruir());
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) {
            DestruirZumbi();
        }
    }

    protected void DestruirZumbi()
    {
        // Calcula a metade do custo
        int pontosRecuperados = Mathf.FloorToInt(attributes.zombieCost / 2.0f);

        // Recupera os pontos na Pilha de Carne
        if (pilhaDeCarne != null)
        {
            pilhaDeCarne.GerarPontos(pontosRecuperados);
        }
        else
        {
            Debug.LogWarning("PilhaDeCarne não foi atribuída. Pontos não foram recuperados.");
        }

        // Inicia o fade-out antes de destruir o objeto
        StartCoroutine(SumirEDestruir());
    }

    // Modificações Visuais

    protected IEnumerator SumirEDestruir() {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }

        // Destroi o objeto após o fade-out
        OnTorreMorreu?.Invoke(gameObject);
        Destroy(gameObject);
    }

    protected IEnumerator DamageFlashEffect() {
        if (spriteRenderer != null)
        {
            // Armazena a cor original do zumbi
            Color originalColor = spriteRenderer.color;

            // Muda a cor para o flash de dano
            spriteRenderer.color = damageFlashColor;

            // Espera o tempo do flash de dano
            yield return new WaitForSeconds(damageFlashDuration);

            // Restaura a cor original
            spriteRenderer.color = originalColor;
        }
    }

    protected void UpgradeStatus() {
        evolutionEffect.Play();
        upgradeSFX.Play();

        Debug.Log("Deu upgrade no " + attributes.zombieName);

        attributes.damage += 10f;
        attributes.maxHealth += 30f;
        currentHealth = attributes.maxHealth;
        
        healthBar.UpdateSlider(currentHealth);
        timeUntilUpgrade = upgradeTime;
    }

    public void ItemUpgrade() => spriteRenderer.sprite = upgradedZombie;
}