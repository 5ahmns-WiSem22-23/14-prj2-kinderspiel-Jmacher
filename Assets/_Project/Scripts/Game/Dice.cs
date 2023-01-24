using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [Range(0, 5)][SerializeField] int duration = 2;
    //[SerializeField] FishManager fishManager;
    [SerializeField] Fish[] fish = new Fish[2];

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

        sprite.color = new Color(.5f, .5f, .5f);

        yield return new WaitForSeconds(duration);

        sprite.color = new Color(1, 1, 1);

        int id = Random.Range(0, fish.Length);
        //fishManager.active = fish[id].transform;
        for (int i = 0; i < fish.Length; i++) fish[i].Active = i == id;

        rolling = false;
    }
}