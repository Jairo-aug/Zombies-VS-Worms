using System.Linq;
using UnityEngine;
using System;

public class Decompoison : Zombie {
    [SerializeField] private ParticleSystem bombEffect;

    protected override void Update() {
        actionCooldownTimer -= Time.deltaTime;

        if (actionCooldownTimer <= 0f) {
            Attack();
            actionCooldownTimer = attributes.actionTime;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    protected override void Attack() {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attributes.range);
        
        if (enemiesInRange.Count() > 1) {
            bombEffect.Play();
            attackSFX.Play();
        }

        foreach (Collider2D collider in enemiesInRange) {
            // REFATORÁVEL NO FUTURO

            if (collider.CompareTag("Enemy")) {
                Worm minhoca = collider.GetComponent<Worm>();
                Larva larva = collider.GetComponent<Larva>();
                Verm verme = collider.GetComponent<Verm>();

                if (minhoca != null)  minhoca.TakeDamage(attributes.damage);
                else if (larva != null) larva.TakeDamage(attributes.damage);
                else if (verme != null) verme.TakeDamage(attributes.damage);
            }
        }
    }
}