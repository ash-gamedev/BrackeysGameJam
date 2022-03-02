using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Checkpoint : MonoBehaviour
    {
        bool checkpointReached = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Enum.Tags.Player.ToString()) && checkpointReached == false)
            {
                checkpointReached = true;
                GameManager.Instance.spawnPoint = transform.position;
            }
        }
    }
}