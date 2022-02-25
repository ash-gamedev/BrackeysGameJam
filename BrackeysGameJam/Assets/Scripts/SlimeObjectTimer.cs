using System.Collections;
using UnityEngine;

public class SlimeObjectTimer : MonoBehaviour
{
    [SerializeField] float gameTime = 15f;
    float currentTime;
    private bool stopTimer;

    Player player;
    UIManager uIManager;

    public void ResetTimer()
    {
        stopTimer = false;
        FindObjectOfType<UIManager>().SetSlimeObjectMaxSliderValue(gameTime);
        uIManager.SetSlimeObjectSliderValue(gameTime);
        currentTime = gameTime;
    }

    void Start()
    {
        player = GetComponent<Player>();
        uIManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        // decrease time when player is object
        if (player.GetIsPlayerAnObject())
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                stopTimer = true;
                player.RemoveSlimeObject();
                uIManager.ShowSlimeObjectBar(false);
            }

            if (stopTimer == false)
            {
                uIManager.SetSlimeObjectSliderValue(currentTime);
            }
        }
    }
}