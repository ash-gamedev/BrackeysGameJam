using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI playerGemsText;

    [Header("General")]
    [SerializeField] int playerLives = 1;
    [SerializeField] float timeBeforeLevelLoad = 1f;
    [SerializeField] Slider timerSlider;
    [SerializeField] Image slimeObjectImage;

    int playerGems = 0;
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

    void Start()
    {
        UpdatePlayerGemsText();
        ShowSlimeObjectBar(false);
    }
    
    public int GetPlayerGems()
    {
        return playerGems;
    }
    
    public void IncreasePlayerGems(int amount)
    {
        playerGems += amount;
        UpdatePlayerGemsText();
    }

    public void UpdatePlayerGemsText()
    {
        playerGemsText.text =  playerGems.ToString() + " x";
    }

    public void SetSlimeObjectSliderValue(float value)
    {
        timerSlider.value = value;

        if(value <= 0)
        {
            ShowSlimeObjectBar(false);
        }
    }

    public void SetSlimeObjectMaxSliderValue(float maxValue)
    {
        timerSlider.maxValue = maxValue;
        ShowSlimeObjectBar(true);
    }

    public void SetSlimeObjectImage(Sprite sprite)
    {
        slimeObjectImage.sprite = sprite;
        slimeObjectImage.SetNativeSize();
    }

    public void ShowSlimeObjectBar(bool show)
    {
        slimeObjectImage.enabled = show;
        timerSlider.gameObject.SetActive(show);
    }

    void ResetPlayerGems()
    {
        playerGems = 0;
        UpdatePlayerGemsText();
    }

    public void ProcessPlayerDeath()
    {
        TakeLife();
    }

    void TakeLife()
    {
        playerLives--;
        ResetPlayerGems();
        SetSlimeObjectSliderValue(0);

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