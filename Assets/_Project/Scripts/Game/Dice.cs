using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Range(0, 5)][SerializeField] float duration = 2;
    public List<Fish> fish = new List<Fish>();
    [SerializeField] Boat boat;
    [SerializeField] Color red = Color.red;
    [SerializeField] Color green = Color.green;

    private bool rolling = false;
    private SpriteRenderer sprite;

    private void Start() => sprite = GetComponent<SpriteRenderer>();

    private void OnMouseDown()
    {
        if (!rolling) StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        rolling = true;

        foreach (Fish f in fish) f.Active = false;

        sprite.color = new Color(1, 1, 1, .5f);

        yield return new WaitForSeconds(duration);

        int id = Random.Range(0, fish.Count + 1);
        if (id < fish.Count)
        {
            for (int i = 0; i < fish.Count; i++) fish[i].Active = i == id;
            sprite.color = fish[id].GetComponent<SpriteRenderer>().color;
        }
        else
        {
            sprite.color = id == 4 || id == 5 ? red : green;
            boat.Move();
        }

        rolling = false;
    }
}