using UnityEngine;

public class ComportamentoMinhoca : MonoBehaviour
{
    private Transform target;
    [SerializeField] float maxSpeed = 2f;
    private float currentSpeed;
    public Rigidbody2D enemyRb;

    [SerializeField] float currentHealth, maxHealth = 50f;
    [SerializeField] EnemyHealthBar healthBar;

    [SerializeField] float danoAtaque = 12f;
    [SerializeField] float intervaloDano = 2f;

    int expAmount = 20;

    private FleshStack fleshStack;
    private bool isTouchingFleshStack = false;
    private Collider2D isTouchingTorre; // Verifica se está tocando a Pilha de Carne

    private float tempoDesdeUltimoDano;

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyRb.gravityScale = 0;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    void Start()
    {
        TargetIsFleshStack();
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void Update()
    {
        tempoDesdeUltimoDano += Time.deltaTime;

        if (target != null && !isTouchingFleshStack)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        if (isTouchingFleshStack && fleshStack != null)
        {
            if (tempoDesdeUltimoDano >= intervaloDano)
            {
                Attack(fleshStack.gameObject);
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
        if (collider.CompareTag("FleshStack"))
        {
            isTouchingFleshStack = true;
            currentSpeed = 0;
        }

        else if (collider.CompareTag("Player"))
        {
            isTouchingTorre = collider;
        }
        
        currentSpeed = 0;    
        enemyRb.isKinematic = true;
        TakeDamage(5);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("FleshStack"))
        {
            isTouchingFleshStack = false;
            currentSpeed = maxSpeed;
        }

        else if (collider.CompareTag("Player"))
        {
            isTouchingTorre = null;
            TargetIsFleshStack();
            currentSpeed = maxSpeed;    
            enemyRb.isKinematic = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ExperienceManager.Instance.AddExperience(10);
        fleshStack.ModifyPointQuantity(expAmount);
        Destroy(gameObject);
    }

    void TargetIsFleshStack()
    {
        GameObject fleshStackObject = GameObject.FindGameObjectWithTag("FleshStack");
        if (fleshStackObject != null)
        {
            target = fleshStackObject.transform;
            fleshStack = fleshStackObject.GetComponent<FleshStack>();
        }
    }

    void Attack(GameObject target)
    {
        Zombie z = target.GetComponent<Zombie>();
        z.TakeDamage(danoAtaque);
    }

    public void levelUp()
    {
        maxHealth += 20f;
        danoAtaque += 2f;
    }
}

