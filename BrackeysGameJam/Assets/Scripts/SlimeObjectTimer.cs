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
        Debug.Log("Resetting Timer");
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
        Debug.Log("Player is object: " + player.GetIsPlayerAnObject());

        // decrease time when player is object
        if (player.GetIsPlayerAnObject())
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                stopTimer = true;
                player.RemoveSlimeObject();
            }

            if (stopTimer == false)
            {
                gameSession.SetSlimeObjectSliderValue(currentTime);
            }
        }
    }
}