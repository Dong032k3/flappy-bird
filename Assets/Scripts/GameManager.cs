using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score UI")]
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text bestScoreText;

    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOverUi;

    private int score;
    private bool hasRevived = false;
    public bool isGameOver = false;
    private Animator animator;

private Vector3 playerStartPos; //  lưu vị trí ban đầu của player

public int GetCurrentScore()
{
    return score;
}

void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        score = 0;
        scoreText.text = "0";
        gameOverUi.SetActive(false);

        //  Lưu lại vị trí ban đầu của Player (theo tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStartPos = player.transform.position;
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            rb.gravityScale = 1f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void AddScore()
    {
        if (!isGameOver)
        {
            score++;
            scoreText.text = score.ToString();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        // Time.timeScale = 0f; 

        int finalScore = score;
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (finalScore > bestScore)
        {
            bestScore = finalScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        finalScoreText.text = "Score : " + finalScore;
        bestScoreText.text = "Best : " + bestScore;

        gameOverUi.SetActive(true);

        Rigidbody2D rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 2f;
    }

    // Hàm hồi sinh
    public void RevivePlayer()
    {
        if (hasRevived) return; // chỉ revive 1 lần
        hasRevived = true;
        isGameOver = false;
        gameOverUi.SetActive(false);

        // Update score UI to show current score
        scoreText.text = score.ToString();

        GameObject[] scoreZones = GameObject.FindGameObjectsWithTag("ScoreZone");
        foreach (GameObject zone in scoreZones)
        {
            Destroy(zone);
        }

        // Xóa tất cả chướng ngại vật trong scene
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        // Reset lại player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1f; // Reset to default gravity scale
        rb.constraints = RigidbodyConstraints2D.None; // Unfreeze the player to allow movement
        // Đặt lại vị trí ban đầu
        player.transform.position = playerStartPos;
        player.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation to be parallel to the ground
        TapToContinueManager.instance.ShowTapToContinue();
    }

    public void ResumePlayerAfterRevive()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Cho phép player di chuyển trở lại
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1f; // bật lại rơi

        isGameOver = false;
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnWatchAdButton()
    {
        AdManager.instance.ShowRewardedAd();
    }
    
public void OnQuitButton()
{
    Application.Quit();
}

public void OnShareButton()
{
    if (ShareManager.Instance != null)
    {
        int currentScore = GetCurrentScore();
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        ShareManager.Instance.ShareScore(currentScore, bestScore);
        Debug.Log("Sharing score: " + currentScore + ", Best: " + bestScore);
    }
    else
    {
        Debug.LogError("ShareManager.Instance is null!");
    }
}
}
