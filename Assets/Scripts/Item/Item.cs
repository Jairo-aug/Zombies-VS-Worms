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

    // Passar o zumbi escolhido como parâmetro tipado como Zombie.
    public void Place(GameObject placedOnZombie) {
        currentState = ItemState.InUse;

        // GAMBIARRA TOTAL TEMPORÁRIA. ISSO NÃO VAI FICAR.
        // Refatorar sistema de zumbis quando terminar.
        Torre01 torre01 = placedOnZombie.GetComponent<Torre01>();
        Torre02 torre02 = placedOnZombie.GetComponent<Torre02>();
        Torre03 torre03 = placedOnZombie.GetComponent<Torre03>();
        Torre04 torre04 = placedOnZombie.GetComponent<Torre04>();
        
        if (torre01 != null) {
            torre01.ItemUpgrade();
        }

        else if (torre02 != null) {
            torre02.ItemUpgrade();
        }

        else if (torre03 != null) {
            torre03.ItemUpgrade();
        }

        else if (torre04 != null) {
            torre04.ItemUpgrade();
        }
        
        Debug.Log("Placed");
    }

    protected enum ItemState {
        OnGround,
        InInventory,
        Dragging,
        InUse
    }
}