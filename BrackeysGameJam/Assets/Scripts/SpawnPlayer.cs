using System.Collections;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    void Start()
    {
        if(GameManager.Instance.checkPointReached != true)
            GameManager.Instance.SetSpawnPoint(transform.position);
        GameManager.Instance.SpawnPlayer();
    }

}