using System.Collections;
using UnityEngine;
public class LevelComplete : MonoBehaviour
{
    UIManager uIManager;
    [SerializeField] float countDelay = 0.1f;

    public void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
    }
    
    
}