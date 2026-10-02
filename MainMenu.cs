using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public void OnStartClick()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Play()
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void OnExitClick()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}
