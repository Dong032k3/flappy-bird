using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float leftBoundary = -2f;
    [SerializeField] private float speed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePipe();
    }
    private void MovePipe()
    {
        // Stop moving on game over, but allow during tap to continue
        if (GameManager.instance.isGameOver && !(TapToContinueManager.instance != null && TapToContinueManager.instance.isWaitingForTap)) return;
        transform.position += Vector3.left * Time.deltaTime * speed;
        if (transform.position.x < leftBoundary)
        {
            Destroy(gameObject);
        }
    }
}
