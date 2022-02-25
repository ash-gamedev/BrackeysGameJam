using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Enum.Tags.Player.ToString()))
            FindObjectOfType<GameManager>().LoadNextLevel();
    }

}