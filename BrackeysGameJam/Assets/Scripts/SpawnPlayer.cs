using System.Collections;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameManager.Instance.SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }
}