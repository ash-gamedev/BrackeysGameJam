using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera camera;
    bool followingPlayer = false;

    // Use this for initialization
    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (followingPlayer) return;
        Player player = FindObjectOfType<Player>();
        if(player != null)
        {
            camera.Follow = player.transform;
            followingPlayer = true;
        }
    }
}