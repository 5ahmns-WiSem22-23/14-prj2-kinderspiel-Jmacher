using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] SpriteRenderer border;

    private bool active = false;
    public bool Active
    {
        get => active;
        set => active = border.enabled = value;
    }

    private void Start() => Active = false;

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }

}