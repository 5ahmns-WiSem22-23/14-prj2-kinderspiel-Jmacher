using UnityEngine;

public class Boat : MonoBehaviour
{
    //[SerializeField] float duration = .8f;

    private GridManager gm;

    private void Awake() => gm = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();

    public void Move()
    {
        Tile[] tiles = gm.GetValidTiles(transform.position);
        transform.position = tiles[Random.Range(0, tiles.Length)].transform.position;
    }
}