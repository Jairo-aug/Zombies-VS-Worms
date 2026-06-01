using UnityEngine;
using System;

public class Deadefensive : Zombie {
    public override string zombieName { get; protected set; } = "Deadefensive";
    public override float zombieCost { get; protected set; } = 200f;
    public override float range { get; protected set; } = 0.5f;
    public override float attackCooldown { get; protected set; } = 2f;
    public override float damage { get; protected set; } = 0f;
    public override float maxHealth { get; protected set; } = 300f;

    protected override void Update()
    {
        if (timeUntilUpgrade <= 0f)
        {
            UpgradeStatus();
        }
    }
}
