using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Level select screen
    public void LevelSelect()
    {
        SceneManager.LoadScene(2); 
    }

    // retry button
    public void Retry()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
