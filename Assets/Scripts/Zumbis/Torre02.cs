using UnityEngine;
using System;

public class Deadefensive : Zombie {
    protected override void Update() {
        if (timeUntilUpgrade <= 0f) UpgradeStatus();
    }
}
