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

    void Start()
    {
        UpdatePlayerGemsText();
        ShowSlimeObjectBar(false);
    }

    public void UpdatePlayerGemsText()
    {
        playerGemsText.text = (GameManager.TotalNumberGems + GameManager.NumberGemsThisLevel).ToString() + " x";
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

}