using UnityEngine;
using System;

public class ShovelKnight : Zomboxer {
    private Vector2 repelDirection;
    private float repelStrength = 100f;

    protected override void Update() {
        timeUntilUpgrade -= Time.deltaTime;

        if (timeUntilUpgrade <= 0f) UpgradeStatus();
    }

    private void RepelEnemy(Enemy enemy) {
        repelDirection = -GetFacingDirection();
        Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();

        StartCoroutine(enemyComponent.GetRepelled(repelDirection, repelStrength, 5));
    }

    private Vector2 GetFacingDirection() => (fleshStack.transform.position - transform.position).normalized;

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            Debug.Log("Repelling");
            RepelEnemy(enemy);
        }
    }
}