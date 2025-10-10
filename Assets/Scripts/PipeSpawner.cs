using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private float spawnRate = 2f;
    private float timer = 0f;
    [SerializeField] private float minY = -1f;
    [SerializeField] private float maxY = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Cộng dồn thời gian trôi qua
        timer += Time.deltaTime;

        // Khi đủ thời gian spawnRate thì sinh pipe mới
        if (timer >= spawnRate)
        {
            SpawnPipe();
            timer = 0f; // reset lại timer
        }
    }
    private void SpawnPipe()
    {
        if (GameManager.instance.isGameOver) return;

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);
        Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
    }
}
