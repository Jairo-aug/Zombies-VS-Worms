using UnityEngine;
using System;
using System.Collections;

public class Zombie : MonoBehaviour, IClickable, IDamageable, IHealable {
    public ZombieData attributes;

    protected float actionCooldownTimer;
    protected Animator animator;
    protected float timeUntilUpgrade, upgradeTime = 45f;
    protected float currentHealth;
    protected SliderBar healthBar;

    public bool isHealthFull => currentHealth == attributes.maxHealth;

    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite fleshificationDropSprite;
    [SerializeField] protected Sprite upgradedZombie; // Temporário

    // Efeitos Sonoros
    [SerializeField] protected AudioSource upgradeSFX;
    [SerializeField] protected AudioSource deathSFX;
    [SerializeField] protected AudioSource attackSFX;
    [SerializeField] protected ParticleSystem evolutionEffect;
    public event Action<GameObject> OnTorreMorreu;

    // Modificações Visuais
    protected float fadeDuration = 0.5f;
    protected float damageFlashDuration = 0.1f;
    protected Color damageFlashColor = Color.red;
    protected float healFlashDuration = 0.1f;
    protected Color healFlashColor = Color.green;
    protected SpriteRenderer spriteRenderer;
    protected FleshStack fleshStack;

    protected virtual void Start() {
        animator = GetComponent<Animator>();
        
        upgradeSFX = GetComponent<AudioSource>();
        deathSFX = GetComponent<AudioSource>();

        healthBar = GetComponentInChildren<SliderBar>();
        currentHealth = attributes.maxHealth;
        healthBar.Set(attributes.maxHealth, currentHealth);
        timeUntilUpgrade = upgradeTime;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;

        GameObject fleshStackObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");
        fleshStack = fleshStackObject.GetComponent<FleshStack>();
    }

    public virtual void OnClicked() { }
    protected virtual void Update() { }
    protected virtual void Attack() { }
    protected virtual void Attack(GameObject target) { }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthBar.UpdateSlider(currentHealth);

        StartCoroutine(DamageFlashEffect());

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void GetHealed(float healAmount) {
        if (currentHealth + healAmount >= attributes.maxHealth) {
            currentHealth = attributes.maxHealth;
        }

        currentHealth += healAmount;
        healthBar.UpdateSlider(currentHealth);

        StartCoroutine(HealFlashEffect());
    }

    protected void Fleshificate() => DropFleshFromFleshification();

    private void DropFleshFromFleshification() {
        GameObject fleshDrop = new GameObject("Piece of Flesh");
            
        FleshDrop fleshComponent = fleshDrop.AddComponent<FleshDrop>();
        fleshComponent.rottenPointsAmount = attributes.fleshificationAmount;
        fleshComponent.InstantiateDrop(fleshificationDropSprite, transform.position);
    }

    protected void Die() {
        deathSFX.Play();
        StartCoroutine(DisappearEffect());
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) {
            DestroyZombie();
        }
    }

    protected void DestroyZombie() {
        int pointsRecovered = Mathf.FloorToInt(attributes.zombieCost / 2);

        fleshStack.ModifyPointQuantity(pointsRecovered);
        
        StartCoroutine(DisappearEffect());
    }

    // Modificações Visuais
    protected IEnumerator DisappearEffect() {
        Color originalColor = spriteRenderer.color;
        
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration){
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        
        OnTorreMorreu?.Invoke(gameObject);
        Destroy(gameObject);
    }

    protected IEnumerator DamageFlashEffect() {
        // Armazena a cor original do zumbi
        Color originalColor = spriteRenderer.color;

        // Muda a cor para o flash de dano
        spriteRenderer.color = damageFlashColor;

        // Espera o tempo do flash de dano
        yield return new WaitForSeconds(damageFlashDuration);

        // Restaura a cor original
        spriteRenderer.color = originalColor;
    }

    protected IEnumerator HealFlashEffect() {
        // Armazena a cor original do zumbi
        Color originalColor = spriteRenderer.color;

        // Muda a cor para o flash de dano
        spriteRenderer.color = healFlashColor;

        // Espera o tempo do flash de dano
        yield return new WaitForSeconds(healFlashDuration);

        // Restaura a cor original
        spriteRenderer.color = originalColor;
    }

    protected void UpgradeStatus() {
        evolutionEffect.Play();
        upgradeSFX.Play();

        Debug.Log("Deu upgrade no " + attributes.zombieName);

        attributes.damage += 10f;
        attributes.maxHealth += 30f;
        currentHealth = attributes.maxHealth;
        
        healthBar.UpdateSlider(currentHealth);
        timeUntilUpgrade = upgradeTime;
    }

    // Forma simples temporária.
    public void UpgradeFromItem() => spriteRenderer.sprite = upgradedZombie;

    protected GameObject FindClosestEnemy() {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attributes.range);
        
        GameObject closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D collider in enemiesInRange) {
            if (collider.CompareTag("Enemy")) {
                float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                
                if (distanceToEnemy < shortestDistance) {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        return closestEnemy;
    }
}