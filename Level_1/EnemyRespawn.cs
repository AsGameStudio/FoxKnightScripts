using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform RespawnZone;
    public int CountRespawn = 1;
    public float RespawnDelay = 1.0f;
    public LayerMask EnemyLayer;

    public List<GameObject> enemyPool = new List<GameObject>();

    private void Start()
    {
        EnemyPool();
        StartCoroutine(SpawnEnemy());
    }


    private void EnemyPool()
    {
        for(int i = 0; i < CountRespawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }
    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < CountRespawn; i++)
        {
            GameObject enemy = GetNextEnemy();
            if (enemy != null)
            {
                Vector3 RandomPosition = GetRandomSpawnPosition();

                enemy.transform.position = RandomPosition;
                enemy.SetActive(true);
            }
            yield return new WaitForSeconds(RespawnDelay);
        }
    }

    GameObject GetNextEnemy()
    {

        foreach(GameObject enemy in enemyPool)
        {
            if(!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        return null;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = RespawnZone.position;
        Vector3 size = RespawnZone.localScale;

        float RandomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float RandomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        Vector3 RandomPosition = new Vector3(RandomX, center.y, RandomZ);

        return RandomPosition;
    }
}
