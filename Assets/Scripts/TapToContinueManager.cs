using UnityEngine;
using TMPro;

public class TapToContinueManager : MonoBehaviour
{
    public static TapToContinueManager instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject tapToContinueUI;
    [SerializeField] private TextMeshProUGUI tapText; // kéo Text TMP vào đây

    [Header("Animation Settings")]
    [SerializeField] private float blinkSpeed = 2f; // tốc độ nhấp nháy

    public bool isWaitingForTap = false;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        tapToContinueUI.SetActive(false);

        // Tạo CanvasGroup để điều chỉnh độ trong suốt
        canvasGroup = tapToContinueUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tapToContinueUI.AddComponent<CanvasGroup>();
        }
    }

    public void ShowTapToContinue()
    {
        Time.timeScale = 0f; // Tạm dừng game
        tapToContinueUI.SetActive(true);
        isWaitingForTap = true;

        // Prevent bird from falling during tap to continue
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }
    }

    void Update()
    {
        if (isWaitingForTap)
        {
            // Hiệu ứng nhấp nháy mượt (fade in/out)
            float alpha = Mathf.PingPong(Time.unscaledTime * blinkSpeed, 1f);
            canvasGroup.alpha = alpha;

            // Nhấn chuột hoặc phím cách để tiếp tục
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                tapToContinueUI.SetActive(false);
                isWaitingForTap = false;
                Time.timeScale = 1f;
                GameManager.instance.ResumePlayerAfterRevive();

                // Make the bird jump up after resuming
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    rb.linearVelocity = Vector2.up * 2f; // Apply jump force
                }
            }
        }
    }
}
