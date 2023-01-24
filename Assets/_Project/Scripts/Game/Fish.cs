using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] SpriteRenderer fill, border;

    private Camera cam;
    private bool active, moving;
    public bool Active
    {
        get => active;
        set => active = border.enabled = value;
    }

    private void Start()
    {
        cam = Camera.main;
        Active = false;
    }

    private void OnMouseDrag()
    {
        if (!active) return;

        if (!moving)
        {
            moving = true;
            fill.sortingLayerName = "Hover";
            fill.color *= new Color(1, 1, 1, .5f);
            border.color *= new Color(1, 1, 1, .5f);
        }

        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x, pos.y, 0);
    }

    private void OnMouseUp()
    {
        if (!active || !moving) return;

        fill.sortingLayerName = "Default";
        fill.color *= new Color(1, 1, 1, 2);
        border.color += new Color(1, 1, 1, 2);

        moving = false;
        Active = false;
    }
}