using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyFlashlight : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                bool? isPlayerAnObject = collision.GetComponent<Player>()?.GetIsPlayerAnObject();
                if(isPlayerAnObject == false)
                {
                    KillPlayer(collision.gameObject);
                }
            }
        }

        void KillPlayer(GameObject player)
        {
            Destroy(player);
        }
    }
}