using System.Collections;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private void Awake()
    {
        // only want one game session
        int numScenePersists = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}