using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Checkpoint : MonoBehaviour
    {
        bool checkpointReached = false;
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Enum.Tags.Player.ToString()) && checkpointReached == false)
            {
                checkpointReached = true;
                GameManager.Instance.SetSpawnPoint(transform.position);
                GameManager.Instance.checkPointReached = true;

                animator.Play("FlagRising");
            }
        }
    }
}