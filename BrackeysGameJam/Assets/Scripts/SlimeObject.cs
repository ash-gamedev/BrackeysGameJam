using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class SlimeObject : MonoBehaviour
    {
        [SerializeField] float fallTime = 1f;

        BoxCollider2D myCollider;
        bool isTouchingGround;

        private void Start()
        {
            myCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (isTouchingGround) return;
            FallToGround();
        }

        void FallToGround()
        {
            transform.position = transform.position + transform.up * ((-1) * fallTime * Time.deltaTime);
            isTouchingGround = myCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        }
    }
}