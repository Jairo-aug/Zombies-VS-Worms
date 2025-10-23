using UnityEngine;

public class GerenciadorTorreLimite : MonoBehaviour
{
    public GameObject[] torresPrefabs; // Array de prefabs de torres
    private bool[] torresColocadas; // Array para rastrear se cada torre foi colocada

    void Start()
    {
        // Inicializa o array de torres colocadas com o tamanho do número de prefabs
        torresColocadas = new bool[torresPrefabs.Length];
    }

    public void MarcarTorreComoColocada(int idTorre)
    {
        // Marca a torre como colocada
        if (idTorre >= 0 && idTorre < torresColocadas.Length)
        {
            torresColocadas[idTorre] = true;
        }
    }

    public bool TorrePodeSerColocada(int idTorre)
    {
        // Verifica se a torre pode ser colocada
        if (idTorre >= 0 && idTorre < torresColocadas.Length)
        {
            return !torresColocadas[idTorre];
        }
        return false;
    }
}
