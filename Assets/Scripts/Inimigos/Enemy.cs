using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable {
    public Transform target; // O alvo a ser perseguido (a Pilha de Carne)
    protected float currentSpeed;
    public Rigidbody2D enemyRb;
    protected Vector2 direction;
    protected BoxCollider2D boxCollider;
    protected EnemyState currentState;

    // ATRIBUTOS
    // Pode ser substituido por um scriptable object.
    protected virtual float maximumSpeed { get; set; }
    protected virtual float maximumHealth { get; set; }
    protected virtual float attackDamage { get; set; }
    protected virtual float attackInterval { get; set; }
    protected virtual int expAmount { get; set; }
    protected virtual float dropChance { get; set; }

    protected float currentHealth;
    protected EnemyHealthBar healthBar;

    [SerializeField] protected Sprite placeholderItemDrop;

    
    protected FleshStack fleshStack; // Referência à Pilha de Carne
    protected bool isTouchingfleshStack = false; // Verifica se está tocando a Pilha de Carne
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
        
        TargetIsFleshStack();
    
        currentHealth = maximumHealth;
        
        if (healthBar != null) {
            healthBar.UpdateHealthBar(currentHealth, maximumHealth);
        }

        currentSpeed = maximumSpeed;

        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Modificações Visuais
        if (spriteRenderer != null) {
            originalColor = spriteRenderer.color; // Armazena a cor original
        }

        SetState(EnemyState.Aggroing);
    }

    protected virtual void Update() {
        timeSinceLastHit += Time.deltaTime; // Atualiza o tempo desde o último dano

        // Aplica dano contínuo enquanto estiver tocando a Pilha de Carne
        if (isTouchingfleshStack && fleshStack != null) {
            
            if (timeSinceLastHit >= attackInterval) {
                // Usar evento para dar dano à pilha.
                fleshStack.TakeDamage(attackDamage); // Aplica dano à vida da Pilha de Carne

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

    protected virtual void FixedUpdate() {
        direction = (target.position - transform.position).normalized;
        
        if (currentState == EnemyState.Hypnotized) {
            enemyRb.linearVelocity = -(direction * currentSpeed);
        }
        
        else if (currentState == EnemyState.Aggroing) {
            if (isTouchingTower || isTouchingfleshStack) {
                currentSpeed = 0;
            } else {
                currentSpeed = maximumSpeed;
            }

            enemyRb.linearVelocity = direction * currentSpeed;
        }
    }

    protected void SetState(EnemyState state) => currentState = state;

    protected virtual void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingfleshStack = false; // Marca que não está mais tocando a Pilha de Carne
        }

        else if (collider.CompareTag("Player")) {
            isTouchingTower = null;
            TargetIsFleshStack();
            currentSpeed = maximumSpeed;    
            enemyRb.isKinematic = false;
        }

        else if (collider.gameObject.transform.parent) {
            if (collider.gameObject.transform.parent.TryGetComponent(out MagnetWizard magnetWizard)) {
                target = magnetWizard.gameObject.transform;
            }
        }
    }

    public IEnumerator GetHypnotized(float hypnotizationTime, int hits, float damage) {
        SetState(EnemyState.Hypnotized);
        spriteRenderer.flipX = true;
        currentSpeed /= 2;
        boxCollider.isTrigger = true;

        float timePerHit = hypnotizationTime / hits;

        for (int i = 0; i < hits; i++) {
            yield return new WaitForSeconds(timePerHit);
            TakeDamage(damage);
        }

        yield return new WaitForSeconds(hypnotizationTime);
        
        SetState(EnemyState.Aggroing);
        spriteRenderer.flipX = false;
        currentSpeed = maximumSpeed;
        boxCollider.isTrigger = false;
    }

    public IEnumerator GetRepelled(Vector2 repelDirection, float repelStrength, float paralizationLength) {
        SetState(EnemyState.Knockedback);
        GetComponent<Animator>().enabled = false;
        boxCollider.isTrigger = true;

        enemyRb.linearVelocity = Vector2.zero;
        enemyRb.AddForce(repelDirection * repelStrength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(paralizationLength);
        
        SetState(EnemyState.Aggroing);
        GetComponent<Animator>().enabled = true;
        boxCollider.isTrigger = false;
    }

    protected virtual void Attack(GameObject target) {
        Zombie z = target.GetComponent<Zombie>();
        z.TakeDamage(attackDamage);
    }

    public void TakeDamage(float damage) {
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
        fleshStack.ModifyPointQuantity(expAmount);

        StartCoroutine(SumirEDestruir());

        if (RNG.RollChance100(dropChance)) {
            DropItem();
        }
    }

    private void DropItem() {
        GameObject itemDrop = new GameObject("Crazy Apple");
            
        Apple apple = itemDrop.AddComponent<Apple>();
        apple.InstantiateItem(placeholderItemDrop, transform.position);
    }

    protected virtual void TargetIsFleshStack() {
        GameObject fleshStackObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");
        
        
        target = fleshStackObject.transform;
        fleshStack = fleshStackObject.GetComponent<FleshStack>();
    }

    public void levelUp() {
        maximumHealth += 25f;
        attackDamage += 4f;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        // Pode ser verificado existência de componente Zombie ao invés.
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingfleshStack = true; 
            currentSpeed = 0;    
            enemyRb.isKinematic = true;
        }

        // Pode ser verificado existência de componente Zombie ao invés.
        else if (collider.CompareTag("Player")) {
            isTouchingTower = collider;
            currentSpeed = 0;    
            enemyRb.isKinematic = true;
        }

        else if (collider.gameObject.transform.parent) {
            if (collider.gameObject.transform.parent.TryGetComponent(out MagnetWizard magnetWizard)) {
                target = magnetWizard.gameObject.transform;
            }
        }
    }

    protected enum EnemyState {
        Aggroing,
        Hypnotized,
        Knockedback
    }
}