using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    [SerializeField] private int waveNumber;
    [SerializeField] private List<Wave> waves;

    [System.Serializable]
    public class Wave {
        public GameObject prefab;
        public float spawnTimer;
        public float spawnInterval;
        public int objectsPerWave;
        public int spawnedObjectCount;
    }

    void Update()
    {
        // ... (Giữ nguyên logic Wave Management)
        if (GameManager.Instance == null) return;
        
        waves[waveNumber].spawnTimer -= Time.deltaTime * GameManager.Instance.worldSpeed;
        if (waves[waveNumber].spawnTimer <= 0){
            waves[waveNumber].spawnTimer += waves[waveNumber].spawnInterval;
            SpawnObject();
        }
        if (waves[waveNumber].spawnedObjectCount >= waves[waveNumber].objectsPerWave){
            waves[waveNumber].spawnedObjectCount = 0;
            waveNumber++;
            if (waveNumber >= waves.Count){
                waveNumber = 0;
            }
        }
    }

    private void SpawnObject(){
        Vector2 spawnPos = RandomSpawnPoint();
        
        // 1. Tạo vật thể
        GameObject newObject = Instantiate(waves[waveNumber].prefab, spawnPos, transform.rotation, transform);
        
        // 2. Thiết lập vận tốc dựa trên loại Rigidbody
        Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
        
        // KIỂM TRA QUAN TRỌNG: Chỉ thiết lập vận tốc nếu Rigidbody không phải là Kinematic
        // (Tức là chỉ thiên thạch/kẻ địch)
        if (rb != null && rb.bodyType != RigidbodyType2D.Kinematic)
        {
            // Logic vận tốc cho thiên thạch (luôn bay vào trung tâm)
            Vector2 targetDirection = (Vector2.zero - spawnPos).normalized;
            
            // Đã tăng tốc độ để đảm bảo chúng bay vào màn hình
            float baseSpeed = Random.Range(3.5f, 6.0f); 
            rb.linearVelocity = targetDirection * baseSpeed;
            
            // Xoay ngẫu nhiên
            rb.angularVelocity = Random.Range(-90f, 90f); 
        } 
        // Nếu đối tượng là Star (có Body Type = Kinematic), code sẽ bỏ qua phần này 
        // và ngôi sao sẽ đứng yên tại vị trí spawnPos.

        waves[waveNumber].spawnedObjectCount++;
    }

    /// <summary>
    /// Trả về một vị trí ngẫu nhiên ở 4 cạnh (trên, dưới, trái, phải) của khu vực minPos/maxPos.
    /// </summary>
    private Vector2 RandomSpawnPoint(){
        Vector2 spawnPoint = Vector2.zero;
        
        int side = Random.Range(0, 4); 

        float minX = minPos.position.x;
        float maxX = maxPos.position.x;
        float minY = minPos.position.y;
        float maxY = maxPos.position.y;

        switch (side)
        {
            case 0: // Cạnh Trái
                spawnPoint.x = minX;
                spawnPoint.y = Random.Range(minY, maxY);
                break;
            case 1: // Cạnh Phải
                spawnPoint.x = maxX;
                spawnPoint.y = Random.Range(minY, maxY);
                break;
            case 2: // Cạnh Dưới
                spawnPoint.x = Random.Range(minX, maxX);
                spawnPoint.y = minY;
                break;
            case 3: // Cạnh Trên
                spawnPoint.x = Random.Range(minX, maxX);
                spawnPoint.y = maxY;
                break;
        }

        return spawnPoint;
    }
}