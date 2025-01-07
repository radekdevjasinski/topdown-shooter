using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

[TestFixture]  // Oznacza klasę jako zbiór testów
public class InfiniteWorldTests
{
    private GameObject playerObject;
    private Player player;
    private GameObject worldObject;
    private InfiniteWorld infiniteWorld;

    [SetUp]
    public void Setup()
    {
        //obiekt gracza
        playerObject = new GameObject("Player");
        player = playerObject.AddComponent<Player>();

        //obiekt świata
        worldObject = new GameObject("World");
        infiniteWorld = worldObject.AddComponent<InfiniteWorld>();
        infiniteWorld.player = player;

        //prefab chunk
        GameObject chunkPrefab = new GameObject("Chunk");
        chunkPrefab.AddComponent<TilemapRenderer>();
        worldObject.transform.SetParent(null);  // Unikamy zagnieżdżania w hierarchii
        infiniteWorld.chunkPrefab = chunkPrefab;

        //inicjalizacja
        playerObject.transform.position = new Vector3(0, 0, 0);
        infiniteWorld.Start();
    }

    [UnityTest]
    public IEnumerator TestWorldGenerationAtPlayerPosition1()
    {    
        // Pozycja gracza daleka od 0,0
        Vector2Int newPlayerPos = new Vector2Int(500, -500);

        // Zmiana pozycji gracza na nowe miejsce
        playerObject.transform.position = 
            new Vector3(newPlayerPos.x * InfiniteWorld.gridSizeX, 
            newPlayerPos.y * InfiniteWorld.gridSizeY, 0);

        // Wykonanie metody aktualizującej świat
        infiniteWorld.UpdateWorld();

        yield return new WaitForSeconds(.1f);

        // Sprawdzanie, czy wokół gracza zostały wygenerowane sektory
        Assert.IsTrue(infiniteWorld.activeChunks.ContainsKey(newPlayerPos));
    }
    [UnityTest]
    public IEnumerator TestWorldGenerationAtPlayerPosition2()
    {    
        // Pozycja gracza daleka od 0,0
        Vector2Int newPlayerPos = new Vector2Int(2000, -3);

        // Zmiana pozycji gracza na nowe miejsce
        playerObject.transform.position = 
            new Vector3(newPlayerPos.x * InfiniteWorld.gridSizeX, 
            newPlayerPos.y * InfiniteWorld.gridSizeY, 0);

        // Wykonanie metody aktualizującej świat
        infiniteWorld.UpdateWorld();

        yield return new WaitForSeconds(.1f);

        // Sprawdzanie, czy wokół gracza zostały wygenerowane sektory
        Assert.IsTrue(infiniteWorld.activeChunks.ContainsKey(newPlayerPos));
    }
    [UnityTest]
    public IEnumerator TestWorldGenerationAtPlayerPosition3()
    {    
        // Pozycja gracza daleka od 0,0
        Vector2Int newPlayerPos = new Vector2Int(-700, 3000);

        // Zmiana pozycji gracza na nowe miejsce
        playerObject.transform.position = 
            new Vector3(newPlayerPos.x * InfiniteWorld.gridSizeX, 
            newPlayerPos.y * InfiniteWorld.gridSizeY, 0);

        // Wykonanie metody aktualizującej świat
        infiniteWorld.UpdateWorld();

        yield return new WaitForSeconds(.1f);

        // Sprawdzanie, czy wokół gracza zostały wygenerowane sektory
        Assert.IsTrue(infiniteWorld.activeChunks.ContainsKey(newPlayerPos));
    }
    [UnityTest]
    public IEnumerator TestChunksRemove()
    {
        // ustawienie pozycji gracza na (0,0)
        playerObject.transform.position = new Vector3(0, 0, 0);
        infiniteWorld.UpdateWorld();

        yield return null; //odczekanie jednej klatki
        
        // sprawdzanie liczby aktywnych chunków przed ruchem
        int initialActiveChunks = infiniteWorld.activeChunks.Count;

        // przemieszczenie gracza daleko, aby spowodować usunięcie chunków
        playerObject.transform.position = new Vector3(-300, -300, 0);
        infiniteWorld.UpdateWorld();
        
        yield return null; //odczekanie jednej klatki

        // sprawdzenie, czy stare chunki dalej nie istnieją
        Assert.IsTrue(infiniteWorld.activeChunks.Count == initialActiveChunks);
    }
    [TearDown]
    public void Teardown()
    {
        // Czyszczenie utworzonych obiektów po zakończeniu testów
        GameObject.DestroyImmediate(playerObject);
        GameObject.DestroyImmediate(worldObject);
    }
}

