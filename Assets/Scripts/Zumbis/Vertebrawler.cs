using UnityEngine;
using System;

public class Vertebrawler : Zombie {
    // Prefabs
    [SerializeField] protected GameObject projectilePrefab;
    protected virtual float projectileLifetime { get; } = 3f;
    protected bool isUpgrade;
    
    protected override void Update() {
        actionCooldownTimer -= Time.deltaTime;
        timeUntilUpgrade -= Time.deltaTime;

        GameObject closestEnemy = FindClosestEnemy();

        if (closestEnemy != null && actionCooldownTimer <= 0f) {
            Attack(closestEnemy);
            actionCooldownTimer = attributes.actionTime;
        }

        if (timeUntilUpgrade <= 0f) UpgradeStatus();
    }

    protected override void Attack(GameObject target) {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Bone bone = projectile.GetComponent<Bone>();
        
        bone.SetTarget(target);
        
        if (isUpgrade == true) bone.UpgradeStatus();       

        // Ignorar a colisão entre o projétil e o inimigo para evitar interação física
        Collider2D enemyCollider = target.GetComponent<Collider2D>();
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        
        if (enemyCollider != null && projectileCollider != null) {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }

        Destroy(projectile, projectileLifetime);
    }
}
