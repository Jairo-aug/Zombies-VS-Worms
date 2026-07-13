using UnityEngine;
using System;

public class Dartastic : Vertebrawler {
    // Prefabs
    protected override float projectileLifetime { get; } = 3f;

    protected override void Attack(GameObject target) {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Dart dart = projectile.GetComponent<Dart>();
        
        if (dart != null) dart.SetDirection(-GetFacingDirection());
        
        if(isUpgrade == true) dart.UpgradeStatus();       

        // Ignorar a colisão entre o projétil e o inimigo para evitar interação física
        Collider2D enemyCollider = target.GetComponent<Collider2D>();
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        
        if (enemyCollider != null && projectileCollider != null) {
            Physics2D.IgnoreCollision(projectileCollider, enemyCollider);
        }

        Destroy(projectile, projectileLifetime);
    }
}