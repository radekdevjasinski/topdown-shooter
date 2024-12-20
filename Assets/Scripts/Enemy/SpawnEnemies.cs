using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameController game;
    [SerializeField]
    private GameObject[] enemyPrefab;
    private List<GameObject> enemies;

    [SerializeField]
    private float spawnDistance = 10f;
    [SerializeField]
    private float spawnCooldown = 5f;
    [SerializeField]
    private float maxEnemiesCount = 20;

    [SerializeField] private Camera mainCamera;

    [SerializeField]
    private float removeDistance = 20f;
    [SerializeField]
    private float removeCheckInterval = 5f;

    void Start()
    {
        enemies = new List<GameObject>(enemyPrefab);
        enemies.Sort((a, b) =>
        {
            float threatA = a.GetComponent<Enemy>().threatCost;
            float threatB = b.GetComponent<Enemy>().threatCost;

            return threatB.CompareTo(threatA);
        });
        
        StartCoroutine(SpawnEnemiesPeriodically());
        StartCoroutine(RemoveDistantEnemiesPeriodically());

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
            // Uzyskaj �rodek kamery
            Vector3 cameraPosition = mainCamera.transform.position;

            // Wybierz losowy k�t w radianach
            float angle = Random.Range(0f, 2f * Mathf.PI);

            // Oblicz pozycj� na okr�gu
            Vector3 spawnPosition = new Vector3(
                cameraPosition.x + Mathf.Cos(angle) * spawnDistance,
                cameraPosition.y + Mathf.Sin(angle) * spawnDistance,
                0);

            // Stw�rz przeciwnika na wybranej pozycji
            SpawnEnemy(spawnPosition, ChooseEnemy());
        }
    }
    GameObject ChooseEnemy()
    {
        List<GameObject> eligibleEnemies = new List<GameObject>();
        float highestThreatCost = 0;
        foreach (GameObject enemy in enemies)
        {
            if (game.ThreatCheck(enemy.GetComponent<Enemy>().threatCost))
            {
                if (enemy.GetComponent<Enemy>().threatCost >= highestThreatCost)
                {
                    eligibleEnemies.Add(enemy);
                    highestThreatCost = enemy.GetComponent<Enemy>().threatCost;
                }
            }
        }

        if (eligibleEnemies.Count == 1)
        {
            return eligibleEnemies[0];
        }

        else if (eligibleEnemies.Count > 1)
        {
            int randomIndex = Random.Range(0, eligibleEnemies.Count);
            return eligibleEnemies[randomIndex];
        }

        return null;
    }

    void SpawnEnemy(Vector3 spawnPosition, GameObject enemyPrefab)
    {
        if (enemyPrefab != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
            Enemy enemyClass = enemy.GetComponent<Enemy>();
            enemyClass.playerRef = player.gameObject;
            enemyClass.gameRef = game;
            game.Threat -= enemyPrefab.GetComponent<Enemy>().threatCost;
        }
    }
    IEnumerator SpawnEnemiesPeriodically()
    {
        while (true)
        {
            SpawnEnemyOnCircle();
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
    public List<GameObject> GetClosestEnemies(int count)
    {
        List<GameObject> enemies = new List<GameObject>();
        List<GameObject> children = new List<GameObject>();

        // Dodajemy wszystkich przeciwnik�w do listy
        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }

        // Sortujemy przeciwnik�w wed�ug odleg�o�ci od gracza
        children.Sort((enemy1, enemy2) =>
        {
            float distance1 = Vector3.Distance(player.gameObject.transform.position, enemy1.transform.position);
            float distance2 = Vector3.Distance(player.gameObject.transform.position, enemy2.transform.position);
            return distance1.CompareTo(distance2);
        });

        // Dodajemy najbli�szych przeciwnik�w do listy
        for (int i = 0; i < Mathf.Min(count, children.Count); i++)
        {
            enemies.Add(children[i]);
        }

        return enemies;
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
    IEnumerator RemoveDistantEnemiesPeriodically()
    {
        while (true)
        {
            RemoveDistantEnemies();
            yield return new WaitForSeconds(removeCheckInterval); // Check every 'removeCheckInterval' seconds
        }
    }

    void RemoveDistantEnemies()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }

        foreach (GameObject enemy in children)
        {
            float distance = Vector3.Distance(player.gameObject.transform.position, enemy.transform.position);
            if (distance > removeDistance)
            {
                Destroy(enemy);
            }
        }
    }
}
