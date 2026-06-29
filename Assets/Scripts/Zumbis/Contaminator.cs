using UnityEngine;
using System;

public class Contaminator : Decompoison {
    [SerializeField] protected float hypnotizationChance = 100f;
    [SerializeField] protected float hypnotizationTime = 10f;

    protected void HypnotizationAttempt() {
        
    }

    protected void HypnotizeAttempt() {
        
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            StartCoroutine(enemy.GetHypnotized(hypnotizationTime));
        }
    }
}