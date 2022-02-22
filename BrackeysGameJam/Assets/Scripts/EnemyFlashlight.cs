using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyFlashlight : MonoBehaviour
    {
        [SerializeField] GameObject enemyArrow;
        [SerializeField] float flashLightSightDistance;
        [SerializeField] float flashLightOffTime = 0f;
        [SerializeField] float flashLightOnTime = 0f;
        bool killPlayer = false;
        bool flashLightOn = true;

        PolygonCollider2D flashLightCollider;
        SpriteRenderer flashLightSpriteRenderer;

        public void Start()
        {
            flashLightCollider = GetComponent<PolygonCollider2D>();
            flashLightSpriteRenderer = GetComponent<SpriteRenderer>();
            if (flashLightOnTime != 0)
            {
                StartCoroutine(SwitchFlashlight());
            }
        }

        IEnumerator SwitchFlashlight()
        {
            while(killPlayer == false)
            {
                // flashlight on
                flashLightOn = true;
                flashLightCollider.enabled = true;
                flashLightSpriteRenderer.enabled = true;

                yield return new WaitForSeconds(flashLightOnTime);

                // flashlight off
                flashLightOn = false;
                flashLightCollider.enabled = false;
                flashLightSpriteRenderer.enabled = false;

                yield return new WaitForSeconds(flashLightOffTime);
            }
        }

        public bool GetIsKillingPlayer() 
        {
            return killPlayer;
        }

        public bool GetIsFlashLightOn()
        {
            return flashLightOn;
        }
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
            // get crossbow location
            Transform crossbow = gameObject.transform.Find("Crossbow");

            // instantiate arrow
            Instantiate(enemyArrow,  // what object to instantiate
                        crossbow.transform.position, // where to spawn the object
                        Quaternion.identity); // need to specify rotation
        }
    }
}