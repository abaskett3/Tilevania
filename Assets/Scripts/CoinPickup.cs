using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    AudioClip coinPickupSFX;

    [SerializeField]
    int coinScore = 100;
    PlayerMovement player;
    private bool isCollected;

    void Start()
    {
        isCollected = false;
        FindReferences();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindReferences();
    }

    void FindReferences()
    {
        player = FindFirstObjectByType<PlayerMovement>();

        if (player == null)
            Debug.LogWarning("Player not found in scene!");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("You have entered the Coin Trigger");
        Debug.Log(string.Format("Player Tag: {0}", collider.CompareTag("Player").ToString()));
        Debug.Log(string.Format("Is NOT Collected: {0}", (!IsCollected()).ToString()));
        Debug.Log(string.Format("Player is Alive : {0}", player.IsAlive()));
        if (collider.CompareTag("Player") && !IsCollected() && player.IsAlive())
        {
            PickupCoin();
        }
    }

    void PickupCoin()
    {
        Debug.Log("Picking Up Coin");
        isCollected = true;
        AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
        FindAnyObjectByType<GameSession>().IncrementScore(coinScore);
        Destroy(gameObject);
    }

    bool IsCollected()
    {
        return this.isCollected;
    }
}
