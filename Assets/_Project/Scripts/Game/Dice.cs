using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Range(0, 5)][SerializeField] int duration = 2;
    [SerializeField] Fish[] fish = new Fish[2];
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

        int id = Random.Range(0, 7);
        if (id < 4)
        {
            for (int i = 0; i < fish.Length; i++) fish[i].Active = i == id;
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