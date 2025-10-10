using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.PlayCoinSound();
            GameManager.instance.AddScore();
        }
    }
}
