using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyFlashlight : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Enemy Trigger");
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Is Player");
                Player player = collision.GetComponent<Player>();
                bool? isPlayerAnObject = player?.GetIsPlayerAnObject();
                if(player != null && isPlayerAnObject == false)
                {
                    Debug.Log("Kill player");
                    player.Die();
                }
            }
        }
    }
}