using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class InfiniteWorld : MonoBehaviour
{
    [SerializeField] private Player player;
    private GameObject chunkPrefab; //Prefab sektora
    private int gridSizeX; //rozmiary sektora
    private int gridSizeY; 

    private Vector3 lastPlayerPosition;
    public Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        lastPlayerPosition = player.gameObject.transform.position;
        chunkPrefab = gameObject.transform.GetChild(0).gameObject;
        gridSizeX = Mathf.CeilToInt(chunkPrefab.GetComponent<TilemapRenderer>().bounds.size.x);
        gridSizeY = Mathf.CeilToInt(chunkPrefab.GetComponent<TilemapRenderer>().bounds.size.y);
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
    // Co jakiś czas, gdy zmieni się pozycja gracza, wygeneruj nowe sektory wokół gracza i usuń stare
    void UpdateWorld()
    {
        // Sprawdź na którym sektorze jest gracz
        Vector2Int playerGridPosition = 
            new Vector2Int(Mathf.RoundToInt(player.gameObject.transform.position.x / gridSizeX), 
            Mathf.RoundToInt(player.gameObject.transform.position.y / gridSizeY));

        // Sprawdź sąsiednie sektory w promieniu 1 jednostki gridu
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int gridPos = new Vector2Int(playerGridPosition.x + x, playerGridPosition.y + y);
                float distance = Vector2.Distance(player.gameObject.transform.position, new Vector2(gridPos.x * gridSizeX, gridPos.y * gridSizeY));

                // Generuj sektor, jeśli nie istnieje
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
