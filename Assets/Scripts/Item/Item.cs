using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IHoverable {
    // ATRIBUTOS
    // Pode ser substituido por um scriptable object.
    protected virtual string itemName { get; set; }
    protected virtual Sprite itemSprite { get; set; }
    protected SpriteRenderer spriteRenderer;
    protected ItemState currentState;
    protected BoxCollider2D boxCollider;
    
    public virtual void Update() {
        if (currentState == ItemState.Dropped) {
            Vector3 mousePosition = Input.mousePosition;
            Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit2D raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
            Transform hoveredObject = raycastHit2D ? raycastHit2D.collider.transform : null;
            bool isSameObject = hoveredObject != null && hoveredObject == gameObject.transform;

            if (isSameObject && Input.GetMouseButton(0)) {
                Debug.Log("Clicou e passou o mouse por cima da " + itemName + "!");
            }
        }
    }

    public virtual void InstantiateItem(Sprite sprite, Vector3 lastPosition) {
        transform.position = lastPosition;
        transform.localScale = new Vector3(3f, 3f, 3f);

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        boxCollider = gameObject.AddComponent<BoxCollider2D>();

        currentState = ItemState.Dropped;
    }

    protected enum ItemState {
        Dropped,
        InInventory,
        InUse
    }
}