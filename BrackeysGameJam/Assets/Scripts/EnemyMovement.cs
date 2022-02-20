using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] float moveSpeed = 3f;
        [SerializeField] float walkDistance = 1;
        private Vector2 walkTarget;
        int direction = -1;

        Rigidbody2D myRigidBody;

        #region Start, Update
        void Start()
        {
            myRigidBody = GetComponent<Rigidbody2D>();

            // set the walk target
            ChangeWalkTarget();
        }

        void Update()
        {
            MoveTowards(walkTarget);
            float distanceToTarget = Vector2.Distance(transform.position, walkTarget);
            if (distanceToTarget <= 0.001f)
            {
                ChangeWalkTarget();
            }
        }
        #endregion

        #region private functions
        // when enemy reaches a wall (beyond collision boundaries)
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Enum.Tags.Platform.ToString()))
            {
                FlipSprite();
                ChangeWalkTarget();
            }
        }

        private void ChangeWalkTarget()
        {
            walkTarget = transform.position + transform.right * (walkDistance * direction);
            direction *= -1;
            FlipSprite();
        }

        private void MoveTowards(Vector2 target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }

        void FlipSprite()
        {
            transform.localScale = new Vector2(direction, 1f);
        }
        #endregion
    }
}