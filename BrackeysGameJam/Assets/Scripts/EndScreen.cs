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
                StartCoroutine(FadeIntoWinMusic());
            }
        }

        IEnumerator FadeIntoWinMusic()
        {
            AudioPlayer audioPlayer = FindObjectOfType<AudioPlayer>();

            if(audioPlayer != null)
            {
                endMusicPlayed = true;

                float maxVolumne = audioPlayer.audioSource.volume;
                float volumneAdjustment = 0.03f;
                float delayVolumeTime = 0.1f;

                // slowly turn down volume
                while (audioPlayer.audioSource.volume > 0)
                {
                    audioPlayer.audioSource.volume -= volumneAdjustment*2;
                    yield return new WaitForSeconds(delayVolumeTime);
                }

                // play win sound
                float lengthOfAudio = (audioPlayer.soundEffects[Enum.SoundEffects.Win].Item1.length) * 0.5f;
                audioPlayer.PlaySoundEffect(Enum.SoundEffects.Win);
                Debug.Log(lengthOfAudio);
                yield return new WaitForSeconds(lengthOfAudio);

                // play new clip
                audioPlayer.audioSource.clip = endScreenAudioClip;
                audioPlayer.audioSource.Play();

                // slowly increase volumne
                while (audioPlayer.audioSource.volume < maxVolumne)
                {
                    audioPlayer.audioSource.volume += volumneAdjustment;
                    yield return new WaitForSeconds(delayVolumeTime);
                }

            }
            yield return new WaitForSeconds(0f);
        }
    }
}