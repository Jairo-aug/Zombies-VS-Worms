using UnityEngine;

public class Item : MonoBehaviour {
    // ATRIBUTOS
    // Pode ser substituido por um scriptable object.
    protected virtual string itemName { get; set; }
    protected virtual Sprite itemSprite { get; set; }
    protected SpriteRenderer spriteRenderer;
    protected ItemState currentState;
    protected BoxCollider2D boxCollider;
    
    public void OnClicked(Inventory inventory) {
        switch(currentState) {
            case ItemState.OnGround:
                Collect(inventory);
                break;
            
            case ItemState.InInventory:
                Drag(inventory);
                break;

            default: return;
        }
    }

    public virtual void InstantiateItem(Sprite sprite, Vector3 lastPosition) {
        transform.position = lastPosition;
        transform.localScale = new Vector3(3f, 3f, 3f);

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        currentState = ItemState.OnGround;
    }

    public void Collect(Inventory inventory) {
        currentState = ItemState.InInventory;

        inventory.AddItem(gameObject);
    }

    public void Drag(Inventory inventory) {
        currentState = ItemState.Dragging;

        inventory.DragItem();
    }
    public void Place(GameObject placedOnZombie) {
        currentState = ItemState.InUse;

        Zombie z = placedOnZombie.GetComponent<Zombie>();
        z.ItemUpgrade();
    }
    protected enum ItemState {
        OnGround,
        InInventory,
        Dragging,
        InUse
    }
}