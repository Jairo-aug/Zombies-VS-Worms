using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Passa por cima dos zumbis.
public class FlyingLarva : Larva {
    // Somado em 20;
    protected override int expAmount { get; set; } = 50;

    protected override void Update() {
        timeSinceLastHit += Time.deltaTime;

        // Verifica se o alvo foi definido
        if (target != null) {
            // Calcula a direção para o alvo (desnecessário)
            Vector2 direction = (target.position - transform.position).normalized;

            // Move o inimigo na direção do alvo
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        if (isTouchingPileOfFlesh && pileOfFlesh != null) {
            if (timeSinceLastHit >= attackInterval) {
                // Usar evento para dar dano à pilha.
                pileOfFlesh.TakeDamage(attackDamage); // Aplica dano à vida da Pilha de Carne
                timeSinceLastHit = 0f; // Reseta o tempo
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        // Verifica se o objeto colidido é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPileOfFlesh = true; // Marca que está tocando a Pilha de Carne
        }
    }

    protected override void OnTriggerExit2D(Collider2D collider) {
        // Verifica se o objeto que saiu do trigger é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPileOfFlesh = false; // Marca que não está mais tocando a Pilha de Carne
        }
    }
}