using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float moveSpeed = -3f;

        Rigidbody2D myRigidBody;

        void Start()
        {
            myRigidBody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
        }

        // when enemy reaches a wall (beyond collision boundaries)
        void OnTriggerExit2D(Collider2D collision)
        {
            moveSpeed = -moveSpeed;
            FlipSprite();
        }

        void FlipSprite()
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}