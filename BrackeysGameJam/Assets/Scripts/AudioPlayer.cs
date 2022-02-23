using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{    
    [Header("Player")]
    [SerializeField] AudioClip playerJump;
    [SerializeField] float playerJumpVolume;
    [SerializeField] AudioClip playerDash;
    [SerializeField] float playerDashVolume;
    [SerializeField] AudioClip playerDeath;
    [SerializeField] float playerDeathVolume;
    [SerializeField] AudioClip playerSwallow;
    [SerializeField] float playerSwallowVolume;

    [Header("Enemy")]
    [SerializeField] AudioClip enemyProjectile;
    [SerializeField] float enemyProjectileVolume;

    [Header("Other")]
    [SerializeField] AudioClip gemPickUp;
    [SerializeField] float gemPickUpVolume;

    // disctionary
    Dictionary<Enum.SoundEffects, (AudioClip, float)> soundEffects;

    // static persists through all instances of a class
    Player player;
    static AudioPlayer instance;

    private void Awake()
    {
        ManageSingleton();

        soundEffects = new Dictionary<Enum.SoundEffects, (AudioClip, float)>
            {
                { Enum.SoundEffects.PlayerJump, (playerJump, playerJumpVolume) },
                { Enum.SoundEffects.PlayerDash, (playerDash, playerDashVolume) },
                { Enum.SoundEffects.PlayerDeath, (playerDeath, playerDeathVolume) },
                { Enum.SoundEffects.EnemyProjectile, (enemyProjectile, enemyProjectileVolume) },
                { Enum.SoundEffects.GemPickUp, (gemPickUp, gemPickUpVolume) },
                { Enum.SoundEffects.PlayerSwallow, (playerSwallow, playerSwallowVolume) }
            };
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void ManageSingleton()
    {
        if (instance != null)
        {
            // need to disable this so other objects don't try to access
            gameObject.SetActive(false);

            // now destroy
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundEffect(Enum.SoundEffects soundEffectName)
    {
        Vector3 soundPos = Camera.main.transform.position; // (soundEffectName.ToString().Contains("Player") ? player.transform.position : Camera.main.transform.position);
        (AudioClip, float) soundEffect = soundEffects[soundEffectName];
        AudioClip audioClip = soundEffect.Item1;
        float volume = soundEffect.Item2;

        AudioSource.PlayClipAtPoint(audioClip, soundPos, volume);
    }
}