using UnityEngine;
using System;
using System.Collections.Generic;

public class Regenecro : Zombie {
    private List<GameObject> nearbyZombies;
    private bool isPileNearby {
        get {
            return Vector2.Distance(transform.position, pilhaDeCarne.gameObject.transform.position) <= attributes.range;
        }
    }

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

        nearbyZombies = new();
    }

    protected override void Update() {
        actionCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        nearbyZombies = FindNearbyZombies();
        Debug.Log("Is pile near? " + isPileNearby);

        if (nearbyZombies.Count > 0 && actionCooldownTimer <= 0f) {
            Heal(nearbyZombies, isPileNearby);
            actionCooldownTimer = attributes.actionTime;
        }
    }

    private void Heal(List<GameObject> zombiesToHeal, bool shouldHealPile) {
        // Criar interface IHealable para healar o que deve ser healado.
    }

    // Pode ser levado à classe base se houver necessidade em outros zumbis (provável).
    private List<GameObject> FindNearbyZombies() {
        List<GameObject> zombies = new();

        Collider2D[] zombiesCollidersFound = Physics2D.OverlapCircleAll(transform.position, attributes.range);

        
        
        foreach(Collider2D c in zombiesCollidersFound) {
            if (c.gameObject == gameObject) continue;
            if (!c.gameObject.TryGetComponent<Zombie>(out Zombie z)) continue;
            
            zombies.Add(z.gameObject);

            Debug.Log("Found a zombie " + z.gameObject.GetComponent<Zombie>().attributes.name);
        }

        return zombies;
    }
}