using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] float duration = .8f;
    [SerializeField] Dice dice;
    [SerializeField] GameObject ending;

    private GridManager gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
    }

    public void Move()
    {
        foreach (var fish in dice.fish)
        {
            if (Vector3.Distance(transform.position, fish.transform.position) < 1.2f)
            {
                LeanTween.move(gameObject, fish.transform.position, duration).setEase(LeanTweenType.easeInOutExpo);
                dice.fish.Remove(fish);
                LeanTween.scale(fish.gameObject, new Vector3(0, 0, 0), .5f).setEaseInCubic().setDestroyOnComplete(true);
                return;
            }
        }
        var target = GetPosition(gm.GetValidTiles(transform.position));
        LeanTween.move(gameObject, target, duration).setEase(LeanTweenType.easeInOutExpo);
    }
    private Vector3 GetPosition(Tile[] tiles)
    {
        int id = Random.Range(0, tiles.Length);
        if (Vector3.Distance(transform.position, tiles[id].transform.position) < .9f) return GetPosition(tiles);
        return tiles[id].transform.position;
    }

    private void Update()
    {
        if (transform.position.x > 5)
        {
            LeanTween.moveX(gameObject, transform.position.x + 100, .5f).setEaseInCubic();
            LeanTween.alpha(gameObject, 0, .5f).setEaseInCubic().setOnComplete(() =>
            {
                ending.SetActive(true);
                ending.GetComponent<Ending>().title.text = "Das Boot ist gefahren!";
            });
        }
    }
}