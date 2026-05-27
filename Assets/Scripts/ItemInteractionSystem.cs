using UnityEngine;

public class ItemInteractionSystem : MonoBehaviour {
    public Camera cam;
    public Inventory inventory;

    private void Start() {
        cam = Camera.main;
    } 

    private void Update() {
        Vector3 mousePosition = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit2D raycastHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
        GameObject hoveredObject = raycastHit2D ? raycastHit2D.collider.gameObject : null;
        
        if (Input.GetMouseButton(0) && hoveredObject != null) {
            if (hoveredObject.TryGetComponent<Item>(out var item)) {
                item.OnHoveredAndClicked(inventory);
            }
        }
    }
}