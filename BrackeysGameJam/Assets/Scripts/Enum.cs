using System.Collections;
using UnityEngine;

public class Enum : MonoBehaviour
{
    public enum Tags
    {
        Player,
        Enemy,
        Item,
        Platform,
        Spikes,
        LevelExit,
        Gem
    }

    public enum PlayerAnimation
    {
        Idling,
        Moving,
        Jumping,
        Falling,
        Dashing,
        Dying,
        Swallowing,
        WallSliding
    }

    public enum EnemyAnimation
    {
        Idling,
        Moving,
        LightOn,
        LightOff
    }

    public enum SoundEffects
    {
        PlayerJump,
        PlayerDash,
        PlayerDeath,
        PlayerSwallow,
        EnemyProjectile,
        GemPickUp,
        Win
    }

}