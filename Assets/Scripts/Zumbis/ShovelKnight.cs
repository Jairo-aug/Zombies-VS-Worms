using UnityEngine;
using System;

public class ShovelKnight : Zomboxer {
    private Vector2 randomRepelDirection;
    private float repelStrength = 100f;
    private float paralizationLength = 4;

    protected override void Update() {
        timeUntilUpgrade -= Time.deltaTime;

        if (timeUntilUpgrade <= 0f) UpgradeStatus();
    }

    private void RepelEnemy(Enemy enemy) {
        randomRepelDirection = GetRandomDirection();
        Enemy enemyComponent = enemy.GetComponentInChildren<Enemy>();

        StartCoroutine(enemyComponent.GetRepelled(randomRepelDirection, repelStrength, paralizationLength));
    }
    
    private Vector2 GetRandomDirection() {
        Vector2 facingDirection = GetFacingDirection();
        int randomAngle = UnityEngine.Random.Range(-75, 75);

        Vector3 direction = new Vector3(-facingDirection.x, 0, -facingDirection.y);
        Vector3 rotation = Quaternion.Euler(0, randomAngle, 0) * direction;

        return new Vector2(rotation.x, rotation.z);
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            Debug.Log("Repelling");
            RepelEnemy(enemy);
        }
    }
}