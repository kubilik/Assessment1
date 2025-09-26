using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void QuitButton()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");
    }

}
