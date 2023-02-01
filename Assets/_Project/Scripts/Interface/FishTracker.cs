using TMPro;
using UnityEngine;

public class FishTracker : MonoBehaviour
{
    [SerializeField] GameObject ending;
    [SerializeField] TextMeshProUGUI txt;
    [HideInInspector] int count;

    private void Update()
    {
        if (count > 3)
        {
            ending.SetActive(true);
            ending.GetComponent<Ending>().title.text = "Alle Fische sind entkommen!";
        }
    }

    public void Count()
    {
        count++;
        txt.text = count.ToString();
    }
}