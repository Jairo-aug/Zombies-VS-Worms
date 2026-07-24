using UnityEngine;
using System;

public class TheTower : Deadefensive {
    public event Action Relocate;

    public override void OnClicked() {
        Relocate?.Invoke();
    }
}