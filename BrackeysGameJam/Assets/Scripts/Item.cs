using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] GameObject slimeObject;

    public GameObject GetSlimeObject()
    {
        return slimeObject;
    }

}