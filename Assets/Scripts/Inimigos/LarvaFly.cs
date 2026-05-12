using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaFly : Larva
{
    protected int expAmount = 50;

    public new void Start()
    {
        base.Start(); 
    }
    
    protected new void Update()
    {
        tempoDesdeUltimoDano += Time.deltaTime; 

        // Verifica se o alvo foi definido
        if (target != null)
        {
            // Calcula a direção para o alvo
            Vector2 direction = (target.position - transform.position).normalized;

            // Move o inimigo na direção do alvo
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }

        if (isTouchingPilhaDeCarne && pilhaDeCarne != null)
        {
            if (tempoDesdeUltimoDano >= intervaloDano)
            {
                pilhaDeCarne.TakeDamage(danoAtaque); // Aplica dano à vida da Pilha de Carne
                tempoDesdeUltimoDano = 0f; // Reseta o tempo
            }
        }
    }

    protected new void OnTriggerEnter2D(Collider2D collider)
    {
        // Verifica se o objeto colidido é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne"))
        {
            isTouchingPilhaDeCarne = true; // Marca que está tocando a Pilha de Carne
        }
    }

    protected new void OnTriggerExit2D(Collider2D collider)
    {
        // Verifica se o objeto que saiu do trigger é a Pilha de Carne
        if (collider.CompareTag("PilhaDeCarne"))
        {
            isTouchingPilhaDeCarne = false; // Marca que não está mais tocando a Pilha de Carne
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

}