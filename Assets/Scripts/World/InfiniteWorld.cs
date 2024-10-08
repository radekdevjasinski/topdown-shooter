using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class InfiniteWorld : MonoBehaviour
{
    private GameObject chunkPrefab; //Prefab sektora
    private int gridSizeX; //rozmiary sektora
    private int gridSizeY; 

    private Vector3 lastPlayerPosition;
    public Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        lastPlayerPosition = Player.Instance.gameObject.transform.position;
        chunkPrefab = gameObject.transform.GetChild(0).gameObject;
        gridSizeX = Mathf.CeilToInt(chunkPrefab.GetComponent<TilemapRenderer>().bounds.size.x);
        gridSizeY = Mathf.CeilToInt(chunkPrefab.GetComponent<TilemapRenderer>().bounds.size.y);
        UpdateWorld();
    }

    void Update()
    {
        Vector3 playerPosition = Player.Instance.gameObject.transform.position;

        if (Mathf.Abs(lastPlayerPosition.x - playerPosition.x) > gridSizeX ||
            Mathf.Abs(lastPlayerPosition.y - playerPosition.y) > gridSizeY)
        {
            UpdateWorld();
            lastPlayerPosition = playerPosition;
        }
    }
    void UpdateWorld()
    {
        Vector2Int playerGridPosition = new Vector2Int(Mathf.RoundToInt(Player.Instance.gameObject.transform.position.x / gridSizeX), Mathf.RoundToInt(Player.Instance.gameObject.transform.position.y / gridSizeY));
        // Sprawd� s�siednie sektory w promieniu 1 jednostki gridu
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int gridPos = new Vector2Int(playerGridPosition.x + x, playerGridPosition.y + y);
                float distance = Vector2.Distance(Player.Instance.gameObject.transform.position, new Vector2(gridPos.x * gridSizeX, gridPos.y * gridSizeY));

                // Generuj sektor, je�li nie istnieje
                if (!activeChunks.ContainsKey(gridPos))
                {
                    GameObject newChunk = CreateGridChunk(gridPos);
                    activeChunks.Add(gridPos, newChunk);

                }
            }
        }

        // Usu� sektory, kt�re s� poza zasi�giem
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

        // Usu� sektory z mapy
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
