using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject obstaclePrefab;
    [Range(0, 10)][SerializeField] int obstacleCount;
    [SerializeField] Vector2Int gridSize;
    [SerializeField] float duration = .1f;
    [SerializeField] Vector2 transparency = new Vector2(.3f, .6f);
    [SerializeField] bool debugPoints;

    [HideInInspector] public bool hovering;
    private Tile[,] grid;
    private List<GameObject> obstacles = new List<GameObject>();
    public Tile[,] Grid => grid;

    private void Awake()
    {
        grid = new Tile[gridSize.x, gridSize.y];
        Vector2 offset = new Vector2(gridSize.x / 2f - .5f, gridSize.y / 2f - .5f);

        //Tiles
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject tile = Instantiate(tilePrefab, transform.position
                    + new Vector3(x - offset.x, y - offset.y, 0), Quaternion.identity);
                tile.transform.SetParent(transform);
                grid[x, y] = tile.GetComponent<Tile>();
            }
        }

        for (int y = 0; y < gridSize.y; y++) grid[6, y].filled = true;

        //Obstacles
        int count = 0;
        while (count < obstacleCount)
        {
            Vector2Int index = new Vector2Int(Random.Range(1, gridSize.x - 2), Random.Range(0, gridSize.y - 1));
            if (grid[index.x, index.y].filled) continue;
            List<Tile> filled = Enumerable.Range(0, gridSize.y).Select(x => grid[index.x, x]).Where(t => t.filled).ToList();
            if (filled.Count > 2) return;

            Transform obstacle = Instantiate(obstaclePrefab, grid[index.x, index.y].transform.position, Quaternion.identity).transform;
            obstacle.SetParent(transform);
            obstacles.Add(obstacle.gameObject);
            grid[index.x, index.y].filled = true;
            count++;
        }
    }

    public IEnumerator Highlight(Transform fish)
    {
        hovering = true;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        Tile[] tiles = GetValidTiles(fish.position);
        SpriteRenderer[] renderers = new SpriteRenderer[tiles.Length];
        for (int i = 0; i < tiles.Length; i++) renderers[i] = tiles[i].img;
        bool[] previous = new bool[tiles.Length];

        foreach (Tile tile in tiles) LeanTween.alpha(tile.gameObject, transparency.x, duration);

        while (hovering)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (renderers[i].bounds.Contains(fish.position) != previous[i])
                {
                    if (previous[i])
                    {
                        previous[i] = false;
                        LeanTween.alpha(renderers[i].gameObject, transparency.x, duration);
                    }
                    else
                    {
                        previous[i] = true;
                        LeanTween.alpha(renderers[i].gameObject, transparency.y, duration);
                    }
                }
            }

            yield return wait;
        }

        foreach (Tile tile in tiles) LeanTween.alpha(tile.gameObject, 0, duration);
    }
    public Tile[] GetValidTiles(Vector3 pos)
    {
        List<Tile> tiles = new List<Tile>();

        Vector2Int start = Vector2Int.zero;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (Vector3.Distance(grid[x, y].transform.position, pos) < .5f)
                {
                    start = new Vector2Int(x, y);
                    break;
                }
            }
        }

        tiles.Add(grid[start.x, start.y]);
        tiles.Add(grid[start.x, Mathf.Clamp(start.y - 1, 0, gridSize.y - 1)]);
        tiles.Add(grid[start.x, Mathf.Clamp(start.y + 1, 0, gridSize.y - 1)]);
        tiles.Add(grid[start.x + 1, start.y]);
        tiles.Add(grid[start.x + 1, Mathf.Clamp(start.y - 1, 0, gridSize.y - 1)]);
        tiles.Add(grid[start.x + 1, Mathf.Clamp(start.y + 1, 0, gridSize.y - 1)]);

        return tiles.Where(t => !t.filled).ToArray();
    }
    public Tile GetClosestTile(Vector3 pos)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (Vector3.Distance(grid[x, y].transform.position, pos) < .5f)
                {
                    return grid[x, y];
                }
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        if (!debugPoints) return;
        if (grid == null) return;

        foreach (Tile tile in grid)
        {
            if (tile.filled) Gizmos.color = Color.red;
            else Gizmos.color = Color.white;
            Gizmos.DrawSphere(tile.transform.position, .2f);
        }
    }
}