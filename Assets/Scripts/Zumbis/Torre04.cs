using System.Linq;
using UnityEngine;
using System;

public class Decompoison : Zombie {
    public override string zombieName { get; protected set; } = "Deadefensive";
    public override float zombieCost { get; protected set; } = 150f;
    public override float range { get; protected set; } = 2.5f;
    public override float attackCooldown { get; protected set; } = 2f;
    public override float damage { get; protected set; } = 50f;
    public override float maxHealth { get; protected set; } = 120f;

    [SerializeField] private ParticleSystem bombEffect;

    protected override void Update() {
        Debug.Log($"Cooldown de ataque do Deocmpoision: {attackCooldownTimer}");
        attackCooldownTimer -= Time.deltaTime;

        if (attackCooldownTimer <= 0f)
        {
            AttackArea(); // Realiza o ataque em área
            attackCooldownTimer = attackCooldown;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    void AttackArea()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range);
        if (enemiesInRange.Count() > 1)
        {
            bombEffect.Play();
            somAtaque.Play();
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
                    minhoca.TakeDamage(damage);
                }
                else if (larva != null)
                {
                    larva.TakeDamage(damage);
                }
                else if (verme != null)
                {
                    verme.TakeDamage(damage);
                }
            }
        }
    }
}