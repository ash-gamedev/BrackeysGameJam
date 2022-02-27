using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] AudioClip endScreenAudioClip;

        void Start()
        {
            FadeIntoWinMusic();

            
        }

        void FadeIntoWinMusic()
        {
            float maxVolumne = AudioPlayer.audioSource.volume;
            float volumneAdjustment = 0.01f;

            // slowly turn down volume
            while (AudioPlayer.audioSource.volume > 0)
            {
                AudioPlayer.audioSource.volume -= volumneAdjustment;
            }

            // play win sound
            FindObjectOfType<AudioPlayer>().PlaySoundEffect(Enum.SoundEffects.Win);

            // play new clip
            AudioPlayer.audioSource.clip = endScreenAudioClip;
            AudioPlayer.audioSource.Play();

            // slowly increase volumne
            while(AudioPlayer.audioSource.volume < maxVolumne)
            {
                AudioPlayer.audioSource.volume += volumneAdjustment;
            }
        }
    }
}