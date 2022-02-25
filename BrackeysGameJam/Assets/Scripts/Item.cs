using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject slimeObject;
    [SerializeField] public Sprite objectIcon;

    Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public GameObject GetSlimeObject()
    {
        return slimeObject;
    }

    public void Swallowed()
    {
        myAnimator.Play("Swallowed");
        float delay = myAnimator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, delay);
    }

}