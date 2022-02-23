using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gem : MonoBehaviour
    {
        bool isCoinPickedUp = false;
        AudioPlayer audioPlayer;
        GameSession gameSession;

        private void Start()
        {
            audioPlayer = FindObjectOfType<AudioPlayer>();
            gameSession = FindObjectOfType<GameSession>();
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isCoinPickedUp)
            {
                // TODO play sound effect
                //audioPlayer.PlaySoundEffect

                // delete game object
                Destroy(gameObject);

                // increase coins in game session
                gameSession.IncreasePlayerGems(1);

                isCoinPickedUp = true;
            }
        }
    }
}