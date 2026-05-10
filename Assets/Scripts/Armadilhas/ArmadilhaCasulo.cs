using System.Linq;
using UnityEngine;
using System;

public class ArmadilhaCasulo : MonoBehaviour
{
    [SerializeField] private float currentTime;
    [SerializeField] private float maxDuration = 1f;
    [SerializeField] private SliderBar timeBar; // Referência à barra de vida

    public static event Action<GameObject> OnTorreMorreu;

    // Modificações Visuais

    [SerializeField] private float fadeDuration = 0.5f; // Duração do fade-out
    public int custo = 50; // Custo da Armadilha Casulo
    [SerializeField] AudioSource somMorte;
    private SpriteRenderer spriteRenderer;
    private PilhaDeCarne pilhaDeCarne;

    void Start()
    {
        // Inicializa a barra de tempo e o tempo atual
        currentTime = maxDuration;

        // Referências
        GameObject pilhaDeCarneObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");
        pilhaDeCarne = pilhaDeCarneObject?.GetComponent<PilhaDeCarne>();

        timeBar = GetComponentInChildren<SliderBar>();
        if (timeBar != null)
        {
            timeBar.Set(maxDuration, currentTime);
        }

        // Inicializa o SpriteRenderer
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer não encontrado no objeto ou seus filhos.");
        }
    }

    void Update()
    {
        // Atualiza o tempo de vida da armadilha
        currentTime -= Time.deltaTime;
        if (timeBar != null)
            timeBar.UpdateSlider(currentTime);

        // Checa se o tempo acabou
        if (currentTime <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Gera 100 pontos na Pilha de Carne ao morrer
        if (pilhaDeCarne != null)
        {
            pilhaDeCarne.GerarPontos(100);
        }
        somMorte.Play();
        StartCoroutine(SumirEDestruir());
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Botão direito do mouse
        {
            DestruirArmadilha();
        }
    }

    void DestruirArmadilha()
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
        
        OnTorreMorreu?.Invoke(gameObject);
        Destroy(gameObject);
    }

    // DANO NA LARVA

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Larva>(out Larva larva))
        {
            larva.TakeDamage(40);
        }
    }
}
