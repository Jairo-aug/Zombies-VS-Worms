using UnityEngine;
using System;

public class Torre02 : MonoBehaviour
{
    public int custo = 200; // Custo da Torre 2
    [SerializeField] float currentHealth, maxHealth = 300f; // Vida máxima da torre (muito alta)
    [SerializeField] SliderBar healthBar; // Referência à barra de vida

    [SerializeField] float timeUntilUpgrade, upgradeTime = 45f;
    [SerializeField] ParticleSystem evolutionEffect;
    private AudioSource somUpgrade;
    [SerializeField] AudioSource somMorte;

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
        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    // Método chamado quando a torre recebe dano
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
        maxHealth += 50f;
        currentHealth = maxHealth;
        healthBar.UpdateSlider(currentHealth);
        timeUntilUpgrade = upgradeTime;
    }
}
