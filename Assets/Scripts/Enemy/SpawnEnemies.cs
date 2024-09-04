using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab;
    [SerializeField]
    private float spawnDistance = 10f;
    [SerializeField]
    private float spawnCooldown = 5f;
    [SerializeField]
    private float maxEnemiesCount = 20;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnEnemyOnCircle();
    }

    void SpawnEnemyOnCircle()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }
        if (children.Count < maxEnemiesCount)
        {
            // Uzyskaj œrodek kamery
            Vector3 cameraPosition = mainCamera.transform.position;

            // Wybierz losowy k¹t w radianach
            float angle = Random.Range(0f, 2f * Mathf.PI);

            // Oblicz pozycjê na okrêgu
            Vector3 spawnPosition = new Vector3(
                cameraPosition.x + Mathf.Cos(angle) * spawnDistance,
                cameraPosition.y + Mathf.Sin(angle) * spawnDistance,
                0);

            // Stwórz przeciwnika na wybranej pozycji
            SpawnRandomEnemy(spawnPosition);
        }
        
        StartCoroutine(SpawnAfterCooldown());
    }
    void SpawnRandomEnemy(Vector3 spawnPosition)
    {
        // Zsumuj wszystkie wagi
        int totalWeight = 0;
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            totalWeight += enemyPrefab[i].GetComponent<Enemy>().spawnWeight;
        }

        // Wybierz losow¹ liczbê z zakresu od 0 do totalWeight
        int randomValue = Random.Range(0, totalWeight);

        // Przeszukaj listê, aby znaleŸæ odpowiadaj¹cy obiekt
        int cumulativeWeight = 0;
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            cumulativeWeight += enemyPrefab[i].GetComponent<Enemy>().spawnWeight;
            if (randomValue < cumulativeWeight)
            {
                Instantiate(enemyPrefab[i], spawnPosition, Quaternion.identity, transform);
                break;
            }
        }
    }
    IEnumerator SpawnAfterCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
        SpawnEnemyOnCircle();
    }
    public GameObject getClosestEnemy()
    {
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject enemy in children)
        {
            float distance = Vector3.Distance(Player.Instance.gameObject.transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
    public void ClearAllEnemies()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject enemy in children)
        {
            Destroy(enemy);
        }
    }
}
