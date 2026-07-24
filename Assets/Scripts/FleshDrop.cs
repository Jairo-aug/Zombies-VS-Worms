using System;
using UnityEngine;

public class FleshDrop : MonoBehaviour, IClickable {
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    public int rottenPointsAmount;

    public static event Action<int> OnFleshClicked;

    public void OnClicked() {
        OnFleshClicked?.Invoke(rottenPointsAmount);
        SelfDestroy();
    }

    public void InstantiateDrop(Sprite sprite, Vector3 lastPosition) {
        transform.position = lastPosition;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        
        spriteRenderer.sortingLayerName = "Drops";
    }

    private void SelfDestroy() => Destroy(gameObject);
}