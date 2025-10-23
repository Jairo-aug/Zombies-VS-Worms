using UnityEngine;

public class ComportamentoVerme : MonoBehaviour
{
    private Transform target;
    [SerializeField] float maxSpeed = 3.6f;
    private float currentSpeed;
    public Rigidbody2D enemyRb;

    [SerializeField] float currentHealth, maxHealth = 20f;
    [SerializeField] EnemyHealthBar healthBar;

    [SerializeField] float danoAtaque = 8f;
    [SerializeField] float intervaloDano = 1.5f;
    
    int expAmount = 25;

    private PilhaDeCarne pilhaDeCarne;
    private bool isTouchingPilhaDeCarne = false;
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
        TargetIsPilhaDeCarne();
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
        pilhaDeCarne.GerarPontos(expAmount);
        Destroy(gameObject);
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

        if(target.gameObject.TryGetComponent<Torre01>(out Torre01 torre01))
        {
            torre01.TakeDamage(danoAtaque);
        }

        else if(target.gameObject.TryGetComponent<Torre02>(out Torre02 torre02))
        {
            torre02.TakeDamage(danoAtaque);
        }

        else if(target.gameObject.TryGetComponent<Torre03>(out Torre03 torre03))
        {
            torre03.TakeDamage(danoAtaque);
        }

        else if(target.gameObject.TryGetComponent<PilhaDeCarne>(out PilhaDeCarne pilhaDeCarne))
        {
            pilhaDeCarne.TakeDamage(danoAtaque);
        }
                
        Debug.Log("Verme atacou o inimigo diretamente." + target.name);
    }

    public void levelUp()
    {
        maxHealth += 20f;
        danoAtaque += 2f;
    }
}
