using UnityEngine;
using UnityEngine.UI; // Certifique-se de importar este namespace para acessar o Text

public class GameManager : MonoBehaviour
{
    public GameObject cursorTorre; // Sprite que segue o cursor quando uma torre é comprada
    public GameObject[] prefabsTorres; // Array de prefabs de torres
    public Grid grid; // Referência ao Grid
    public Text pontosText; // Referência ao campo de texto para exibir pontos
    public Font minhaFonte; // Fonte personalizada para o texto

    private FleshStack fleshStack; // Referência ao script que gerencia os pontos

    void Start()
    {
        // Obtém a referência ao script fleshStack
        fleshStack = FindObjectOfType<FleshStack>();

        if (fleshStack == null)
        {
            Debug.LogError("fleshStack não encontrada na cena.");
        }

        // Configura a fonte personalizada no campo de texto
        if (pontosText != null && minhaFonte != null)
        {
            pontosText.font = minhaFonte;
        }
    }

    // Método para comprar uma torre com base em um ID
    public void ComprarTorre(int idTorre)
    {
        if (idTorre >= 0 && idTorre < prefabsTorres.Length)
        {
            GameObject torre = prefabsTorres[idTorre];
            int custo = GetTorreCusto(torre);

            // Verifica se o jogador tem pontos suficientes
            if (fleshStack.rottenPoints < custo)
            {
                Debug.Log("rottenPoints insuficientes para comprar esta torre!");
                return;
            }

            // Carrega o sprite da torre comprada no cursor
            Sprite torreSprite = torre.GetComponent<SpriteRenderer>().sprite;
            if (torreSprite != null)
            {
                SpriteRenderer cursorRenderer = cursorTorre.GetComponent<SpriteRenderer>();
                if (cursorRenderer != null)
                {
                    cursorRenderer.sprite = torreSprite;
                    cursorTorre.SetActive(true);
                }
                else
                {
                    Debug.LogError("SpriteRenderer não encontrado no cursorTorre.");
                }
            }
            else
            {
                Debug.LogError("Sprite da torre não encontrado para o ID: " + idTorre);
            }

            // Deduz os pontos do jogador
            fleshStack.rottenPoints -= custo;
            AtualizarUI(); // Atualiza a UI após deduzir os pontos
        }
        else
        {
            Debug.LogError("ID de torre inválido: " + idTorre);
        }
    }

    // Método para obter o custo da torre a partir de um prefab
    private int GetTorreCusto(GameObject torre)
    {
        // Obtém o componente de torre do prefab
        Zombie z = GetComponent<Zombie>();

        if (z != null)
        {
            // Usa reflexão para obter o campo custo
            var custoField = z.GetType().GetField("custo");
            return custoField != null ? (int)custoField.GetValue(z) : 0;
        }

        Debug.LogError("Componente da torre não encontrado.");
        return 0;
    }

    // Método para atualizar a UI com os pontos restantes
    private void AtualizarUI()
    {
        if (pontosText != null)
        {
            pontosText.text = "Pontos: " + fleshStack.rottenPoints;
        }
    }
}
