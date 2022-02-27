using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] AudioClip endScreenAudioClip;
        bool endMusicPlayed = false;

        private void Update()
        {
            if(endMusicPlayed == false)
            {
                FadeIntoWinMusic();
            }
        }

        void FadeIntoWinMusic()
        {
            AudioPlayer audioPlayer = FindObjectOfType<AudioPlayer>();

            if(audioPlayer != null)
            {
                endMusicPlayed = true;

                float maxVolumne = audioPlayer.audioSource.volume;
                float volumneAdjustment = 0.01f;

                // slowly turn down volume
                while (audioPlayer.audioSource.volume > 0)
                {
                    audioPlayer.audioSource.volume -= volumneAdjustment;
                }

                // play win sound
                FindObjectOfType<AudioPlayer>().PlaySoundEffect(Enum.SoundEffects.Win);

                // play new clip
                audioPlayer.audioSource.clip = endScreenAudioClip;
                audioPlayer.audioSource.Play();

                // slowly increase volumne
                while (audioPlayer.audioSource.volume < maxVolumne)
                {
                    audioPlayer.audioSource.volume += volumneAdjustment;
                }

            }
            
        }
    }
}