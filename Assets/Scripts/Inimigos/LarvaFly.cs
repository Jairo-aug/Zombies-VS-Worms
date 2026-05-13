using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Passa por cima dos zumbis.
public class LarvaFly : Larva {
    // Somado em 20;
    protected override int expAmount { get; set; } = 50;

    protected override void Update() {
        tempoDesdeUltimoDano += Time.deltaTime;

        // Verifica se o alvo foi definido
        if (target != null) {
            // Calcula a direção para o alvo (desnecessário)
            Vector2 direction = (target.position - transform.position).normalized;

            // Move o inimigo na direção do alvo
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        if (isTouchingPilhaDeCarne && pilhaDeCarne != null) {
            if (tempoDesdeUltimoDano >= intervaloDano) {
                pilhaDeCarne.TakeDamage(danoAtaque); // Aplica dano à vida da Pilha de Carne
                tempoDesdeUltimoDano = 0f; // Reseta o tempo
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        // Verifica se o objeto colidido é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPilhaDeCarne = true; // Marca que está tocando a Pilha de Carne
        }
    }

    protected override void OnTriggerExit2D(Collider2D collider) {
        // Verifica se o objeto que saiu do trigger é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne")) {
            isTouchingPilhaDeCarne = false; // Marca que não está mais tocando a Pilha de Carne
        }
    }
}