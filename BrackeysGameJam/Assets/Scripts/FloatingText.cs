using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        Animator animator;

        // Use this for initialization
        void Start()
        {
            text.enabled = false;
            Renderer renderer = this.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = 100;
            }

            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            text.enabled = true;
            animator.Play("FloatingText");
        }
    }
}