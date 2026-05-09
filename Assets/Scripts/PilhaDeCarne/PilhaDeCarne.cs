using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;

// Relativamente independente.
// Única dependência é a classe HealthBar;
// REFATORÁVEL.
public class PilhaDeCarne : MonoBehaviour
{
    public int pontosPodres = 0; // Pontos disponíveis, inicializando com 0
    public TextMeshProUGUI pontosPodresText; // Referência ao texto na UI para exibir os pontos
    public float pontosPorSegundo = 40f; // Quantidade de pontos gerados por segundo

    [SerializeField] float maxHealth = 80f;
    private float health;
    [SerializeField] HealthBar playerHealthBar;

    private float tempoUpdate;
    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer para a cor
    [SerializeField] private Color damageColor = Color.red; // Cor para o efeito de dano
    private Color originalColor; // Cor original

    private void Start()
    {
        // Inicia a geração automática de pontos a cada segundo
        health = maxHealth;
        pontosPodres = 300;
        AtualizarUI(); // Atualiza a UI quando o jogo começa
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.UpdateHealthBar(health, maxHealth); // Atualiza a UI de vida desde o início
        
        spriteRenderer = GetComponent<SpriteRenderer>(); // Pegando a referência ao SpriteRenderer
        originalColor = spriteRenderer.color; // Armazena a cor original do sprite
    }

    // Método para gerar pontos por segundo
    public void GerarPontos(int pontosGanhos)
    {
        pontosPodres += pontosGanhos;
        if (pontosPodres > 300)
            pontosPodres = 300;
        AtualizarUI(); // Atualiza a UI após gerar pontos
    }

    // Torna o método público para que ele possa ser acessado de outros scripts
    public void AtualizarUI()
    {
        pontosPodresText.text = "" + pontosPodres;
    }

    // Novo método para reduzir pontos
    public void ReduzirPontos(float quantidade)
    {
        pontosPodres -= Mathf.RoundToInt(quantidade); // Reduz a quantidade, arredondando para inteiro
        if (pontosPodres < 0) pontosPodres = 0; // Garante que os pontos não fiquem negativos
        AtualizarUI(); // Atualiza a UI após reduzir pontos
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        playerHealthBar.UpdateHealthBar(health, maxHealth);
        
        // Adiciona o efeito visual de dano
        StartCoroutine(DamageEffect());

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageEffect()
    {
        // Muda a cor para o efeito de dano
        spriteRenderer.color = damageColor;

        // Espera 0.1 segundos e volta para a cor original
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("Derrota");
    }
}
