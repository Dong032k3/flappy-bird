using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioManager audioManager;
    private bool hasDiedSoundPlayed = false;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();

        // Ensure physics is enabled
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;
        rb.gravityScale = 1f;

        // Allow animation to run during pause
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        HandleAnimation();
        HandleJump();
        HandleRotation();

    }

    private void HandleAnimation()
    {
        if (TapToContinueManager.instance != null && TapToContinueManager.instance.isWaitingForTap)
        {
            animator.speed = 1f;
        }
        else if (GameManager.instance.isGameOver)
        {
            animator.speed = 0f;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    private void HandleJump()
    {
        // Prevent jump if game is over or waiting for tap to continue
        if (GameManager.instance.isGameOver || (TapToContinueManager.instance != null && TapToContinueManager.instance.isWaitingForTap)) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            audioManager.PlayFlySound();
        }
    }

    private void HandleRotation()
    {
        if (TapToContinueManager.instance != null && TapToContinueManager.instance.isWaitingForTap)
        {
            // During tap to continue, keep bird parallel to ground
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        float angle = rb.linearVelocity.y > 0 ? 30f : -30f;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            rotationSpeed * Time.unscaledDeltaTime
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (GameManager.instance.isGameOver) return;

        if (collision.collider.CompareTag("Pipe"))
        {
            // chết khi đụng Pipe
            isFalling = true;
            GameManager.instance.GameOver();
            if (!hasDiedSoundPlayed)
            {
                audioManager.PlayDieSound();
                hasDiedSoundPlayed = true; //  lần sau không phát lại
            }
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            // chết khi chạm đất
            isFalling = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            GameManager.instance.GameOver();
            if (!hasDiedSoundPlayed)
            {
                audioManager.PlayDieSound();
                hasDiedSoundPlayed = true; //  lần sau không phát lại
            }

            // giữ nguyên vị trí khi chạm đất
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
