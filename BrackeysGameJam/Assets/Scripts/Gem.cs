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
                // play sound effect
                audioPlayer.PlaySoundEffect(Enum.SoundEffects.GemPickUp);

                // delete game object
                Destroy(gameObject);

                // increase coins in game session
                gameSession.IncreasePlayerGems(1);

                isCoinPickedUp = true;
            }
        }
    }
}