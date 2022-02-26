using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    static TextMeshProUGUI playerGemsText;

    [Header("General")]
    static bool isPlayerAlive = true;
    static float timeBeforeLevelLoad = 1f;

    public static int NumberGems { get; set; }

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static int GetPlayerGems()
    {
        return NumberGems;
    }
    
    public static void IncreasePlayerGems(int amount)
    {
        if (isPlayerAlive == false) return;
        NumberGems += amount;
        FindObjectOfType<UIManager>().UpdatePlayerGemsText();
    }
    
    static void ResetPlayerGems()
    {
        NumberGems = 0;
        FindObjectOfType<UIManager>().UpdatePlayerGemsText();
    }

    public static void ProcessPlayerDeath()
    {
        try
        {
            Instance.TakeLife();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void TakeLife()
    {
        isPlayerAlive = false;
        ResetLevel();
    }

    public static void ResetGame()
    {
        isPlayerAlive = true;
        ResetPlayerGems();
        FindObjectOfType<UIManager>().SetSlimeObjectSliderValue(0);
        FindObjectOfType<UIManager>().ShowSlimeObjectBar(false);
    }

    void ResetLevel()
    {
        //reload current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(currentSceneIndex));

        ResetGame();
    }

    private static IEnumerator LoadLevel(int sceneIndex)
    {
        FindObjectOfType<UIManager>().ShowSlimeObjectBar(false);

        yield return new WaitForSecondsRealtime(timeBeforeLevelLoad);

        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        StartCoroutine(LoadLevel(nextSceneIndex));
    }
}