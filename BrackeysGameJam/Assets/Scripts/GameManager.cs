using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject player;
    private Vector3 spawnPoint;
    public bool checkPointReached;

    [Header("UI")]
    static TextMeshProUGUI playerGemsText;

    [Header("General")]
    static bool isPlayerAlive = true;
    static float timeBeforeLevelLoad = 1f;

    public static int CollectedNumberGems { get; set; }

    static int lastLevelIndex;
    static int currentLevelIndex;

    public static GameManager Instance;

    private void Awake()
    {
        // only want one game session
        int numGameSessions = FindObjectsOfType<GameManager>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            Instance = this;
        }
    }

    public static void IncreasePlayerGems(int amount)
    {
        if (isPlayerAlive == false) return;
        CollectedNumberGems += amount;
        FindObjectOfType<UIManager>().UpdatePlayerGemsText();
    }
    
    public static void ResetSession()
    {
        CollectedNumberGems = 0;
    }

    static void ResetPlayerGems()
    {
        CollectedNumberGems = 0;
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

    public void SetSpawnPoint(Vector3 newSpawn)
    {
        spawnPoint = newSpawn;
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
        StartCoroutine(LoadLevel(currentSceneIndex, timeBeforeLevelLoad));

        ResetLevelVariables();
    }

    private static IEnumerator LoadLevel(int sceneIndex, float delay)
    {
        FindObjectOfType<UIManager>().ShowSlimeObjectBar(false);

        yield return new WaitForSecondsRealtime(delay);

        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextLevel()
    {
        CollectedNumberGems = 0;
        Instance.checkPointReached = false;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        StartCoroutine(LoadLevel(nextSceneIndex, timeBeforeLevelLoad*2f));
    }

    public void SpawnPlayer()
    {
        Debug.Log("spawned at: " + spawnPoint);

        // instantiate player at spawnpoint
        Instantiate(player,  // what object to instantiate
                    spawnPoint, // where to spawn the object
                    Quaternion.identity); // need to specify rotation
    }
}