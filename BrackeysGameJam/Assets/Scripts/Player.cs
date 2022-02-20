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

    //items
    [Header("Change/Swallow")]
    [SerializeField] GameObject swallowedObject;
    [SerializeField] float swallowRange = 1f;
    SpriteRenderer playerSpriteRenderer;
    SpriteRenderer objectSpriteRenderer;
    bool canChange = false;

    [Header("For testing:")]
    [SerializeField] bool isObject = false;

    Rigidbody2D myRigidBody;
    CircleCollider2D myBodyCollider;

    bool isAlive = true;
    
    #region Start, Update, Awake
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CircleCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        objectSpriteRenderer = swallowedObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isAlive) return;
        Move();
        FlipSprite();
    }
    #endregion

    #region public functions
    public bool GetIsPlayerAnObject()
    {
        return isObject;
    }
    #endregion

    #region private functions
    void Move()
    {
        if (isObject) return;
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

    void ChangeIntoObject()
    {
        // stop movement 
        myRigidBody.velocity = new Vector2(0, 0);

        // hide spriterenderer on player
        playerSpriteRenderer.enabled = false;

        // show swallowed object sprite
        objectSpriteRenderer.enabled = true;

        // update bool
        isObject = true;
    }

    void ChangeIntoPlayer()
    {
        // show spriterenderer on player
        playerSpriteRenderer.enabled = true;

        // hide swallowed object sprite
        objectSpriteRenderer.enabled = false;

        // update bool
        isObject = false;
    }

    void SwallowObject(GameObject item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;

        if (sprite != null)
            objectSpriteRenderer.sprite = sprite;

        Destroy(item);

        canChange = true;
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
    
    void OnChangeIntoObject(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed)
        {
            // if not an object, and an object has been swallowed, change into an object
            if (!isObject && canChange)
            {
                Debug.Log("Changing");
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
            Debug.Log("Swallowing");
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

    #endregion




}
