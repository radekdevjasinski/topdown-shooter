using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Collections;

public class InfiniteWorld : MonoBehaviour
{
    [SerializeField] public Player player;
    public GameObject chunkPrefab; //Prefab sektora
    public static int gridSizeX = 99; //rozmiary sektora
    public static int gridSizeY = 96; 

    private Vector3 lastPlayerPosition;
    public Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();

    public void Start()
    {
        activeChunks = new();
        lastPlayerPosition = player.gameObject.transform.position;
        UpdateWorld();
    }
    void Update()
    {
        Vector3 playerPosition = player.gameObject.transform.position;

        if (Mathf.Abs(lastPlayerPosition.x - playerPosition.x) > gridSizeX ||
            Mathf.Abs(lastPlayerPosition.y - playerPosition.y) > gridSizeY)
        {
            UpdateWorld();
            lastPlayerPosition = playerPosition;
        }
    }
    // co jakiś czas, gdy zmieni się pozycja gracza, wygeneruj nowe sektory wokół gracza i usuń stare
    public void UpdateWorld()
    {
        // sprawdź na którym sektorze jest gracz
        Vector2Int playerGridPosition = 
            new Vector2Int(Mathf.RoundToInt(player.gameObject.transform.position.x / gridSizeX), 
            Mathf.RoundToInt(player.gameObject.transform.position.y / gridSizeY));

        // sprawdź sąsiednie sektory
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int gridPos = new Vector2Int(playerGridPosition.x + x, playerGridPosition.y + y);
                float distance = Vector2.Distance(player.gameObject.transform.position, new Vector2(gridPos.x * gridSizeX, gridPos.y * gridSizeY));

                // generuj sektor, jeśli nie istnieje
                if (!activeChunks.ContainsKey(gridPos))
                {
                    GameObject newChunk = CreateGridChunk(gridPos);
                    activeChunks.Add(gridPos, newChunk);

                }
            }
        }

        // Usuń sektory, które są poza zasięgiem
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in activeChunks)
        {
            float distanceX = Mathf.Abs(playerGridPosition.x - chunk.Key.x);
            float distanceY = Mathf.Abs(playerGridPosition.y - chunk.Key.y);

            if (distanceX > 1 || distanceY > 1)
            {
                chunksToRemove.Add(chunk.Key);
            }
        }

        // Usuń sektory z mapy
        foreach (var chunkPos in chunksToRemove)
        {
            Destroy(activeChunks[chunkPos]);
            activeChunks.Remove(chunkPos);
        }
    }

    GameObject CreateGridChunk(Vector2Int gridPosition)
    {
        // Tworzenie nowego sektora
        GameObject newChunk = Instantiate(chunkPrefab);
        newChunk.transform.parent = transform;
        newChunk.transform.localPosition = new Vector3(gridPosition.x * gridSizeX, gridPosition.y * gridSizeY, 0);
        return newChunk;
    }
}
