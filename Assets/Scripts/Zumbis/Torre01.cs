using UnityEngine;
using System;

public class Zomboxer : Zombie {
    public override string zombieName { get; protected set; } = "Zomboxer";
    public override float zombieCost { get; protected set; } = 100f;
    public override float range { get; protected set; } = 0.5f;
    public override float attackCooldown { get; protected set; } = 2f;
    public override float damage { get; protected set; } = 20f;
    public override float maxHealth { get; protected set; } = 50f;

    protected override void Update() {
        attackCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        // Verifica se o cooldown de ataque terminou
        if (attackCooldownTimer <= 0f)
        {
            // Encontra todos os inimigos dentro do alcance de ataque
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range);
            
            foreach (Collider2D collider in enemiesInRange)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Attack(collider.gameObject);
                }
            }

            // Reseta o cooldown de ataque
            attackCooldownTimer = attackCooldown;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
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

        somAtaque.Play();

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