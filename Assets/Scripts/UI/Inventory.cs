using UnityEngine;

public class Inventory : MonoBehaviour {
    public GameObject storedItem;

    public void AddItem(GameObject newItem) {
        if (storedItem) {
            Destroy(storedItem);
            storedItem = null;
        }

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
}