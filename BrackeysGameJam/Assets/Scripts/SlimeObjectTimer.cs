using System.Collections;
using UnityEngine;

public class SlimeObjectTimer : MonoBehaviour
{
    [SerializeField] float gameTime = 15f;
    float currentTime;
    private bool stopTimer;

    Player player;
    GameSession gameSession;

    public void ResetTimer()
    {
        stopTimer = false;
        gameSession.SetSlimeObjectMaxSliderValue(gameTime);
        gameSession.SetSlimeObjectSliderValue(gameTime);
        currentTime = gameTime;
    }

    void Start()
    {
        player = GetComponent<Player>();
        gameSession = FindObjectOfType<GameSession>();
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
                gameSession.ShowSlimeObjectBar(false);
            }

            if (stopTimer == false)
            {
                gameSession.SetSlimeObjectSliderValue(currentTime);
            }
        }
    }
}