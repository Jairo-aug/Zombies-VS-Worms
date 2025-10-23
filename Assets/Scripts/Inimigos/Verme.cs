using UnityEngine;
using System.Collections;
using System;

public class Verme : MonoBehaviour
{
    protected Transform target; // Alterado para protected
    [SerializeField] protected float maxSpeed = 3.6f; // Alterado para protected
    protected float currentSpeed; // Alterado para protected
    public Rigidbody2D enemyRb;

    [SerializeField] protected float currentHealth, maxHealth = 20f; // Alterado para protected
    [SerializeField] protected EnemyHealthBar healthBar; // Alterado para protected

    [SerializeField] protected float danoAtaque = 10f; // Se precisar acessar, faça protected
    [SerializeField] protected float intervaloDano = 1.5f;

    // Campo 'expAmount' removido do SerializeField para evitar a serialização duplicada
    protected int expAmount = 20; // Agora é apenas protegido e não serializado.

    protected PilhaDeCarne pilhaDeCarne; // Alterado para protected
    private bool isTouchingPilhaDeCarne = false; // Pode permanecer private
    private Collider2D isTouchingTorre;

    private float tempoDesdeUltimoDano;

    // Modificações Visuais

    [SerializeField] private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer
    [SerializeField] private Color damageColor = Color.red; // Cor ao tomar dano
    [SerializeField] private float fadeDuration = 0.2f; // Duração do fade-out
    private Color originalColor; // Cor original do sprite

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyRb.gravityScale = 0;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    protected virtual void Start() // Alterado para protected
    {
        TargetIsPilhaDeCarne();
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Modificações Visuais

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Armazena a cor original
        }
    }

    void Update()
    {
        tempoDesdeUltimoDano += Time.deltaTime;

        if (target != null && !isTouchingPilhaDeCarne)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        if (isTouchingPilhaDeCarne && pilhaDeCarne != null)
        {
            if (tempoDesdeUltimoDano >= intervaloDano)
            {
                pilhaDeCarne.TakeDamage(danoAtaque);
                tempoDesdeUltimoDano = 0f;
            }
        }
        else if (isTouchingTorre != null)
        {
            if (tempoDesdeUltimoDano >= intervaloDano)
            {
                Attack(isTouchingTorre.gameObject);
                tempoDesdeUltimoDano = 0f; // Reseta o tempo
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("PilhaDeCarne"))
        {
            isTouchingPilhaDeCarne = true;
            currentSpeed = 0;
            enemyRb.isKinematic = true;
        }
        else if (collider.CompareTag("Player"))
        {
            isTouchingTorre = collider;
            currentSpeed = 0;
            enemyRb.isKinematic = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("PilhaDeCarne"))
        {
            isTouchingPilhaDeCarne = false;
            currentSpeed = maxSpeed;
        }
        else if (collider.CompareTag("Player"))
        {
            isTouchingTorre = null;
            TargetIsPilhaDeCarne();
            currentSpeed = maxSpeed;
            enemyRb.isKinematic = false;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage; // Reduz a vida atual

        // Feedback de dano: troca de cor por um instante
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            StartCoroutine(DamageEffect());
        }

        // Atualiza a barra de vida
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Modificações Visuais

        if (spriteRenderer != null)
        {
            StartCoroutine(DamageEffect());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

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

        Destroy(gameObject);
    }

    protected void Die()
    {
        // Gerar experiência e pontos
        ExperienceManager.Instance.AddExperience(expAmount);
        pilhaDeCarne.GerarPontos(expAmount);
        StartCoroutine(SumirEDestruir());
    }

    void TargetIsPilhaDeCarne()
    {
        GameObject pilhaDeCarneObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");
        if (pilhaDeCarneObject != null)
        {
            target = pilhaDeCarneObject.transform;
            pilhaDeCarne = pilhaDeCarneObject.GetComponent<PilhaDeCarne>();
        }
    }

    void Attack(GameObject target)
    {
        // Aplica dano diretamente ao inimigo mais próximo
        if (target.gameObject.TryGetComponent<Torre01>(out Torre01 torre01))
        {
            torre01.TakeDamage(danoAtaque);
        }
        else if (target.gameObject.TryGetComponent<Torre02>(out Torre02 torre02))
        {
            torre02.TakeDamage(danoAtaque);
        }
        else if (target.gameObject.TryGetComponent<Torre03>(out Torre03 torre03))
        {
            torre03.TakeDamage(danoAtaque);
        }
        else if (target.gameObject.TryGetComponent<PilhaDeCarne>(out PilhaDeCarne pilhaDeCarne))
        {
            pilhaDeCarne.TakeDamage(danoAtaque);
        }
    }

    public void levelUp()
    {
        maxHealth += 20f;
        danoAtaque += 2f;
    }
}

