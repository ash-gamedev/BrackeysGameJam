using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] float moveSpeed = 3f;
        [SerializeField] float walkDistance = 1;
        [SerializeField] float idlePauseTime = 1f;
        [SerializeField] int direction = -1;
        private Vector2 walkTarget;
        

        Rigidbody2D myRigidBody;
        EnemyFlashlight flashlight;
        Enum.EnemyAnimation animationState;
        Animator myAnimator;
        bool isIdle;

        #region Start, Update
        void Start()
        {
            myRigidBody = GetComponent<Rigidbody2D>();
            myAnimator = GetComponent<Animator>();
            flashlight = gameObject.transform.Find("Flashlight").GetComponent<EnemyFlashlight>();

            // set the walk target
            if(walkDistance > 0)
                ChangeWalkTarget();
        }

        void Update()
        {
            if (flashlight.GetIsKillingPlayer() == true)
            {
                ChangeAnimationState(Enum.EnemyAnimation.Idling);
                return;
            }
            if(walkDistance > 0)
            {
                if (!isIdle)
                    MoveTowards(walkTarget);

                float distanceToTarget = Vector2.Distance(transform.position, walkTarget);

                if (distanceToTarget <= 0.001f)
                {
                    ChangeWalkTarget();
                }
            }
            else
            {
                if (flashlight.GetIsFlashLightOn())
                    ChangeAnimationState(Enum.EnemyAnimation.LightOn);
                else
                    ChangeAnimationState(Enum.EnemyAnimation.LightOff);
            }
        }
        #endregion

        #region private functions
        // when enemy reaches a wall (beyond collision boundaries)
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(Enum.Tags.Platform.ToString()))
            {
                if(!isIdle) ChangeWalkTarget();
            }
        }

        private void ChangeWalkTarget()
        {
            walkTarget = transform.position + transform.right * (walkDistance * direction);
            direction *= -1;

            // set animation to idle
            isIdle = true;
            ChangeAnimationState(Enum.EnemyAnimation.Idling);
            Invoke("IdlingComplete", idlePauseTime);
        }

        void IdlingComplete()
        {
            isIdle = false;
            FlipSprite();
        }

        private void MoveTowards(Vector2 target)
        {
            // set animation to walking
            ChangeAnimationState(Enum.EnemyAnimation.Moving);

            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }

        void FlipSprite()
        {
            transform.localScale = new Vector2(direction, 1f);
        }

        void ChangeAnimationState(Enum.EnemyAnimation newState)
        {
            //stop the same animation from interuptting itself
            if (animationState == newState) return;

            //play the animation
            myAnimator.Play(newState.ToString());

            //reassign current state
            animationState = newState;
        }
        #endregion
    }
}