using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] SpriteRenderer fill, border;

    private GridManager grid;
    private Tile[] validTiles;
    private Tile target;
    private Camera cam;
    private bool active, moving;
    public bool Active
    {
        get => active;
        set => active = border.enabled = value;
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
        Tile start = grid.GetStartTile(transform.position);
        if (start != null) start.filled = false;

        SetRenderer("Hover", .5f);

        validTiles = grid.GetValidTiles(transform.position);
        grid.StartCoroutine(grid.Highlight(transform));
    }
    private void OnMouseDrag()
    {
        if (!active) return;

        Vector3 mousPos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector3.Lerp(transform.position, new Vector3(mousPos.x, mousPos.y), .1f);
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

        SetRenderer("Default", 1);
        StartCoroutine(Transition(target.transform.position));
        moving = false;
        target.filled = true;
        grid.hovering = false;
        Active = false;
    }
    private void SetRenderer(string sortingLayer, float alpha)
    {
        fill.sortingLayerName = sortingLayer;
        fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, alpha);
        border.color = new Color(border.color.r, border.color.g, border.color.b, alpha);
    }
    private IEnumerator Transition(Vector3 target)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float time = 0f, fac, duration = .2f;
        Vector3 start = transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            fac = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(Mathf.InverseLerp(0, duration, time));

            transform.position = Vector3.Lerp(start, target, fac);
            yield return wait;
        }

        transform.position = target;
    }
}