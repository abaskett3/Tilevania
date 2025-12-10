using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D rigidBody;
    PlayerMovement player;
    float xSpeed;
    const float DestroyTime = 1f;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }
    void FixedUpdate()
    {
        rigidBody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Destroy(collider.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        Destroy(gameObject, DestroyTime);
    }
}
