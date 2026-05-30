using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Se move mais rápido em 15f.
// Dá 20 de xp a mais.

public class FlashVerm : Verm {
    // Somado em 15f;
    protected override float maximumSpeed { get; set; } = 18.6f;
    // Somado em 20;
    protected override int expAmount { get; set; } = 40;
    
    // Chance padrão: 10%
    protected override float dropChance { get; set; } = 80f;
}
