using System.Linq;
using UnityEngine;
using System;

public class Decompoison : Zombie {
    [SerializeField] private ParticleSystem bombEffect;

    protected override void Update() {
        Debug.Log($"Cooldown de ataque do Deocmpoision: {attackCooldownTimer}");
        attackCooldownTimer -= Time.deltaTime;

        if (attackCooldownTimer <= 0f)
        {
            AttackArea(); // Realiza o ataque em área
            attackCooldownTimer = attributes.attackCooldown;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    void AttackArea()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attributes.range);
        if (enemiesInRange.Count() > 1)
        {
            bombEffect.Play();
            attackSFX.Play();
        }

        foreach (Collider2D collider in enemiesInRange)
        {
            // REFATORÁVEL
            if (collider.CompareTag("Enemy"))
            {
                // Verifica se o alvo possui os componentes válidos
                Worm minhoca = collider.GetComponent<Worm>();
                Larva larva = collider.GetComponent<Larva>();
                Verm verme = collider.GetComponent<Verm>();

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
    }
}