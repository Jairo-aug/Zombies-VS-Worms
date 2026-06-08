using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

// Relativamente independente.
// Única dependência é a classe HealthBar;
// REFATORÁVEL.
public class PilhaDeCarne : MonoBehaviour, IHealable {
    public bool isHealthFull => health == maxHealth;

    public float pontosPodres = 0; // Pontos disponíveis, inicializando com 0
    public TextMeshProUGUI pontosPodresText; // Referência ao texto na UI para exibir os pontos
    public float pontosPorSegundo = 40f; // Quantidade de pontos gerados por segundo

    [SerializeField] float maxHealth = 80f;
    private float health;
    [SerializeField] SliderBar playerHealthBar;

    private float tempoUpdate;
    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer para a cor
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color healColor = Color.green; 
    private Color originalColor; // Cor original

    private void Start() {
        // Inicia a geração automática de pontos a cada segundo
        health = maxHealth;
        pontosPodres = 300;
        AtualizarUI(); // Atualiza a UI quando o jogo começa
        playerHealthBar.Set(maxHealth, health); // Atualiza a UI de vida desde o início
        
        spriteRenderer = GetComponent<SpriteRenderer>(); // Pegando a referência ao SpriteRenderer
        originalColor = spriteRenderer.color; // Armazena a cor original do sprite
    
        // Dá pra organizar o código melhor.
        // Unificar tudo em um método.
        FleshDrop.OnFleshClicked += (float fleshificationAmount) => {
            pontosPodres += fleshificationAmount;

            AtualizarUI();
        };
    }

    // Método para gerar pontos por segundo
    public void GerarPontos(int pontosGanhos)
    {
        pontosPodres += pontosGanhos;
        if (pontosPodres > 300)
            pontosPodres = 300;
        AtualizarUI(); // Atualiza a UI após gerar pontos
    }

    public void AtualizarUI() => pontosPodresText.text = pontosPodres.ToString();

    // Novo método para reduzir pontos
    // Refatorar.
    public void ReduzirPontos(int quantidade) {
        pontosPodres -= quantidade; 
        if (pontosPodres < 0) pontosPodres = 0;
        AtualizarUI();
    }

    public void ReceivePointsFromFleshification(float quantity) {
        pontosPodres += quantity;
        AtualizarUI();
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        playerHealthBar.UpdateSlider(health);
        
        // Adiciona o efeito visual de dano
        StartCoroutine(DamageEffect());

        if (health <= 0) Die();
    }

    public void GetHealed(float healAmount) {
        if (health + healAmount >= maxHealth) {
            health = maxHealth;
        }

        health += healAmount;
        playerHealthBar.UpdateSlider(health);

        StartCoroutine(HealEffect());
    }

    private IEnumerator DamageEffect() {
        // Muda a cor para o efeito de dano
        spriteRenderer.color = damageColor;

        // Espera 0.1 segundos e volta para a cor original
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = originalColor;
    }

    private IEnumerator HealEffect() {
        // Muda a cor para o efeito de dano
        spriteRenderer.color = healColor;

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
