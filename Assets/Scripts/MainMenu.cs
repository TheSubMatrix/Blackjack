using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Changes the scene to the game scene
    /// </summary>
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Game Scene");
    }
    /// <summary>
    /// Quits the game
    /// </summary>
    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
