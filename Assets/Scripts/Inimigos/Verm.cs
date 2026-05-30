using UnityEngine;
using System.Collections;
using System;

public class Verm : Enemy {
    protected override float maximumSpeed { get; set; } = 3.6f;
    protected override float maximumHealth { get; set; } = 20f;
    protected override float attackDamage { get; set; } = 10f;
    protected override float attackInterval { get; set; } = 1.5f;
    protected override int expAmount { get; set; } = 20;
    
    // Chance padrão: 5%
    protected override float dropChance { get; set; } = 80f;
}