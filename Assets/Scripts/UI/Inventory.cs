using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] private GameObject cursor;
    public GameObject storedItem;
    private bool isItemBeingDraggedOut;

    private void Update() {
        if (isItemBeingDraggedOut) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            cursor.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

            if (Input.GetMouseButtonUp(0)) {
                isItemBeingDraggedOut = false;

                Debug.Log("Plop");
                
                cursor.SetActive(false);
                Place();
            }
        } 
    }

    public void AddItem(GameObject newItem) {
        RemoveItem();

        Transform itemTransform = newItem.transform;
        itemTransform.SetParent(transform, false);
        itemTransform.localPosition = Vector3.zero;

        // QUANDO ADICIONARMOS OS ITENS MESMO, REMOVER ESTA LINHA OU MUDAR VALOR
        // SE NÃO O ITEM VAI FICAR GIGANTE. ISSO É GAMBIARRA.
        itemTransform.localScale *= 400;

        SpriteRenderer renderer = newItem.GetComponentInChildren<SpriteRenderer>();
        renderer.sortingOrder = 1;

        storedItem = newItem;
    }

    public void DragItem() {
        SpriteRenderer renderer = storedItem.GetComponentInChildren<SpriteRenderer>();
        Sprite itemSprite = renderer.sprite;

        if (itemSprite is null) {
            Debug.LogError("Sprite do item a ser carregado é nulo.");
            return;
        }

        Debug.Log("Item sendo carregado");
        isItemBeingDraggedOut = true;

        SpriteRenderer cursorRenderer = cursor.GetComponent<SpriteRenderer>();
        cursorRenderer.sprite = itemSprite;
        cursor.SetActive(true);

        // QUANDO ADICIONARMOS OS ITENS MESMO, REMOVER ESTA LINHA OU MUDAR VALOR
        // SE NÃO O ITEM VAI FICAR GIGANTE. ISSO É GAMBIARRA.
        cursor.transform.localScale = new(4f, 4f, 4f);

        storedItem.SetActive(false);
    }

    public void Place() {
        storedItem.GetComponent<Item>().Place();
        RemoveItem();
    }

    private void RemoveItem() {
        if (storedItem) {
            Destroy(storedItem);
            storedItem = null;
        }
    }
}