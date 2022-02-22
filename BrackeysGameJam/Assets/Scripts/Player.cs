using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables
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
    [SerializeField] float wallJumpTime = 0.2f;
    [SerializeField] float wallSlideSpeed = 0.001f;
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] bool isWallSliding = false;
    [SerializeField] float wallJumpBoost = 2f;
    RaycastHit2D WallCheckHit;
    float jumpTime;
    bool collidedWithWall = false;

    //items
    [Header("Change/Swallow")]
    [SerializeField] float swallowRange = 1f;
    GameObject swallowedObject;
    GameObject swallowedObjectInstance;
    SpriteRenderer playerSpriteRenderer;
    bool canChange = false;
    bool isSwallowing = false;

    [Header("Change/Swallow")]
    [SerializeField] ParticleSystem EnemyDeathParticleEffect;

    [Header("For testing:")]
    [SerializeField] bool isObject = false;

    bool isTouchingGround;
    LayerMask groundLayer;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBodyCollider;
    CapsuleCollider2D myFeetCollider2D;
    Animator myAnimator;
    Enum.PlayerAnimation playerAnimationState;

    //public object
    AudioPlayer audioPlayer;

    bool isAlive = true;

    // Private variables: to track movement
    bool isFacingRight;
    #endregion

    #region Start, Update, Awake
    void Start()
    {
        groundLayer = LayerMask.GetMask("Platform");
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        myFeetCollider2D = GetComponent<CapsuleCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Update()
    {
        if (!isAlive) return;
        Move();
        Dash();
        FlipSprite();
        SetAnimation();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        isFacingRight = moveInput.x > 0;
        isTouchingGround = myFeetCollider2D.IsTouchingLayers(groundLayer);

        //Wall Jumpy
        if (collidedWithWall)
        {
            if (isFacingRight)
            {
                WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
                Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.blue);
            }
            else
            {
                WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
                Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.blue);
            }

            // if player is trying to move toward the wall
            if (WallCheckHit && !isTouchingGround && moveInput.x != 0)
            {
                isWallSliding = true;

                // to give buffer time to wall jump
                jumpTime = Time.time + wallJumpTime;
            }
            else if (jumpTime < Time.time)
            {

                isWallSliding = false;
                collidedWithWall = false;
            }

            // Grappling wall slide, slow wall speed
            if (isWallSliding)
            {
                Debug.Log(myRigidBody.velocity);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, Mathf.Clamp(myRigidBody.velocity.y, wallSlideSpeed, float.MaxValue));
            }
        }
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

        // stop movement 
        myRigidBody.velocity = new Vector2(0, 0);

        // first make sure player sprite is enabled
        ChangeIntoPlayer();

        // play animation
        ChangeAnimationState(Enum.PlayerAnimation.Dying);
        float destroyDelay = myAnimator.GetCurrentAnimatorStateInfo(0).length;

        //play audio 
        audioPlayer.PlaySoundEffect(Enum.SoundEffects.PlayerDeath);

        FindObjectOfType<GameSession>().ProcessPlayerDeath();
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

            float direction = Mathf.Sign(transform.localScale.x);
            myRigidBody.velocity = new Vector2(direction * dashSpeed, myRigidBody.velocity.y);
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

        isSwallowing = true;
        ChangeAnimationState(Enum.PlayerAnimation.Swallowing);
        float delay = myAnimator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("SwallowComplete", delay);

        Destroy(itemObject);

        canChange = true;
    }
    
    void SwallowComplete()
    {
        isSwallowing = false;
    }

    void SetAnimation()
    {
        //if (isObject) return; 

        Enum.PlayerAnimation state;

        Vector2 playerVelocity = myRigidBody.velocity;

        bool isTouchingGround = myFeetCollider2D.IsTouchingLayers(groundLayer);
        bool isMoving = isTouchingGround && Mathf.Abs(playerVelocity.x) > Mathf.Epsilon && !isWallSliding;
        bool isJumping = !isTouchingGround && Mathf.Abs(playerVelocity.y) > Mathf.Epsilon && playerVelocity.y > 0 && !isWallSliding;
        bool isFalling = !isTouchingGround && Mathf.Abs(playerVelocity.y) > Mathf.Epsilon && playerVelocity.y <= 0 && !isWallSliding;

        if (!isSwallowing)
        {
            if (isDashing)
                state = Enum.PlayerAnimation.Dashing;
            else if (isWallSliding)
                state = Enum.PlayerAnimation.WallSliding;
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
            isTouchingGround = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask(Enum.Tags.Platform.ToString()));

            // jump while player is touching ground / or wall sliding
            if (isTouchingGround || isWallSliding)
            {
                Debug.Log("Jump - collidedWithTall : " + collidedWithWall);

                float speed = jumpSpeed + (isWallSliding ? wallJumpBoost : 0);
                myRigidBody.velocity += new Vector2(0f, speed);

                //play audio 
                audioPlayer.PlaySoundEffect(Enum.SoundEffects.PlayerJump);

                isWallSliding = false;
                collidedWithWall = false;

            }
        }
    }
    
    void OnDash(InputValue value)
    {
        if(!isDashing)
        {
            dashTime = startDashTime;
            isDashing = true;

            //play audio 
            audioPlayer.PlaySoundEffect(Enum.SoundEffects.PlayerDash);
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
        if (collision.gameObject.CompareTag(Enum.Tags.Spikes.ToString()))
        {
            Debug.Log("Death by spikes");
            Die();
        }

        if (collision.gameObject.CompareTag(Enum.Tags.Enemy.ToString()))
        {
            if (isDashing)
            {
                // Particles
                ParticleSystem instance = Instantiate(EnemyDeathParticleEffect, collision.gameObject.transform.position, Quaternion.identity);
                Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);

                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Death by collision with enemy");
                Debug.Log(collision);
                Debug.Log(isDashing);
                if(!isObject)
                    Die();
            }
        }

        else if (collision.gameObject.CompareTag(Enum.Tags.Platform.ToString()))
        {
            isTouchingGround = myFeetCollider2D.IsTouchingLayers(groundLayer);
            if (!isTouchingGround)
            {
                Debug.Log("Wall Collide");
                collidedWithWall = true;
            }
        }
    }

    #endregion

    #endregion




}
