using UnityEngine;
using System;

public class ShovelKnight : Zomboxer {
    private Vector2 repelDirection;
    private float repelStrength = 10f;

    protected override void Update() {
        // Como é um zumbi reativo, ele não precisa disto.
        //actionCooldownTimer -= Time.deltaTime;

        timeUntilUpgrade -= Time.deltaTime;

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attributes.range);
        
        foreach (Collider2D collider in enemiesInRange) {
            if (!collider.CompareTag("Enemy")) continue;
            
            Enemy enemyComponent = collider.gameObject.GetComponentInChildren<Enemy>();
            if (ShouldReactToEnemy(collider.transform)) {
                Debug.Log($"react to {collider.gameObject.name}");
                RepelEnemy(collider);
            }
        }

        if (timeUntilUpgrade <= 0f) UpgradeStatus();
    }

    private void RepelEnemy(Collider2D enemy) {
        repelDirection = -GetFacingDirection();
        Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();

        enemyComponent.Knockback(repelDirection, repelStrength);
    }

    private Vector2 GetFacingDirection() => (fleshStack.transform.position - transform.position).normalized;
}