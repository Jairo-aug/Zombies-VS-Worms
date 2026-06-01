using UnityEngine;
using System;

public class Vertebrawler : Zombie {
    public override string zombieName { get; protected set; } = "Vertebrawler";
    public override float zombieCost { get; protected set; } = 150f;
    public override float range { get; protected set; } = 4.4f;
    public override float attackCooldown { get; protected set; } = 2f;
    public override float damage { get; protected set; } = 0f;
    public override float maxHealth { get; protected set; } = 50f;

    // Prefabs
    [SerializeField] private GameObject projectilePrefab;
    private float projectileLifetime = 3f;
    private bool isUpgrade;
    
    protected override void Update() {
        attackCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range);
        GameObject closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D collider in enemiesInRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = collider.gameObject;
                }
            }
        }

        if (closestEnemy != null && attackCooldownTimer <= 0f)
        {
            Attack(closestEnemy);
            attackCooldownTimer = attackCooldown;
        }

        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }

    protected override void Attack(GameObject target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        // Obtenha o script do projétil após a instância
        Projetil projScript = projectile.GetComponent<Projetil>();
        
        if (projScript != null)
        {
            projScript.SetTarget(target);
        }
        else
        {
            Debug.LogError("Script 'Projetil' não encontrado no prefab do projétil.");
        }

        if(isUpgrade == true)
        {
            projScript.UpgradeStatus();       
        }

        // Ignorar a colisão entre o projétil e o inimigo para evitar interação física
        Collider2D enemyCollider = target.GetComponent<Collider2D>();
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        
        if (enemyCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }

        Destroy(projectile, projectileLifetime);
    }
}
