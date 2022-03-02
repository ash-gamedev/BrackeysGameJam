using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay = 2f;
    public void LoadMainMenu()
    {
        // in case returning after pause
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Tutorial");
        GameManager.Instance.SpawnPlayer();
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void LoadWin()
    {
        StartCoroutine(WaitAndLoad("Win", sceneLoadDelay));
    }
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    IEnumerator WaitAndLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}