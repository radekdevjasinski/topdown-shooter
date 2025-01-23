using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField]
    public Player player;
    [SerializeField]
    public GameController game;
    [SerializeField]
    private List<GameObject> enemyTypes;
    public List<GameObject> enemies;

    [SerializeField]
    public float spawnDistance = 10f;
    [SerializeField]
    private float spawnCooldown = 2f;
    private float maxEnemiesCount = 200;

    [SerializeField] public Camera mainCamera;

    [SerializeField]
    private float removeDistance = 20f;
    [SerializeField]
    private float removeCheckInterval = 5f;
    private static float cooldownDecreaseInterval = 15f; // Czas, po którym zmniejszy się spawnCooldown

    public void Reset()
    {
        spawnCooldown = 2f;
    }
    void Start()
    {
        GameObject[] enemyPrefabArray = Resources.LoadAll<GameObject>("Prefabs/Enemies");
        enemyTypes = new List<GameObject>(enemyPrefabArray);
        enemyTypes.Sort((a, b) =>
        {
            float threatA = a.GetComponent<Enemy>().threatCost;
            float threatB = b.GetComponent<Enemy>().threatCost;

            return threatB.CompareTo(threatA);
        });
        
        enemies = new();

        StartCoroutine(SpawnEnemiesPeriodically());
        StartCoroutine(RemoveDistantEnemiesPeriodically());
        StartCoroutine(DecreaseSpawnCooldownPeriodically());


    }
    // Generuj przeciwnika na okręgu
    public void SpawnEnemyOnCircle()
    {
        //Jeżeli maksymalna ilość przeciwników w świecie nie jest przekroczona,
        //przystąp do generowania
        if (enemies.Count < maxEnemiesCount)
        {
            // Uzyskaj środek kamery, tym samym pozycję gracza
            Vector3 cameraPosition = mainCamera.transform.position;

            // Wybierz losowy kąt w radianach
            float angle = Random.Range(0f, 2f * Mathf.PI);

            // Oblicz pozycję na okręgu
            Vector3 spawnPosition = new Vector3(
                cameraPosition.x + Mathf.Cos(angle) * spawnDistance,
                cameraPosition.y + Mathf.Sin(angle) * spawnDistance,
                0);

            // Stwórz przeciwnika na wybranej pozycji, z wybranym typem
            SpawnEnemy(spawnPosition, ChooseEnemy());
        }
    }
    //wybierz typ przeciwnika
    public GameObject ChooseEnemy()
    {
        List<GameObject> eligibleEnemies = new List<GameObject>();
        
        //znajdź najtrudniejszych przeciwników którzy mogą się wygenerować
        float highestThreatCost = 0;
        foreach (GameObject enemy in enemyTypes)
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

        //jeżeli tylko jeden typ przeciwnika jest możliwy, 
        //zwróć go
        if (eligibleEnemies.Count == 1)
        {
            return eligibleEnemies[0];
        }

        //jeżeli kilka typów przeciwników może się pojawić, 
        //zwróć losowego z nich
        else if (eligibleEnemies.Count > 1)
        {
            int randomIndex = Random.Range(0, eligibleEnemies.Count);
            return eligibleEnemies[randomIndex];
        }

        //jeżeli żaden nie może się wygenerować, zwróć wartość null
        return null;
    }

    public void SpawnEnemy(Vector3 spawnPosition, GameObject enemyPrefab)
    {
        if (enemyPrefab != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
            Enemy enemyClass = enemy.GetComponent<Enemy>();
            enemyClass.playerRef = player.gameObject;
            enemyClass.gameRef = game;
            game.Threat -= enemyPrefab.GetComponent<Enemy>().threatCost;

            enemies.Add(enemy);
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
        List<GameObject> sortedEnemies = enemies;
        List<GameObject> closestEnemies = new();


        // Sortujemy przeciwnik�w wed�ug odleg�o�ci od gracza
        sortedEnemies.Sort((enemy1, enemy2) =>
        {
            float distance1 = Vector3.Distance(player.gameObject.transform.position, enemy1.transform.position);
            float distance2 = Vector3.Distance(player.gameObject.transform.position, enemy2.transform.position);
            return distance1.CompareTo(distance2);
        });

        // Dodajemy najbli�szych przeciwnik�w do listy
        for (int i = 0; i < Mathf.Min(count, sortedEnemies.Count); i++)
        {
            closestEnemies.Add(sortedEnemies[i]);
        }

        return closestEnemies;
    }

    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        enemies = new();
    }
    IEnumerator RemoveDistantEnemiesPeriodically()
    {
        while (true)
        {
            RemoveDistantEnemies();
            yield return new WaitForSeconds(removeCheckInterval); // Check every 'removeCheckInterval' seconds
        }
    }

    public void RemoveDistantEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.gameObject.transform.position, enemy.transform.position);
            if (distance > removeDistance)
            {
                Destroy(enemy);
                enemies.Remove(enemy);
            }
        }
    }
    // Coroutine do zmniejszania spawnCooldown
    IEnumerator DecreaseSpawnCooldownPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownDecreaseInterval);

            // Zmniejszamy spawnCooldown o 0.1, ale nie poniżej 0.5f
            spawnCooldown = Mathf.Max(spawnCooldown - 0.1f, 0.5f);
        }
    }
}
