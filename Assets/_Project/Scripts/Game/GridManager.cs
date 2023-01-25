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
    [SerializeField] bool debugPoints;

    [HideInInspector] public bool hovering;
    private Tile[,] grid;

    private void Awake() => Generate();
    private void Generate()
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

        for (int y = 0; y < gridSize.y; y++) grid[0, y].filled = true;

        //Obstacles
        int count = 0;
        while (count < obstacleCount)
        {
            Tile target = grid[Random.Range(0, gridSize.x - 1), Random.Range(0, gridSize.y - 1)];
            if (target.filled) continue;

            Transform obstacle = Instantiate(obstaclePrefab, target.transform.position, Quaternion.identity).transform;
            obstacle.SetParent(transform);
            target.filled = true;
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

        while (hovering)
        {
            foreach (var img in renderers)
            {
                if (img.bounds.Contains(fish.position)) img.color =
                        new Color(img.color.r, img.color.g, img.color.b, .6f);
                else img.color =
                        new Color(img.color.r, img.color.g, img.color.b, .3f);
            }

            yield return wait;
        }

        foreach (var img in renderers) img.color =
                new Color(img.color.r, img.color.g, img.color.b, 0f);
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

    public Tile GetStartTile(Vector3 pos)
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

        foreach (var tile in grid)
        {
            if (tile.filled) Gizmos.color = Color.red;
            else Gizmos.color = Color.white;
            Gizmos.DrawSphere(tile.transform.position, .2f);
        }
    }
}