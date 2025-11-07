using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Cần thiết để tham chiếu đến TMP_Text
using UnityEngine.UI; // Cần thiết cho Canvas và Button

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // --- BIẾN MỚI CHO QUẢN LÝ ĐIỂM SỐ
    [Header("Score Management")]
    private int currentScore = 0;
    private int highScore = 0;
    [SerializeField] private TMP_Text scoreText; // Kéo thả TMP_Text hiển thị điểm
    
    // --- BIẾN CŨ
    [Header("Game Control")]
    public float worldSpeed;
    public int critterCounter;
    [SerializeField] private GameObject boss1;

    // Các tham chiếu UI cũ (nếu cần, hãy bỏ comment và gán trong Editor)
    // [Header("UI Menu References")]
    // public Canvas GameMenu; 


    void Awake(){
        // Khởi tạo Singleton
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        } else {
            Instance = this;
            // KÍCH HOẠT DÒNG NÀY: GIỮ GAMEMANAGER TỒN TẠI GIỮA CÁC SCENE
            DontDestroyOnLoad(gameObject); 
            // Tải High Score ngay khi Awake
            LoadHighScore(); 
        }
    }

    void Start(){
        critterCounter = 0;
        // Đặt điểm số và cập nhật hiển thị ban đầu
        currentScore = 0; 
        UpdateScoreDisplay();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3")){
            Pause();
        }

        if (critterCounter > 15){
            critterCounter = 0;
            Instantiate(boss1, new Vector2(15f, 0), Quaternion.Euler(0,0,-90));
        }
    }
    
    // --- CHỨC NĂNG MỚI: QUẢN LÝ ĐIỂM SỐ ---

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreDisplay();
    }
    
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    // --- CHỨC NĂNG MỚI: HIGH SCORE ---
    
    private void LoadHighScore()
    {
        // PlayerPrefs là cách đơn giản nhất để lưu dữ liệu local
        highScore = PlayerPrefs.GetInt("HighScore", 0); 
    }

    private void SaveHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            // Bắt buộc phải gọi Save để lưu vào đĩa
            PlayerPrefs.Save(); 
            Debug.Log("New High Score Saved: " + highScore);
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }
    
    // --- CHỨC NĂNG CŨ (Đã chỉnh sửa nhẹ) ---

    public void Pause(){
        // Sử dụng UIController.Instance (Giữ nguyên)
        if (UIController.Instance.pausePanel.activeSelf == false){
            UIController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
            AudioManager.Instance.PlaySound(AudioManager.Instance.pause);
        } else {
            UIController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
            PlayerController.Instance.ExitBoost();
            AudioManager.Instance.PlaySound(AudioManager.Instance.unpause);
        }
    }

    // ... (Giữ nguyên các hàm QuitGame, GoToMainMenu, SetWorldSpeed)

    public void GameOver(){
        // Gọi Save High Score trước khi chuyển scene
        SaveHighScore(); 
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen(){
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame(){
        // Luôn luôn lưu điểm khi thoát game
        SaveHighScore(); 
        Application.Quit();
    }

    public void SetWorldSpeed(float speed){
        worldSpeed = speed;
    }
}
