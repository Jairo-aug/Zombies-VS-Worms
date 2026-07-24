using UnityEngine;
using System;

public class Zomboxer : Zombie {
    protected override void Update() {
        actionCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        if (actionCooldownTimer <= 0f) {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attributes.range);
            
            foreach (Collider2D collider in enemiesInRange) {
                if (collider.CompareTag("Enemy")) {
                    Attack(collider.gameObject);
                }
            }

            actionCooldownTimer = attributes.actionTime;
        }

        if (timeUntilUpgrade <= 0f) UpgradeStatus();
    }

    protected override void Attack(GameObject target) {
        if (animator != null)
        {
            animator.SetTrigger("IsAttacking");
            Debug.Log("animatoração de ataque acionada!");
        }

        // Verifica e aplica dano com base no tipo de inimigo
        // REFATORÁVEL
        Worm minhoca = target.GetComponent<Worm>();
        Larva larva = target.GetComponent<Larva>();
        Verm verme = target.GetComponent<Verm>();

        attackSFX.Play();

        if (minhoca != null)
        {
            minhoca.TakeDamage(attributes.damage);
        }
        else if (larva != null)
        {
            larva.TakeDamage(attributes.damage);
        }
        else if (verme != null)
        {
            verme.TakeDamage(attributes.damage);
        }
    }
}