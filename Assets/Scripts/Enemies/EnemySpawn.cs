using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float minZ;
    public float maxZ;
    public GameObject[] enemy;
    public int numbersOfEnemies;
    public float spawnTime;

    private int _currentEnemies;

    private void Update()
    {
        if (_currentEnemies >= numbersOfEnemies)
        {
            int enemies = FindObjectsOfType<Enemy>().Length;
            if (enemies <= 0)
            {
                FindObjectOfType<ResetCameraScript>().Activate();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            FindObjectOfType<CameraFollow>().maxXAndY.x = transform.position.x;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        bool positionX = Random.Range(0, 2) == 0 ? true : false;
        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(minZ, maxZ);

        if (positionX)
        {
            spawnPosition = new Vector3(transform.position.x + 10, 0, spawnPosition.z);
        }
        else
        {
            spawnPosition = new Vector3(transform.position.x - 10, 0, spawnPosition.z);
        }
        Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPosition, Quaternion.identity);

        _currentEnemies++;

        if (_currentEnemies < numbersOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }
}
