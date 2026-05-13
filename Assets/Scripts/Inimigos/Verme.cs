using UnityEngine;
using System.Collections;
using System;

public class Verme : Enemy {
    protected override float maxSpeed { get; set; } = 3.6f;
    protected override float maxHealth { get; set; } = 20f;
    protected override float danoAtaque { get; set; } = 10f;
    protected override float intervaloDano { get; set; } = 1.5f;
    protected override int expAmount { get; set; } = 20;
}