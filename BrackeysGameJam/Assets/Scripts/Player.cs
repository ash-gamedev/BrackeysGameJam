using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int jumpSpeed = 5;
    Vector2 moveInput;

    [Header("Wall Jump")]
    [SerializeField] float wallJumpTime = 0.2f;
    [SerializeField] float wallSlideSpeed = 0.001f;
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;

    Rigidbody2D myRigidBody;
    CircleCollider2D myBodyCollider;

    bool isAlive = true;

    #region Start, Update, Awake
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (!isAlive) return;
        Move();
        FlipSprite();
    }
    #endregion

    #region private functions
    void Move()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is techincally 0

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    #region input functions
    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed)
        {
            bool isTouchingGround = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));

            // first jump while player is touching ground
            if (isTouchingGround || isWallSliding)
            {
                myRigidBody.velocity += new Vector2(0f, jumpSpeed);
            }
        }
    }
    #endregion

    #endregion




}
