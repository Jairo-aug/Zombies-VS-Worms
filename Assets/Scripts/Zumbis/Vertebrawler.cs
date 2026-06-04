using UnityEngine;
using System;

public class Vertebrawler : Zombie {
    // Prefabs
    [SerializeField] private GameObject projectilePrefab;
    private float projectileLifetime = 3f;
    private bool isUpgrade;
    
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
        Projetil projScript = projectile.GetComponent<Projetil>();
        
        if (projScript != null) projScript.SetTarget(target);
        
        else Debug.LogError("Script 'Projetil' não encontrado no prefab do projétil.");

        if(isUpgrade == true) projScript.UpgradeStatus();       

        // Ignorar a colisão entre o projétil e o inimigo para evitar interação física
        Collider2D enemyCollider = target.GetComponent<Collider2D>();
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        
        if (enemyCollider != null && projectileCollider != null) {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }

        Destroy(projectile, projectileLifetime);
    }
}
