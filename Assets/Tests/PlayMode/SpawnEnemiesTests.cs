using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class SpawnEnemiesTests
{
    private GameObject playerObject;
    private Player player;
    private GameObject gameControllerObject;
    private GameController game;
    private GameObject spawnEnemiesObject;
    private SpawnEnemies spawnEnemies;
    private Camera mainCamera;

    [SetUp]
    public void Setup()
    {
        // Tworzenie obiektu gracza
        playerObject = new GameObject("Player");
        player = playerObject.AddComponent<Player>();

        // Tworzenie obiektu GameController
        gameControllerObject = new GameObject("GameController");
        game = gameControllerObject.AddComponent<GameController>();
        game.threatSpeed = 0.3f;

        // Tworzenie obiektu do generowania przeciwników
        spawnEnemiesObject = new GameObject("SpawnEnemies");
        spawnEnemies = spawnEnemiesObject.AddComponent<SpawnEnemies>();
        spawnEnemies.player = player;
        spawnEnemies.game = game;

        // Dodanie kamery głównej
        mainCamera = new GameObject("MainCamera").AddComponent<Camera>();
        spawnEnemies.mainCamera = mainCamera;

/*
        // Inicjalizacja pozostałych wartości
        spawnEnemies.spawnDistance = 10f;
        spawnEnemies.spawnCooldown = 1f;
        spawnEnemies.maxEnemiesCount = 20f;
        spawnEnemies.removeDistance = 20f;
        spawnEnemies.removeCheckInterval = 1f;
        */
    }

    [UnityTest]
    public IEnumerator TestSpawnEnemy1()
    {
        //wartość zagrożenia umożliwiająca generowanie przeciwnika
        game.Threat = 100;

        //wykonaj próbę generowania przeciwnika
        spawnEnemies.SpawnEnemyOnCircle();

        yield return null; // poczekaj na jedną klatkę

        //sprawdź, czy wygenerowano jednego przeciwnika
        Assert.AreEqual(1, spawnEnemies.transform.childCount);
    }

    [UnityTest]
    public IEnumerator TestSpawnEnemy2()
    {
       //wartość zagrożenia, która nie pozwala na generowanie przeciwnika
        game.Threat = 0;

        //wykonaj próbę generowania przeciwnika
        spawnEnemies.SpawnEnemyOnCircle();

        yield return null; // poczekaj na jedną klatkę

        //upewnij się, że nie wygenerowano przeciwnika
        Assert.AreEqual(0, spawnEnemies.transform.childCount);
    }
    [UnityTest]
    public IEnumerator TestSpawnEnemy3()
    {
        //wartość zagrożenia umożliwiająca generowanie przeciwnika
        game.Threat = 100;

        //wykonaj próbę generowania przeciwnika
        spawnEnemies.SpawnEnemyOnCircle();

        yield return null; // poczekaj na jedną klatkę

        //upewnij się, że wartość zagrożenia się zmniejszyła
        Assert.True(game.Threat < 100);
    }
     [UnityTest]
    public IEnumerator TestSpawnEnemy4()
    {
        //wartość zagrożenia umożliwiająca generowanie przeciwnika
        game.Threat = 100;

        //wykonaj próbę generowania przeciwnika
        spawnEnemies.SpawnEnemyOnCircle();

        yield return null; // poczekaj na jedną klatkę

        //pobierz referencję wygenerowanego przeciwnika
        GameObject enemy = spawnEnemies.transform.GetChild(0).gameObject;

        //upewnij się, że został wygenerowany w odpowiedniej odległości
        Assert.True(Vector2.Distance(playerObject.transform.position, enemy.transform.position) >= spawnEnemies.spawnDistance - 1);
    }
    [UnityTest]
    public IEnumerator TestThreat()
    {
        game.Threat = 1;
        game.Update();
        yield return null;
        Assert.True(game.Threat > 1);
    }


    [UnityTest]
    public IEnumerator TestRemoveDistantEnemies()
    {
        game.Threat = 100;
        spawnEnemies.SpawnEnemyOnCircle();
        GameObject enemy = spawnEnemies.transform.GetChild(0).gameObject;

        enemy.transform.position = new Vector3(100, 100, 0); // Przeciwnik daleko od gracza
        spawnEnemies.RemoveDistantEnemies();

        yield return null;

        Assert.AreEqual(0, spawnEnemies.transform.childCount);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.DestroyImmediate(playerObject);
        GameObject.DestroyImmediate(gameControllerObject);
        GameObject.DestroyImmediate(spawnEnemiesObject);
        GameObject.DestroyImmediate(mainCamera.gameObject);
    }
}

