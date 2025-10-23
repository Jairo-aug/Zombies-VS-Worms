using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorInimigo : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array de prefabs dos inimigos (Minhoca, Verme, Larva)
    public Transform[] spawnPoints; // Array de pontos de spawn
    public int maxSpawnPoints = 3;
    public float spawnInterval = 3.5f; // Intervalo entre cada spawn
    public int maxEnemies = 20; // Número máximo de inimigos permitidos na tela

    private List<GameObject> activeEnemies = new List<GameObject>(); // Lista para controlar os inimigos ativos

    // [SerializeField] float timeBetweenWaves = 5f; 

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            // Verifica se o número de inimigos ativos é menor que o limite máximo
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        // Escolher um ponto de spawn aleatório
        int randomSpawnIndex = Random.Range(0, maxSpawnPoints);
        Transform spawnPoint = spawnPoints[randomSpawnIndex];

        // Escolher aleatoriamente um tipo de inimigo (Minhoca, Verme, Larva)
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];

        // Instanciar o inimigo no ponto de spawn
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Adiciona o novo inimigo à lista de inimigos ativos
        activeEnemies.Add(newEnemy);
        countEnemies();
    }

    public void countEnemies()
    {
        Debug.Log("Enemy list size is: " + activeEnemies.Count);
    }

    public void levelUp()
    {
        activeEnemies.Clear();
        maxEnemies += 10;
        spawnInterval -= 1.4f;
        maxSpawnPoints += 2;
    }
}
