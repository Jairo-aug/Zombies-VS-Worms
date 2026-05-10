using System.Linq;
using UnityEngine;
using System;

public class Torre04 : MonoBehaviour
{
    public int custo = 150; // Custo da Torre 4
    public float attackRange = 2.5f; // Alcance de ataque da torre
    public float attackCooldown = 3f; // Tempo entre ataques
    public float dano = 50f; // Dano por ataque
    private float attackCooldownTimer;

    [SerializeField] float currentHealth, maxHealth = 120f; // Vida máxima da torre
    [SerializeField] SliderBar healthBar; // Referência à barra de vida

    [SerializeField] float timeUntilUpgrade, upgradeTime = 45f;
    [SerializeField] ParticleSystem evolutionEffect;
    [SerializeField] ParticleSystem bombEffect;

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
        somUpgrade = GetComponent<AudioSource>();
        somMorte = GetComponent<AudioSource>();
        
        healthBar = GetComponentInChildren<SliderBar>();
        currentHealth = maxHealth;
        healthBar.Set(maxHealth, currentHealth);
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

        if (attackCooldownTimer <= 0f)
        {
            AttackArea(); // Realiza o ataque em área
            attackCooldownTimer = attackCooldown;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    void AttackArea()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
        if (enemiesInRange.Count() > 1)
        {
            bombEffect.Play();
            somAtaque.Play();
        }

        foreach (Collider2D collider in enemiesInRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Verifica se o alvo possui os componentes válidos
                Minhoca minhoca = collider.GetComponent<Minhoca>();
                Larva larva = collider.GetComponent<Larva>();
                Verme verme = collider.GetComponent<Verme>();

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
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.UpdateSlider(currentHealth);

        // Modificações Visuais

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

    // Função para quando você clica com o botão direito no zumbi

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
        dano += 30f;
        maxHealth += 25f;
        currentHealth = maxHealth;
        healthBar.UpdateSlider(currentHealth);
        timeUntilUpgrade = upgradeTime;

    }
}
