using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ExpAmoun acrescido de 15;
// Health acrescido em 10;
// BonusDano acrescido em 5f;
public class MurshWorm : Worm {
    // Acrescido em 10f
    protected override float maximumHealth { get; set; } = 60f;
    // Acrescido em 5f
    protected override float attackDamage { get; set; } = 15f;
    // Descrescido em 0.2f
    protected override float attackInterval { get; set; } = 1.8f;
    // Acrescido em 15
    protected override int expAmount { get; set; } = 30;
    
    // Chance padrão: 10%
    protected override float dropChance { get; set; } = 80f;
}
