using UnityEngine;

public class InteractionSystem : MonoBehaviour {
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
                item.targetInventory = inventory;
            }

            if (hoveredObject.TryGetComponent<IClickable>(out var clickable)){
                clickable.OnClicked();
            }
        }
    }
}