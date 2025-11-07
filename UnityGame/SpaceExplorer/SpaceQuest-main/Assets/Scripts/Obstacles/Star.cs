using UnityEngine;

public class Star : MonoBehaviour
{
    // Giá trị điểm khi thu thập ngôi sao này
    public int scoreValue = 100;

    // Tốc độ di chuyển ngôi sao (ngược lại với hướng cuộn)
    // Giá trị âm để di chuyển sang trái
    [SerializeField] private float horizontalSpeed = -0.5f; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate lý tưởng cho vật lý và chuyển động
    void FixedUpdate()
    {
        // Kiểm tra nếu GameManager tồn tại
        if (GameManager.Instance != null)
        {
            // Tính toán vận tốc X dựa trên tốc độ ngang và worldSpeed
            // Đảm bảo ngôi sao di chuyển sang trái đồng bộ với tốc độ game
            float moveX = horizontalSpeed * GameManager.Instance.worldSpeed * Time.fixedDeltaTime;
            
            // Di chuyển ngôi sao sang trái
            transform.position += new Vector3(moveX, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Kiểm tra xem đối tượng va chạm có phải là người chơi (Player) không
        if (other.CompareTag("Player"))
        {
            // 2. Tăng điểm số
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }
            
            // 3. Hủy ngôi sao
            Destroy(gameObject);
        }
    }
}