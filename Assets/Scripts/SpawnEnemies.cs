using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float spawnDistance = 10f;
    [SerializeField]
    private float spawnCooldown = 5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnEnemyOnCircle();
    }

    void SpawnEnemyOnCircle()
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
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
        StartCoroutine(SpawnAfterCooldown());
    }
    IEnumerator SpawnAfterCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
        SpawnEnemyOnCircle();
    }
}
