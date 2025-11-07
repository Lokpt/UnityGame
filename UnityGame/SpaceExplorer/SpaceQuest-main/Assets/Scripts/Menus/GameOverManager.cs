using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 
using System.Collections; 

public class GameOverManager : MonoBehaviour
{
    // Kéo thả các đối tượng TMP_Text từ Canvas vào các trường này trong Editor
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;

    void Start()
    {
        // ----------------------------------------------------
        // 1. ẨN CÁC PHẦN TỬ UI CỦA GAME CHÍNH (SCORE DISPLAY)
        //    SỬ DỤNG TAG ĐỂ TÌM KIẾM ĐÁNG TIN CẬY HƠN
        // ----------------------------------------------------
        
        // Tìm đối tượng Canvas chính của game bằng tag "GameUI" (hoặc tên Canvas nếu bạn biết)
        GameObject gameUICanvas = GameObject.FindWithTag("GameUI"); 
        
        // Nếu không tìm thấy bằng Tag, thử tìm bằng tên phổ biến
        if (gameUICanvas == null)
        {
            gameUICanvas = GameObject.Find("UI Canvas"); 
        }
        
        if (gameUICanvas != null)
        {
            // Vô hiệu hóa toàn bộ Canvas chứa UI game chính
            gameUICanvas.SetActive(false);
            Debug.Log("Game UI Canvas disabled.");
        }
        else
        {
            Debug.LogWarning("Cannot find Game UI Canvas to disable. Make sure the Canvas object has the 'GameUI' Tag in Level1 scene.");
        }
        
        // ----------------------------------------------------
        // 2. HIỂN THỊ ĐIỂM SỐ
        // ----------------------------------------------------
        
        if (GameManager.Instance != null)
        {
            int finalScore = GameManager.Instance.GetCurrentScore();
            int currentHighScore = GameManager.Instance.GetHighScore();

            if (finalScoreText != null)
            {
                finalScoreText.text = "FINAL SCORE: " + finalScore.ToString();
            }

            if (highScoreText != null)
            {
                highScoreText.text = "HIGH SCORE: " + currentHighScore.ToString();
            }

            if (finalScore == currentHighScore && currentHighScore > 0)
            {
                if (highScoreText != null) {
                    highScoreText.color = Color.yellow; 
                }
            }
        }
        else
        {
            Debug.LogError("GameManager instance not found. Cannot display scores.");
        }
    }

    public void RestartGame()
    {
        // Phục hồi lại Game UI Canvas (nếu nó được giữ lại)
        GameObject gameUICanvas = GameObject.FindWithTag("GameUI"); 
        if (gameUICanvas == null)
        {
             gameUICanvas = GameObject.Find("UI Canvas"); 
        }

        if (gameUICanvas != null)
        {
            gameUICanvas.SetActive(true);
        }
        
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Level1"); 
    }

    public void GoToMainMenu()
    {
        // ... (Giữ nguyên)
    }
}
