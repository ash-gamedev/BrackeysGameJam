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

    public static int TotalNumberGems { get; set; }
    public static int TotalNumberGamesInGame { get; set; }
    public static int NumberGemsThisLevel { get; set; }

    static int lastLevelIndex;
    static int currentLevelIndex;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static int GetPlayerGems()
    {
        return TotalNumberGems;
    }
    
    public static void IncreasePlayerGems(int amount)
    {
        if (isPlayerAlive == false) return;
        NumberGemsThisLevel += amount;
        FindObjectOfType<UIManager>().UpdatePlayerGemsText();
    }
    
    public static void ResetSession()
    {
        TotalNumberGems = 0;
        NumberGemsThisLevel = 0;
    }

    static void ResetPlayerGems()
    {
        NumberGemsThisLevel = 0;
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

    public static void ResetLevelVariables()
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

        ResetLevelVariables();
    }

    private static IEnumerator LoadLevel(int sceneIndex)
    {
        FindObjectOfType<UIManager>().ShowSlimeObjectBar(false);

        yield return new WaitForSecondsRealtime(timeBeforeLevelLoad);

        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextLevel()
    {
        TotalNumberGems += NumberGemsThisLevel;
        NumberGemsThisLevel = 0;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        StartCoroutine(LoadLevel(nextSceneIndex));
    }
}