using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rigidBody;
    BoxCollider2D boxCollider;

    [SerializeField]
    float moveSpeed = 1f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        rigidBody.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        moveSpeed = -moveSpeed;
        FlipSprite();
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2(-(transform.localScale.x), transform.localScale.y);
    }
}
