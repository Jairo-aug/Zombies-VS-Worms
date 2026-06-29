using UnityEngine;
using System;

public class Contaminator : Decompoison {
    [SerializeField] protected float hypnotizationChance = 10f;
    [SerializeField] protected float hypnotizationTime = 7f;

    protected void HypnotizationAttempt(Enemy enemy) {
        if (RNG.RollChance100(hypnotizationChance)) {
            Hypnotize(enemy);
        }
    }

    protected void Hypnotize(Enemy enemy) {
        StartCoroutine(enemy.GetHypnotized(hypnotizationTime));
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Enemy enemy)) {
            HypnotizationAttempt(enemy);
        }
    }
}