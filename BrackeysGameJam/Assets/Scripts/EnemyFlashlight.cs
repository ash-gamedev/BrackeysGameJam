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
        [SerializeField] float attackDelay = 0.2f; // allow a small amount of time where player is moving in light, before trying to kill
        bool canShootPlayer = false;
        bool playerInLight = false;
        bool shootPlayer = false;
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
            while(shootPlayer == false)
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
            return shootPlayer;
        }

        public bool GetIsFlashLightOn()
        {
            return flashLightOn;
        }

        void SetCanShootPlayer()
        {
            if (playerInLight)
                canShootPlayer = true;
            else
                canShootPlayer = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Enum.Tags.Player.ToString()) && !playerInLight)
            {
                playerInLight = true;
                Invoke("SetCanShootPlayer", attackDelay);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag(Enum.Tags.Player.ToString()) && canShootPlayer)
            {
                Player player = collision.GetComponent<Player>();
                bool? isPlayerAnObject = player?.GetIsPlayerAnObject();
                if(player != null && isPlayerAnObject == false && shootPlayer == false && player.isAlive)
                {
                    shootPlayer = true;
                    ShootPlayer();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Enum.Tags.Player.ToString()))
            {
                playerInLight = false;
                canShootPlayer = false;
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