using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();    
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Enum.Tags.Player.ToString()))
        {
            uiManager.LevelComplete();            
        }

    }

}