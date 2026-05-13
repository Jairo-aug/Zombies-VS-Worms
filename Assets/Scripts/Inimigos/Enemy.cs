using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    protected Transform target; // O alvo a ser perseguido (a Pilha de Carne)
    protected float currentSpeed;
    public Rigidbody2D enemyRb;

    // ATRIBUTOS
    // Pode ser substituido por um scriptable object.
    protected virtual float maximumSpeed { get; set; } = 1f;
    protected virtual float maximumHealth { get; set; } = 85f;
    protected virtual float attackDamage { get; set; } = 10f;
    protected virtual float attackInterval { get; set; } = 2f;
    protected virtual int expAmount { get; set; } = 30;
    protected virtual float dropChance { get; set; } = 20f;

    protected float currentHealth;
    protected EnemyHealthBar healthBar;

    
    protected PilhaDeCarne pileOfFlesh; // Referência à Pilha de Carne
    protected bool isTouchingPileOfFlesh = false; // Verifica se está tocando a Pilha de Carne
    protected Collider2D isTouchingTower; // Verifica se está tocando a Pilha de Carne
    protected float timeSinceLastHit; // Tempo desde a última aplicação de dano

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
        
        TargetIspileOfFlesh();
    
        currentHealth = maximumHealth;
        
        if (healthBar != null) {
            healthBar.UpdateHealthBar(currentHealth, maximumHealth);
        }

        currentSpeed = maximumSpeed;

        // Modificações Visuais
        if (spriteRenderer != null) {
            originalColor = spriteRenderer.color; // Armazena a cor original
        }
    }

    protected virtual void Update() {
        timeSinceLastHit += Time.deltaTime; // Atualiza o tempo desde o último dano

        // Verifica se o alvo foi definido
        if (target != null) {
            // Calcula a direção para o alvo
            Vector2 direction = (target.position - transform.position).normalized;

            // Move o inimigo na direção do alvo
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        // Aplica dano contínuo enquanto estiver tocando a Pilha de Carne
        if (isTouchingPileOfFlesh && pileOfFlesh != null) {
            
            if (timeSinceLastHit >= attackInterval) {
                // Usar evento para dar dano à pilha.
                pileOfFlesh.TakeDamage(attackDamage); // Aplica dano à vida da Pilha de Carne

                timeSinceLastHit = 0f; // Reseta o tempo
            }
        }

        else if (isTouchingTower != null) {
            if (timeSinceLastHit >= attackInterval) {
                Attack(isTouchingTower.gameObject);
                timeSinceLastHit = 0f; // Reseta o tempo
            }
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPileOfFlesh = true; 
            currentSpeed = 0;    
            enemyRb.isKinematic = true;
        }

        else if (collider.CompareTag("Player")) {
            isTouchingTower = collider;
            currentSpeed = 0;    
            enemyRb.isKinematic = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPileOfFlesh = false; // Marca que não está mais tocando a Pilha de Carne
        }

        else if (collider.CompareTag("Player")) {
            isTouchingTower = null;
            TargetIspileOfFlesh();
            currentSpeed = maximumSpeed;    
            enemyRb.isKinematic = false;
        }
    }

    protected virtual void Attack(GameObject target) {
        // Aplica dano diretamente ao inimigo mais próximo

        if(target.gameObject.TryGetComponent<Torre01>(out Torre01 torre01)) {
            torre01.TakeDamage(attackDamage);
        }

        else if(target.gameObject.TryGetComponent<Torre02>(out Torre02 torre02)) {
            torre02.TakeDamage(attackDamage);
        }

        else if(target.gameObject.TryGetComponent<Torre03>(out Torre03 torre03)) {
            torre03.TakeDamage(attackDamage);
        }

        else if(target.gameObject.TryGetComponent<Torre04>(out Torre04 torre04)) {
            torre04.TakeDamage(attackDamage);
        }

        else if(target.gameObject.TryGetComponent<PilhaDeCarne>(out PilhaDeCarne pileOfFlesh)) {
            pileOfFlesh.TakeDamage(attackDamage);
        } 
    }

    public virtual void TakeDamage(float damage) {
        currentHealth -= damage; // Reduz a vida atual

        // Atualiza a barra de vida
        if (healthBar != null) {
            healthBar.UpdateHealthBar(currentHealth, maximumHealth);
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
        pileOfFlesh.GerarPontos(expAmount);
        StartCoroutine(SumirEDestruir());

        WillDropAnItem();
    }

    protected virtual void TargetIspileOfFlesh() {
        GameObject pileOfFleshObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");
        if (pileOfFleshObject != null) {
            target = pileOfFleshObject.transform;
            pileOfFlesh = pileOfFleshObject.GetComponent<PilhaDeCarne>(); // Obtém o script da Pilha de Carne
        }
    }

    public void levelUp() {
        maximumHealth += 25f;
        attackDamage += 4f;
    }

    protected bool WillDropAnItem() {
        System.Random r = new System.Random();
        int randomNumber = r.Next(0, 101);

        Debug.Log($"Inimigo com {dropChance}% de taxa rodou {randomNumber}. Vai dropar? {randomNumber < dropChance}.");

        return randomNumber < dropChance;
    }
}