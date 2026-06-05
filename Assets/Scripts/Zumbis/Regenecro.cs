using UnityEngine;
using System;
using System.Collections.Generic;

public class Regenecro : Zombie {
    private List<IHealable> nearbyHealables;
    [SerializeField] private int healAmount = 20;
    [SerializeField] private int carnificationAmount = 20;

    protected override void Start() {
        animator = GetComponent<Animator>();
        
        upgradeSFX = GetComponent<AudioSource>();
        deathSFX = GetComponent<AudioSource>();

        healthBar = GetComponentInChildren<SliderBar>();
        currentHealth = attributes.maxHealth;
        healthBar.Set(attributes.maxHealth, currentHealth);
        timeUntilUpgrade = upgradeTime;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;

        GameObject pilhaDeCarneObject = GameObject.FindGameObjectWithTag("PilhaDeCarne");

        if (pilhaDeCarneObject != null) {
            pilhaDeCarne = pilhaDeCarneObject.GetComponent<PilhaDeCarne>();
        }

        nearbyHealables = new();
    }

    protected override void Update() {
        actionCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        nearbyHealables = FindNearbyHealables();

        if (nearbyHealables.Count > 0 && actionCooldownTimer <= 0f) {
            foreach (IHealable healable in nearbyHealables) {
                Heal(healable);
            }

            Fleshificate();
            
            actionCooldownTimer = attributes.actionTime;
        }
    }

    private void Heal(IHealable healable) {
        if (healable.isHealthFull) return;
        
        healable.GetHealed(healAmount);
    }

    // Carnificar -> carnificação.
    private void Fleshificate() {
        pilhaDeCarne.ReceivePointsFromFleshification(carnificationAmount);
        Debug.Log("Carnificação! +20!");
    }

    // Pode ser levado à classe base se houver necessidade em outros zumbis (provável).
    private List<IHealable> FindNearbyHealables() {
        List<IHealable> healables = new();

        Collider2D[] collidersFound = Physics2D.OverlapCircleAll(transform.position, attributes.range);
        
        foreach(Collider2D c in collidersFound) {
            if (c.gameObject == gameObject) continue;
            if (!c.gameObject.TryGetComponent<IHealable>(out IHealable h)) continue;
            
            healables.Add(h);
        }

        return healables;
    }
}