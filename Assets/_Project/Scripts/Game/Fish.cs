using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] SpriteRenderer fill, border, shadow;
    [SerializeField] FishTracker tracker;
    [SerializeField] Dice dice;
    [SerializeField] float duration = .2f;
    [SerializeField] float smoothing = .1f;

    private GridManager grid;
    private Tile[] validTiles;
    private Tile target;
    private Camera cam;
    private bool active, moving;
    public bool Active
    {
        get => active;
        set
        {
            active = value;
            LeanTween.alpha(border.gameObject, value ? 1f : 0f, duration).setEase(LeanTweenType.easeOutQuad);
        }
    }

    private void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
        cam = Camera.main;
        Active = false;
    }

    private void OnMouseDown()
    {
        if (!active) return;

        moving = true;
        Tile start = grid.GetClosestTile(transform.position);
        if (start != null) start.filled = false;

        LeanTween.alpha(shadow.gameObject, .6f, duration).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveLocal(shadow.gameObject, new Vector3(-.04f, -.04f), duration).setEase(LeanTweenType.easeOutQuad);

        validTiles = grid.GetValidTiles(transform.position);

        grid.StartCoroutine(grid.Highlight(transform));
    }
    private void OnMouseDrag()
    {
        if (!active) return;

        Vector3 mousPos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector3.Lerp(transform.position, new Vector3(mousPos.x, mousPos.y), smoothing);
    }
    private void OnMouseUp()
    {
        if (!active || !moving) return;

        float shortestDistance = 9999;
        foreach (Tile tile in validTiles)
        {
            float dist = Vector3.Distance(transform.position, tile.transform.position);
            if (shortestDistance > dist)
            {
                target = tile;
                shortestDistance = dist;
            }
        }
        LeanTween.move(gameObject, target.transform.position, duration).setEase(LeanTweenType.easeOutQuad);
        LeanTween.alpha(shadow.gameObject, .3f, duration).setEase(LeanTweenType.easeOutQuad);
        LeanTween.moveLocal(shadow.gameObject, new Vector3(.02f, .02f), duration).setEase(LeanTweenType.easeOutQuad);

        moving = false;
        target.filled = true;
        grid.hovering = false;
        Active = false;

        if (target.transform.position.x > 5)
        {
            dice.fish.Remove(this);
            target.filled = false;
            LeanTween.moveX(gameObject, transform.position.x + 8, .5f).setEaseInCubic();
            LeanTween.alpha(gameObject, 0, .5f).setEaseInCubic().setOnComplete(() =>
            {
                tracker.count++;
                Destroy(gameObject);
            });
        }
    }
}