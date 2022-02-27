using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] AudioClip mainMenuAudioClip;
        AudioPlayer audioPlayer;
        bool musicPlayed = false;

        private void Update()
        {
            if (musicPlayed == true) return;
            if (audioPlayer == null) audioPlayer = FindObjectOfType<AudioPlayer>();
            if (audioPlayer != null)
            {
                if (audioPlayer.audioSource.clip == mainMenuAudioClip)
                    musicPlayed = true;
                else
                    FadeIntoMusic();
            }
            
        }

        void FadeIntoMusic()
        {
            musicPlayed = true;

            float maxVolumne = audioPlayer.audioSource.volume;
            float volumneAdjustment = 0.01f;

            // slowly turn down volume
            while (audioPlayer.audioSource.volume > 0)
            {
                audioPlayer.audioSource.volume -= volumneAdjustment;
            }

            // play new clip
            audioPlayer.audioSource.clip = mainMenuAudioClip;
            audioPlayer.audioSource.Play();

            // slowly increase volumne
            while (audioPlayer.audioSource.volume < maxVolumne)
            {
                audioPlayer.audioSource.volume += volumneAdjustment;
            }
        }
    }
}