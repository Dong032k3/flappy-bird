using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Material material;
    [SerializeField]
    private float parallaxFactor = 0.01f;
    private float offset;
    public float gameSpeed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        ParallaxScroll();
    }
    private void ParallaxScroll()
    {
        // Check null để tránh lỗi runtime
        if (material == null) return;

        // Stop scrolling khi game over, trừ khi đang chờ tap
        if (GameManager.instance != null && GameManager.instance.isGameOver &&
            !(TapToContinueManager.instance != null && TapToContinueManager.instance.isWaitingForTap))
            return;

        float speed = gameSpeed * parallaxFactor;
        offset += Time.unscaledDeltaTime * speed;

        // 🔹 Tự động phát hiện property tên nào hợp lệ để offset
        if (material.HasProperty("_BaseMap"))
            material.SetTextureOffset("_BaseMap", Vector2.right * offset);
        else if (material.HasProperty("_MainTex"))
            material.SetTextureOffset("_MainTex", Vector2.right * offset);
    }

}