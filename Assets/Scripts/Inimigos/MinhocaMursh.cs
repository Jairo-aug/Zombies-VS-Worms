using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinhocaMursh : Minhoca
{
    // Ajuste do intervalo de dano e experiência
    protected float intervaloDano = 1.8f; // Novo valor para a subclasse
    protected int expAmount = 30;

    // Aumentar vida e dano
    [SerializeField] private float bonusHealth = 10f; // Vida extra específica da MinhocaMursh
    [SerializeField] private float bonusDano = 5f;    // Dano extra específico da MinhocaMursh

    protected override void Start()
    {
        base.Start(); // Garante que o Start da classe base seja chamado
        maxHealth += bonusHealth;   // Incrementa a vida base
        danoAtaque += bonusDano;   // Incrementa o dano base
        currentHealth = maxHealth; // Atualiza a vida atual para o novo valor máximo

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }
}
