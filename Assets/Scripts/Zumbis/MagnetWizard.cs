using UnityEngine;
using System;

public class MagnetWizard : Deadefensive {
    [SerializeField] private BoxCollider2D attractionTrigger;

    protected override void Start() {
        base.Initialize();

        attractionTrigger.size *= attributes.range;
    }
}