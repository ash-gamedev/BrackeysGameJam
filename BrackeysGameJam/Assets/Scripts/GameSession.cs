using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 1;
    [SerializeField] float timeBeforeLevelLoad = 1f;

    // Awake happens before start
    void Awake()
    {
        // only want one game session
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        TakeLife();
    }

    void TakeLife()
    {
        playerLives--;

        //reload current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(currentSceneIndex));
    }

    void ResetGameSession()
    {
        StartCoroutine(LoadLevel(0));
        // destroy this instance of game session to get totally fresh new game session
        // when reset to start
        Destroy(gameObject);
    }

    private IEnumerator LoadLevel(int sceneIndex)
    {
        yield return new WaitForSecondsRealtime(timeBeforeLevelLoad);

        SceneManager.LoadScene(sceneIndex);
    }
}