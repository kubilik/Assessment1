using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private bool useBuildIndex = false;
    [SerializeField] private AudioSource finishSound;  // inspector’dan atayabilirsin 
    [SerializeField] private float delayBeforeLoad = 2f; // saniye cinsinden bekleme süresi

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlaySoundAndLoad());
        }
    }

    private IEnumerator PlaySoundAndLoad()
    {
        if (finishSound != null)
        {
            finishSound.Play();
        }

        yield return new WaitForSeconds(delayBeforeLoad);

        if (useBuildIndex)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
