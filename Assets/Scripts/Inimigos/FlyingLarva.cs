using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Passa por cima dos zumbis.
public class FlyingLarva : Larva {
    // Somado em 20;
    protected override int expAmount { get; set; } = 50;
    
    // Chance padrão: 10%
    protected override float dropChance { get; set; } = 80f;

    protected override void Update() {
        timeSinceLastHit += Time.deltaTime;
        
        direction = (target.position - transform.position).normalized;

        enemyRb.linearVelocity = direction * currentSpeed;

        if (isTouchingfleshStack && fleshStack != null) {
            if (timeSinceLastHit >= attackInterval) {
                // Usar evento para dar dano à pilha.
                fleshStack.TakeDamage(attackDamage); // Aplica dano à vida da Pilha de Carne
                timeSinceLastHit = 0f; // Reseta o tempo
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        // Verifica se o objeto colidido é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingfleshStack = true; // Marca que está tocando a Pilha de Carne
        }
    }

    protected override void OnTriggerExit2D(Collider2D collider) {
        // Verifica se o objeto que saiu do trigger é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingfleshStack = false; // Marca que não está mais tocando a Pilha de Carne
        }
    }
}