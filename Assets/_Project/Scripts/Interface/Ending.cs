using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] GameObject background, button;
    public TextMeshProUGUI title;

    private void Awake()
    {
        LeanTween.moveLocalY(title.gameObject, 100, duration).setEaseOutQuad();
        LeanTween.scale(button, new Vector3(1, 1, 1), duration).setEaseOutQuad();
        LeanTween.alphaCanvas(title.GetComponent<CanvasGroup>(), 1, duration).setEaseOutQuad();
        LeanTween.alphaCanvas(background.GetComponent<CanvasGroup>(), 1, duration * 2).setEaseOutQuad();
    }

    public void Reload() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}