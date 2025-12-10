using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rigidbody;
    CapsuleCollider2D capsuleCollider;
    BoxCollider2D boxCollider;
    Animator animator;

    [SerializeField]
    float runSpeed = 5f;

    [SerializeField]
    float jumpHeight = 8f;

    [SerializeField]
    float climbSpeed = 3f;

    [SerializeField]
    Vector2 deathKick = new Vector2(10f, 20f);

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Transform gun;

    private float startingGravityScale;
    private float startingAnimationSpeed;
    private bool isAlive;

    void Start()
    {
        Debug.Log("A New Player has been Created.");
        isAlive = true;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        SetStartingGravityScale(rigidbody.gravityScale);
        SetStartingAnimationSpeed(animator.speed);
    }

    void FixedUpdate()
    {
        if (!IsAlive())
        {
            return;
        }

        Run();
        FlipSprite();
        ClimbLadder();

        if (IsTouchingEnemies() || IsTouchingHazards())
        {
            Die();
        }
    }

    void OnMove(InputValue value)
    {
        if (!IsAlive())
        {
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rigidbody.linearVelocity.y);
        rigidbody.linearVelocity = playerVelocity;
        animator.SetBool("isRunning", HasHorizontalSpeed());
    }

    void ClimbLadder()
    {
        if (IsTouchingLadders())
        {
            rigidbody.gravityScale = 0f;
            Vector2 climbVelocity = new Vector2(
                rigidbody.linearVelocity.x,
                moveInput.y * climbSpeed
            );
            rigidbody.linearVelocity = climbVelocity;
            animator.SetBool("isClimbing", HasVerticalSpeed());
        }
        else
        {
            rigidbody.gravityScale = GetStartingGravityScale();
            animator.SetBool("isClimbing", false);
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && (IsTouchingGround() || IsTouchingLadders()))
        {
            rigidbody.linearVelocity += new Vector2(0f, jumpHeight);
        }
    }

    void OnAttack(InputValue value)
    {
        if (IsAlive())
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    void FlipSprite()
    {
        if (HasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.linearVelocity.x), 1f);
        }
    }

    bool HasHorizontalSpeed()
    {
        return Mathf.Abs(rigidbody.linearVelocity.x) > Mathf.Epsilon;
    }

    bool HasVerticalSpeed()
    {
        return Mathf.Abs(rigidbody.linearVelocity.y) > Mathf.Epsilon;
    }

    bool IsTouchingLadders()
    {
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }

    bool IsTouchingGround()
    {
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    bool IsTouchingEnemies()
    {
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Enemies"));
    }

    bool IsTouchingHazards()
    {
        return boxCollider.IsTouchingLayers(LayerMask.GetMask("Hazards"));
    }

    void SetStartingGravityScale(float gravityScale)
    {
        this.startingGravityScale = gravityScale;
    }

    float GetStartingGravityScale()
    {
        return this.startingGravityScale;
    }

    void SetStartingAnimationSpeed(float animationSpeed)
    {
        this.startingAnimationSpeed = animationSpeed;
    }

    float GetStartingAnimationSpeed()
    {
        return this.startingAnimationSpeed;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    void Die()
    {
        isAlive = false;
        DeathKick();
        FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
    }

    private void DeathKick()
    {
        animator.SetTrigger("Dying");
        rigidbody.linearVelocity = deathKick;
    }
}
