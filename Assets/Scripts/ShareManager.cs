using UnityEngine;
using System.Collections;
using System.IO;
public class ShareManager : MonoBehaviour
{
    public static ShareManager Instance;

    void Awake()
    {
        // Tạo singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại khi đổi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gọi chia sẻ điểm số (chụp ảnh + share)
    /// </summary>
    public void ShareScore(int currentScore, int bestScore)
    {
        StartCoroutine(CaptureAndShare(currentScore, bestScore));
    }

    private IEnumerator CaptureAndShare(int currentScore, int bestScore)
    {
        yield return new WaitForEndOfFrame();

        // 🖼️ Chụp ảnh màn hình
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // 💾 Lưu file ảnh tạm
        string filePath = Path.Combine(Application.temporaryCachePath, "flappy_share.png");
        File.WriteAllBytes(filePath, screenshot.EncodeToPNG());

        Destroy(screenshot);

        // 📨 Gọi menu chia sẻ hệ thống
        new NativeShare()
            .AddFile(filePath)
            .SetSubject("My Flappy Bird Score!")
            .SetText($"🐤 I scored {currentScore} points! My best is {bestScore}! Can you beat me?")
            .Share();

        Debug.Log($"✅ Screenshot saved and shared: {filePath}");
    }
}
