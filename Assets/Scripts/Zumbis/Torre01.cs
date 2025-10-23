using UnityEngine;
using System;

public class Torre01 : MonoBehaviour
{
    public int custo = 100; // Custo da Torre 1
    public float attackRange = 0.5f; // Alcance de ataque da torre
    public float attackCooldown = 2f; // Tempo entre ataques
    public float dano = 20f; // Dano que a torre causa por ataque
    private float attackCooldownTimer;
    private Animator anim; // Referência ao componente Animator
    
    [SerializeField] float timeUntilUpgrade, upgradeTime = 45f;
    [SerializeField] ParticleSystem evolutionEffect;

    [SerializeField] float currentHealth, maxHealth = 100f; // Vida máxima da torre
    [SerializeField] HealthBar healthBar; // Referência à barra de vida

    private AudioSource somUpgrade;
    [SerializeField] AudioSource somMorte;
    [SerializeField] AudioSource somAtaque;

    public static event Action<GameObject> OnTorreMorreu;

    // Modificações Visuais

    [SerializeField] private float fadeDuration = 0.5f; // Duração do fade-out
    [SerializeField] private float damageFlashDuration = 0.1f; // Duração do flash de dano
    [SerializeField] private Color damageFlashColor = Color.red; // Cor do flash de dano
    private SpriteRenderer spriteRenderer;
    private PilhaDeCarne pilhaDeCarne;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        somUpgrade = GetComponent<AudioSource>();
        somMorte = GetComponent<AudioSource>();

        healthBar = GetComponentInChildren<HealthBar>();
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        timeUntilUpgrade = upgradeTime;

        // Modificações Visuais

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        GameObject pilhaDeCarneObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");

        if (pilhaDeCarneObject != null)
        {
            pilhaDeCarne = pilhaDeCarneObject.GetComponent<PilhaDeCarne>();
        }
    }

    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        // Verifica se o cooldown de ataque terminou
        if (attackCooldownTimer <= 0f)
        {
            // Encontra todos os inimigos dentro do alcance de ataque
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
            
            foreach (Collider2D collider in enemiesInRange)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Attack(collider.gameObject);
                }
            }

            // Reseta o cooldown de ataque
            attackCooldownTimer = attackCooldown;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    void Attack(GameObject target)
    {
        if (anim != null)
        {
            anim.SetTrigger("IsAttacking");
            Debug.Log("Animação de ataque acionada!");
        }

        // Verifica e aplica dano com base no tipo de inimigo
        Minhoca minhoca = target.GetComponent<Minhoca>();
        Larva larva = target.GetComponent<Larva>();
        Verme verme = target.GetComponent<Verme>();

        somAtaque.Play();

        if (minhoca != null)
        {
            minhoca.TakeDamage(dano);
        }
        else if (larva != null)
        {
            larva.TakeDamage(dano);
        }
        else if (verme != null)
        {
            verme.TakeDamage(dano);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        StartCoroutine(DamageFlashEffect());

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        somMorte.Play();
        StartCoroutine(SumirEDestruir());
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Botão direito do mouse
        {
            DestruirZumbi();
        }
    }

    void DestruirZumbi()
    {
        // Calcula a metade do custo
        int pontosRecuperados = Mathf.FloorToInt(custo / 2.0f);

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

    System.Collections.IEnumerator SumirEDestruir()
    {
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

    private System.Collections.IEnumerator DamageFlashEffect()
    {
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

    void UpgradeStatus()
    {
        evolutionEffect.Play();
        somUpgrade.Play();
        dano += 10f;
        maxHealth += 30f;
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        timeUntilUpgrade = upgradeTime;
    }
}
