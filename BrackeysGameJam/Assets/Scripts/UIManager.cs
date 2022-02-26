using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI playerGemsText;
    [SerializeField] Slider timerSlider;
    [SerializeField] Image slimeObjectImage;
    [SerializeField] GameObject levelCompleteUI;
    [SerializeField] Slider gemSlider;
    [SerializeField] TextMeshProUGUI gemFinalCount;
    [SerializeField] float gemCountDelay = 0.1f;

    bool isLevelComplete = false;
    int totalGems = 0;

    AudioPlayer audioPlayer;

    void Start()
    {
        gemSlider.value = 0;
        gemFinalCount.text = "";
        UpdatePlayerGemsText();
        ShowSlimeObjectBar(false);
        totalGems = GameObject.FindGameObjectsWithTag(Enum.Tags.Gem.ToString()).Length;
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    public void UpdatePlayerGemsText()
    {
        playerGemsText.text = (GameManager.CollectedNumberGems).ToString() + " x";
    }

    public void SetSlimeObjectSliderValue(float value)
    {
        if (timerSlider != null)
        {
            timerSlider.value = value;

            if (value <= 0)
            {
                ShowSlimeObjectBar(false);
            }
        }
    }

    public void SetSlimeObjectMaxSliderValue(float maxValue)
    {
        if (timerSlider != null)
        {
            timerSlider.maxValue = maxValue;
            ShowSlimeObjectBar(true);
        }
    }

    public void SetSlimeObjectImage(Sprite sprite)
    {
        if (slimeObjectImage != null)
        {
            slimeObjectImage.sprite = sprite;
            slimeObjectImage.SetNativeSize();
        }
    }

    public void ShowSlimeObjectBar(bool show)
    {
        if (slimeObjectImage != null && timerSlider != null)
        {
            slimeObjectImage.enabled = show;
            timerSlider.gameObject.SetActive(show);
        }
    }

    public void LevelComplete()
    {
        if (!isLevelComplete)
        {
            isLevelComplete = true;
            levelCompleteUI.SetActive(true);

            // set gems slider value
            gemSlider.maxValue = totalGems;
            StartCoroutine(ShowGemsCollected());
        }
    }

    public void IncreaseGemsSliderValue(int sliderValue)
    {
        audioPlayer.PlaySoundEffect(Enum.SoundEffects.GemPickUp);

        gemSlider.value = sliderValue;
        gemFinalCount.text = sliderValue.ToString() + " / " + totalGems.ToString(); 
    }

    public IEnumerator ShowGemsCollected()
    {
        yield return new WaitForSeconds(1f);

        int gemsCollected = GameManager.CollectedNumberGems;
        int currentGemCount = 0;
        while (currentGemCount <= gemsCollected)
        {
            IncreaseGemsSliderValue(currentGemCount);
            yield return new WaitForSeconds(gemCountDelay);
            currentGemCount++;
        }

        FindObjectOfType<GameManager>().LoadNextLevel();
    }
}