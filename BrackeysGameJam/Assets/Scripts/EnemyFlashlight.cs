using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyFlashlight : MonoBehaviour
    {
        [SerializeField] GameObject enemyArrow;
        bool killPlayer = false;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                bool? isPlayerAnObject = player?.GetIsPlayerAnObject();
                if(player != null && isPlayerAnObject == false && killPlayer == false)
                {
                    killPlayer = true;
                    ShootPlayer();
                }
            }
        }

        void ShootPlayer()
        {
            // instantiate arrow
            Instantiate(enemyArrow,  // what object to instantiate
                        transform.position, // where to spawn the object
                        Quaternion.identity); // need to specify rotation
        }
    }
}