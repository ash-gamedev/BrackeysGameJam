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

    [Header("Dash")]
    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float startDashTime = 1f;
    float dashTime;
    bool isDashing = false;

    [Header("Wall Jump")]
    //[SerializeField] float wallJumpTime = 0.2f;
    //[SerializeField] float wallSlideSpeed = 0.001f;
    //[SerializeField] float wallDistance = 0.5f;
    [SerializeField] bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;

    //items
    [Header("Change/Swallow")]
    [SerializeField] float swallowRange = 1f;
    GameObject swallowedObject;
    GameObject swallowedObjectInstance;
    SpriteRenderer playerSpriteRenderer;
    bool canChange = false;

    [Header("For testing:")]
    [SerializeField] bool isObject = false;
    
    Rigidbody2D myRigidBody;
    CircleCollider2D myBodyCollider;
    Animator myAnimator;
    Enum.PlayerAnimation playerAnimationState;

    bool isAlive = true;
    
    #region Start, Update, Awake
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CircleCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) return;
        Move();
        Dash();
        FlipSprite();
        SetAnimation();
    }
    #endregion

    #region public functions
    public bool GetIsPlayerAnObject()
    {
        return isObject;
    }

    public void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
    #endregion

    #region private functions
    void Move()
    {
        if (isObject || isDashing) return;
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
    }

    void Dash()
    {
        if (isObject || !isDashing) return;
        if(dashTime <= 0)
        {
            isDashing = false;
            dashTime = startDashTime;
        }
        else
        {
            dashTime -= Time.deltaTime;

            myRigidBody.velocity = new Vector2(moveInput.x * dashSpeed, myRigidBody.velocity.y);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is techincally 0

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    void ChangeIntoObject()
    {
        // stop movement 
        myRigidBody.velocity = new Vector2(0, 0);

        // hide spriterenderer on player
        playerSpriteRenderer.enabled = false;

        // move object to player position show swallowed object sprite
        swallowedObjectInstance = Instantiate(swallowedObject,  // what object to instantiate
                        transform.position, // where to spawn the object
                        Quaternion.identity, // need to specify rotation
                        transform); // this will place the enemies under the Player as an child

        // update bool
        isObject = true;
    }

    void ChangeIntoPlayer()
    {
        // show spriterenderer on player
        playerSpriteRenderer.enabled = true;

        // hide swallowed object sprite
        Destroy(swallowedObjectInstance);

        // update bool
        isObject = false;
    }


    void SwallowObject(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();

        if (itemObject != null)
        {
            swallowedObject = item.GetSlimeObject();
        }

        Destroy(itemObject);

        canChange = true;
    }
    void SetAnimation()
    {
        //if (isObject) return; 

        Enum.PlayerAnimation state;

        Vector2 playerVelocity = myRigidBody.velocity;

        bool isTouchingGround = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        bool isMoving = isTouchingGround && Mathf.Abs(playerVelocity.x) > Mathf.Epsilon;
        bool isJumping = !isTouchingGround && Mathf.Abs(playerVelocity.y) > Mathf.Epsilon && playerVelocity.y > 0;
        bool isFalling = !isTouchingGround && Mathf.Abs(playerVelocity.y) > Mathf.Epsilon && playerVelocity.y <= 0 && !isWallSliding;

        if (isDashing)
            state = Enum.PlayerAnimation.Dashing;
        else if (isJumping)
            state = Enum.PlayerAnimation.Jumping;
        else if (isFalling)
            state = Enum.PlayerAnimation.Falling;
        else if (isMoving)
            state = Enum.PlayerAnimation.Moving;
        else
            state = Enum.PlayerAnimation.Idling;

        ChangeAnimationState(state);
    }

    void ChangeAnimationState(Enum.PlayerAnimation newState)
    {
        //stop the same animation from interuptting itself
        if (playerAnimationState == newState) return;

        //play the animation
        myAnimator.Play(newState.ToString());

        //reassign current state
        playerAnimationState = newState;
    }
    
    #region input functions
    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive || isObject) return;
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
    
    void OnDash(InputValue value)
    {
        if(moveInput.x != 0 && !isDashing)
        {
            dashTime = startDashTime;
            isDashing = true;
        }
    }

    void OnChangeIntoObject(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed)
        {
            // if not an object, and an object has been swallowed, change into an object
            if (!isObject && canChange)
            {
                ChangeIntoObject();
            }
            // otherwise, change back to normal form
            else
            {
                ChangeIntoPlayer();
            }
        }
    }
    
    void OnSwallowObject(InputValue value)
    {
        if (!isAlive || isObject) return;
        if (value.isPressed)
        {
            var itemsToSwallow = Physics2D.OverlapCircleAll(transform.position, swallowRange);

            float minItemDistance = float.MaxValue;
            GameObject closestItem = null;

            //find the closest item to player
            foreach (var item in itemsToSwallow)
            {
                if (item.CompareTag(Enum.Tags.Item.ToString()))
                {
                    float distanceToPlayer = Vector2.Distance(transform.position, item.transform.position);
                    if (distanceToPlayer < minItemDistance)
                    {
                        minItemDistance = distanceToPlayer;
                        closestItem = item.gameObject;
                    }
                }
            }

            if(closestItem != null)
            {
                SwallowObject(closestItem);
            }
        }
    }
    #endregion

    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Enum.Tags.Enemy.ToString()))
        {
            if (isDashing)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Die();
            }
        }
    }
    #endregion

    #endregion




}
