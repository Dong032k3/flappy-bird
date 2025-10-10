using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
