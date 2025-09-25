using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Inspector'dan ayarlayabilirsin
    [SerializeField] private bool useBuildIndex = false; // Ýstersen build index de kullanabilirsin

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Player taglý objeye temas ederse
        {
            if (useBuildIndex)
            {
                int currentIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentIndex + 1); // sýradaki sahneyi yükler
            }
            else
            {
                SceneManager.LoadScene(nextSceneName); // inspector'dan ismini girdiðin sahneyi yükler
            }
        }
    }
}
