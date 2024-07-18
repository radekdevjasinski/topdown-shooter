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
            float chance = Random.Range(0, 100);
            if (chance <=90)
            {
                Instantiate(enemyPrefab[0], spawnPosition, Quaternion.identity, transform);

            }
            else
            {
                Instantiate(enemyPrefab[1], spawnPosition, Quaternion.identity, transform);

            }
        }
        
        StartCoroutine(SpawnAfterCooldown());
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
