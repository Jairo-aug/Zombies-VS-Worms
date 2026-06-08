using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

// Relativamente independente.
// Única dependência é a classe HealthBar;
// REFATORÁVEL.
public class FleshStack : MonoBehaviour, IHealable {
    public bool isHealthFull => health == maxHealth;

    public float rottenPoints = 0; // Pontos disponíveis, inicializando com 0
    public TextMeshProUGUI rottenPointsText; // Referência ao texto na UI para exibir os pontos
    public float pointsPerSecond = 40f; // Quantidade de pontos gerados por segundo

    [SerializeField] float maxHealth = 80f;
    private float health;
    [SerializeField] SliderBar playerHealthBar;

    private float updateTimer;
    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer para a cor
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private Color healColor = Color.green; 
    private Color originalColor; // Cor original

    private void Start() {
        // Inicia a geração automática de pontos a cada segundo
        health = maxHealth;
        rottenPoints = 150;
        
        UpdateDisplay(); // Atualiza a UI quando o jogo começa
        playerHealthBar.Set(maxHealth, health); // Atualiza a UI de vida desde o início
        
        spriteRenderer = GetComponent<SpriteRenderer>(); // Pegando a referência ao SpriteRenderer
        originalColor = spriteRenderer.color; // Armazena a cor original do sprite
    
        FleshDrop.OnFleshClicked += (int fleshificationAmount) => {
            ModifyPointQuantity(fleshificationAmount);
        };
    }

    public void ModifyPointQuantity(int quantity) {
        rottenPoints += quantity;

        UpdateDisplay();
    }

    public void UpdateDisplay() => rottenPointsText.text = rottenPoints.ToString();

    public void TakeDamage(float damageAmount) {
        health -= damageAmount;
        playerHealthBar.UpdateSlider(health);
        
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

    void Die() {
        Destroy(gameObject);
        SceneManager.LoadScene("Derrota");
    }
}
