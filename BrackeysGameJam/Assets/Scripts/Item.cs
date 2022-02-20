using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject slimeObject;

    BoxCollider2D myCollider;
    bool isTouchingGround;

    public GameObject GetSlimeObject()
    {
        return slimeObject;
    }

    private void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isTouchingGround) return;
    }

    void FallToGround()
    {
        isTouchingGround = myCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));

    }
}