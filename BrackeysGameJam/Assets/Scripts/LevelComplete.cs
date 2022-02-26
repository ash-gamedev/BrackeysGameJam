using System.Collections;
using UnityEngine;
public class LevelComplete : MonoBehaviour
{
    UIManager uIManager;
    [SerializeField] float countDelay = 0.1f;

    public void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
    }
    
    public IEnumerable ShowGemsCollected()
    {
        Debug.Log("Starting ShowGemsCollected");
        int gemsCollected = GameManager.CollectedNumberGems;
        int currentGemCount = 0;
        while(currentGemCount < gemsCollected)
        {
            uIManager.IncreaseGemsSliderValue(currentGemCount);
            yield return new WaitForSeconds(countDelay);
            currentGemCount++;
        }
        Debug.Log("Finished!");
    }
}