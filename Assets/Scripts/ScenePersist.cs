using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Initializing New ScenePersist");
        //Create our singleton
        int numberScenePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (numberScenePersists > 1)
        {
            Debug.Log("Multiple ScenePersists found. Removing newly creating ScenePersist.");
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Debug.Log("Destroying ScenePersist Object.");
        Destroy(gameObject);
    }
}
