using UnityEngine;
using System;

public class Vertebrawler : Zombie {
    public override string zombieName { get; protected set; } = "Vertebrawler";
    public override float zombieCost { get; protected set; } = 150f;
    public override float attackRange { get; protected set; } = 4.4f;
    public override float attackCooldown { get; protected set; } = 2f;
    public override float damage { get; protected set; } = 0f;
    public override float maxHealth { get; protected set; } = 50f;

<<<<<<< Updated upstream
    [SerializeField] float currentHealth, maxHealth = 50f;
    [SerializeField] SliderBar healthBar;

    public float projectileLifetime = 3f; // Tempo de vida do projétil em segundos

    [SerializeField] float timeUntilUpgrade, upgradeTime = 45f;
    [SerializeField] ParticleSystem evolutionEffect;

    private AudioSource somUpgrade;
    [SerializeField] AudioSource somMorte;

    private bool isUpgrade = false;

    public static event Action<GameObject> OnTorreMorreu;

    // Modificações Visuais

    [SerializeField] private float fadeDuration = 0.5f; // Duração do fade-out
    [SerializeField] private float damageFlashDuration = 0.1f; // Duração do flash de dano
    [SerializeField] private Color damageFlashColor = Color.red; // Cor do flash de dano
    private SpriteRenderer spriteRenderer;
    private PilhaDeCarne pilhaDeCarne;
    
    // Template
    [SerializeField] private Sprite upgradedZombie;


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
=======
    // Prefabs
    [SerializeField] private GameObject projectilePrefab;
    private float projectileLifetime = 3f;
    private bool isUpgrade;
    
    protected override void Update() {
>>>>>>> Stashed changes
        attackCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);
        GameObject closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D collider in enemiesInRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        if (closestEnemy != null && attackCooldownTimer <= 0f)
        {
            Attack(closestEnemy);
            attackCooldownTimer = attackCooldown;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    protected override void Attack(GameObject target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        // Obtenha o script do projétil após a instância
        Projetil projScript = projectile.GetComponent<Projetil>();
        
        if (projScript != null)
        {
            projScript.SetTarget(target);
        }
        else
        {
            Debug.LogError("Script 'Projetil' não encontrado no prefab do projétil.");
        }

        if(isUpgrade == true)
        {
            projScript.UpgradeStatus();       
        }

        // Ignorar a colisão entre o projétil e o inimigo para evitar interação física
        Collider2D enemyCollider = target.GetComponent<Collider2D>();
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        
        if (enemyCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }

        Destroy(projectile, projectileLifetime);
    }
<<<<<<< Updated upstream


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
        maxHealth += 20f;
        currentHealth = maxHealth;
        healthBar.UpdateSlider(currentHealth);
        timeUntilUpgrade = upgradeTime;
        isUpgrade = true;
    }
    
    public void ItemUpgrade() {
        Debug.Log("Item Upgrade no " + this.GetType().Name + "!");
        spriteRenderer.sprite = upgradedZombie;
    }
}
=======
}
>>>>>>> Stashed changes
