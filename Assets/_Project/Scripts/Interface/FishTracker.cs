using UnityEngine;

public class FishTracker : MonoBehaviour
{
    [SerializeField] GameObject ending;
    [HideInInspector] public int count;

    private void Update()
    {
        if (count > 3)
        {
            ending.SetActive(true);
            ending.GetComponent<Ending>().title.text = "Alle Fische sind entkommen!";
        }
    }
}