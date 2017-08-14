using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public float minZ, maxZ;
    public GameObject[] enemy;
    public int numberOfEnemies;
    public float spawnTime;

    private int currentEnemies;
    private CameraFollow cameraFollow;

	// Use this for initialization
	void Start () {
        cameraFollow = FindObjectOfType<CameraFollow>();

    }
	
	// Update is called once per frame
	void Update () {
		if (currentEnemies >= numberOfEnemies)
        {
            int enemies = FindObjectsOfType<Enemy>().Length;
            if (enemies <= 0)
            {
                cameraFollow.maxXAndY.x = 200;
                gameObject.SetActive(false);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            cameraFollow.maxXAndY.x = transform.position.x;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        bool positionX = Random.Range(0, 2) == 0 ? true : false;
        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(minZ, maxZ);
        if (positionX)
        {
            spawnPosition = new Vector3(transform.position.x + 10, 0, spawnPosition.z);
        } else
        {
            spawnPosition = new Vector3(transform.position.x - 10, 0, spawnPosition.z);
        }

        Instantiate(enemy[Random.Range(0, enemy.Length)], spawnPosition, Quaternion.identity);
        currentEnemies++;
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }
}
