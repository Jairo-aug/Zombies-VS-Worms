using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {
    protected Transform target; // O alvo a ser perseguido (a Pilha de Carne)
    protected float currentSpeed;
    public Rigidbody2D enemyRb;

    // ATRIBUTOS
    // Pode ser substituido por um scriptable object.
    protected virtual float maxSpeed { get; set; } = 1f;
    protected virtual float maxHealth { get; set; } = 85f;
    protected virtual float danoAtaque { get; set; } = 10f;
    protected virtual float intervaloDano { get; set; } = 2f;
    protected virtual int expAmount { get; set; } = 30;


    protected float currentHealth;
    protected EnemyHealthBar healthBar;

    
    protected PilhaDeCarne pilhaDeCarne; // Referência à Pilha de Carne
    protected bool isTouchingPilhaDeCarne = false; // Verifica se está tocando a Pilha de Carne
    protected Collider2D isTouchingTorre; // Verifica se está tocando a Pilha de Carne
    protected float tempoDesdeUltimoDano; // Tempo desde a última aplicação de dano

    // Modificações Visuais
    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer
    private Color damageColor = Color.red; // Cor ao tomar dano
    private float fadeDuration = 0.2f; // Duração do fade-out
    private Color originalColor; // Cor original do sprite

    protected virtual void Awake() {
        // Usar GetComponent<> para pegar todos os componentes necessários.
        enemyRb = GetComponent<Rigidbody2D>();
        enemyRb.gravityScale = 0;
        
        // Verifica se a barra de saúde do inimigo está atribuída
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    protected virtual void Start() {
        // Encontra o objeto chamado "Pilha de Carne" na cena e define o alvo
        
        TargetIsPilhaDeCarne();
    
        currentHealth = maxHealth;
        
        if (healthBar != null) {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        currentSpeed = maxSpeed;

        // Modificações Visuais
        if (spriteRenderer != null) {
            originalColor = spriteRenderer.color; // Armazena a cor original
        }
    }

    protected virtual void Update() {
        tempoDesdeUltimoDano += Time.deltaTime; // Atualiza o tempo desde o último dano

        // Verifica se o alvo foi definido
        if (target != null) {
            // Calcula a direção para o alvo
            Vector2 direction = (target.position - transform.position).normalized;

            // Move o inimigo na direção do alvo
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        // Aplica dano contínuo enquanto estiver tocando a Pilha de Carne
        if (isTouchingPilhaDeCarne && pilhaDeCarne != null) {
            
            if (tempoDesdeUltimoDano >= intervaloDano) {
                pilhaDeCarne.TakeDamage(danoAtaque); // Aplica dano à vida da Pilha de Carne
                tempoDesdeUltimoDano = 0f; // Reseta o tempo
            }
        }

        else if (isTouchingTorre != null) {
            if (tempoDesdeUltimoDano >= intervaloDano) {
                Attack(isTouchingTorre.gameObject);
                tempoDesdeUltimoDano = 0f; // Reseta o tempo
            }
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPilhaDeCarne = true; 
            currentSpeed = 0;    
            enemyRb.isKinematic = true;
        }

        else if (collider.CompareTag("Player")) {
            isTouchingTorre = collider;
            currentSpeed = 0;    
            enemyRb.isKinematic = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPilhaDeCarne = false; // Marca que não está mais tocando a Pilha de Carne
        }

        else if (collider.CompareTag("Player")) {
            isTouchingTorre = null;
            TargetIsPilhaDeCarne();
            currentSpeed = maxSpeed;    
            enemyRb.isKinematic = false;
        }
    }

    protected virtual void Attack(GameObject target) {
        // Aplica dano diretamente ao inimigo mais próximo

        if(target.gameObject.TryGetComponent<Torre01>(out Torre01 torre01)) {
            torre01.TakeDamage(danoAtaque);
        }

        else if(target.gameObject.TryGetComponent<Torre02>(out Torre02 torre02)) {
            torre02.TakeDamage(danoAtaque);
        }

        else if(target.gameObject.TryGetComponent<Torre03>(out Torre03 torre03)) {
            torre03.TakeDamage(danoAtaque);
        }

        else if(target.gameObject.TryGetComponent<Torre04>(out Torre04 torre04)) {
            torre04.TakeDamage(danoAtaque);
        }

        else if(target.gameObject.TryGetComponent<PilhaDeCarne>(out PilhaDeCarne pilhaDeCarne)) {
            pilhaDeCarne.TakeDamage(danoAtaque);
        } 
    }

    public virtual void TakeDamage(float damage) {
        currentHealth -= damage; // Reduz a vida atual

        // Atualiza a barra de vida
        if (healthBar != null) {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Modificações Visuais

        if (spriteRenderer != null) {
            StartCoroutine(DamageEffect());
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    protected IEnumerator DamageEffect() {
        if (spriteRenderer != null) {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    protected IEnumerator SumirEDestruir() {
        if (spriteRenderer != null) {
            Color originalColor = spriteRenderer.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }

        Destroy(gameObject);
    }

    protected void Die() {
        // Gerar experiência e pontos
        ExperienceManager.Instance.AddExperience(expAmount);
        pilhaDeCarne.GerarPontos(expAmount);
        StartCoroutine(SumirEDestruir());
    }

    protected virtual void TargetIsPilhaDeCarne() {
        GameObject pilhaDeCarneObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");
        if (pilhaDeCarneObject != null) {
            target = pilhaDeCarneObject.transform;
            pilhaDeCarne = pilhaDeCarneObject.GetComponent<PilhaDeCarne>(); // Obtém o script da Pilha de Carne
        }
    }

    public void levelUp() {
        maxHealth += 25f;
        danoAtaque += 4f;
    }
}