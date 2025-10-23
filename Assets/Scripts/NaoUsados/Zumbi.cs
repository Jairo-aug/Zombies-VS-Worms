using UnityEngine;

public class Zumbi : MonoBehaviour
{
    [SerializeField] private int custoZumbi = 100; // Custo do zumbi
    [SerializeField] private float fadeDuration = 0.5f; // Duração do fade-out
    [SerializeField] private float damageFlashDuration = 0.1f; // Duração do flash de dano
    [SerializeField] private Color damageFlashColor = Color.red; // Cor do flash de dano
    private PilhaDeCarne pilhaDeCarne; // Referência à Pilha de Carne
    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer do zumbi

    void Start()
    {
        // Obtém a referência ao objeto PilhaDeCarne usando a tag
        GameObject pilhaDeCarneObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");
        if (pilhaDeCarneObject != null)
        {
            pilhaDeCarne = pilhaDeCarneObject.GetComponent<PilhaDeCarne>();
        }
        else
        {
            Debug.LogWarning("PilhaDeCarne não encontrada. Certifique-se de que o objeto tem a tag 'PilhaDeCarne'.");
        }

        // Busca o SpriteRenderer nos filhos do zumbi (não apenas no objeto principal)
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer não encontrado nos filhos do objeto Zumbi.");
        }

        // Teste imediato de flash de cor ao iniciar
        StartCoroutine(DamageFlashEffect());
    }

    void OnMouseOver()
    {
        // Verifica se o botão direito do mouse foi pressionado enquanto o cursor está sobre o zumbi
        if (Input.GetMouseButtonDown(1)) // Botão direito do mouse
        {
            DestruirZumbi();
        }
    }

    void DestruirZumbi()
    {
        // Calcula a metade do custo
        int pontosRecuperados = Mathf.FloorToInt(custoZumbi / 2.0f);

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
        StartCoroutine(FadeOutAndDestroy());
    }

    // Função para causar dano ao zumbi
    public void TakeDamage(float damageAmount)
    {
        // Inicia o efeito visual de dano (flash de cor)
        StartCoroutine(DamageFlashEffect());

        // Lógica adicional para lidar com o dano do zumbi (por exemplo, reduzir vida, etc.)
    }

    // Efeito de cor (flash) quando o zumbi leva dano
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
        else
        {
            Debug.LogWarning("SpriteRenderer não encontrado, não é possível mostrar o efeito de dano.");
        }
    }

    System.Collections.IEnumerator FadeOutAndDestroy()
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
        Destroy(gameObject);
    }
}
