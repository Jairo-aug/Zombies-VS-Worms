using UnityEngine;

public class TorreSelecionada : MonoBehaviour
{
    public Camera mainCamera; // A câmera principal
    public GameObject torrePrefab; // O prefab da torre
    private GameObject torreInstancia; // A instância da torre selecionada

    private SpriteRenderer torreSpriteRenderer; // O SpriteRenderer da torre selecionada

    void Update()
    {
        // Verifica se a torre está selecionada
        if (torreInstancia != null)
        {
            // Atualiza a posição da torre com base na posição do cursor
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Garante que a torre se mova apenas no plano 2D
            torreInstancia.transform.position = mousePosition;

            // Atualiza a ordem de camada para garantir que a torre fique atrás do cursor
            torreSpriteRenderer.sortingOrder = 1; // Coloca a torre atrás do cursor
        }
    }

    // Método para selecionar a torre e criar uma instância
    public void SelecionarTorre()
    {
        if (torreInstancia != null)
        {
            Destroy(torreInstancia); // Destroi a torre existente antes de criar uma nova
        }

        // Cria uma nova instância da torre e armazena a referência
        torreInstancia = Instantiate(torrePrefab);
        torreSpriteRenderer = torreInstancia.GetComponent<SpriteRenderer>();

        // Inicializa a posição da torre com a posição do cursor
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Garante que a torre se mova apenas no plano 2D
        torreInstancia.transform.position = mousePosition;
    }

    // Método para colocar a torre no chão
    public void ColocarTorre()
    {
        if (torreInstancia != null)
        {
            // Finaliza a colocação da torre
            torreSpriteRenderer.sortingOrder = 0; // Restaura a ordem de camada padrão
            torreInstancia = null; // Limpa a referência da torre
        }
    }
}
