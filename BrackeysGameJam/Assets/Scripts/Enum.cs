using System.Collections;
using UnityEngine;

public class Enum : MonoBehaviour
{
    public enum Tags
    {
        Player,
        Enemy,
        Item,
        Platform
    }

    public enum PlayerAnimation
    {
        Idling,
        Moving,
        Jumping,
        Falling,
        Dashing,
        Dying,
        Swallowing
    }

    public enum SoundEffects
    {
        PlayerJump,
        PlayerDash,
        PlayerDeath,
        EnemyProjectile
    }

}