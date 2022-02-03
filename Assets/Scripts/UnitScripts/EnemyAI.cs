using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float spawnIntervalInSeconds = 10;
    [SerializeField] float initialSpawnDelaySeconds = 5;

    private bool canSpawn = false;
    private SpawnManager spawnManager;
    
   
    private void Awake()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }
    private void Start()
    {
        StartCoroutine(IntitialSpawnDelayCoroutine(initialSpawnDelaySeconds));
    }

    private void Update()
    {
        if (!canSpawn) return;
        StartCoroutine(SpawnIntervalCoroutine(spawnIntervalInSeconds));
        spawnManager.SpawnEnemyUnit();
    }

    IEnumerator SpawnIntervalCoroutine(float waitTime)
    {
        canSpawn = false;
        yield return new WaitForSeconds(waitTime);
        canSpawn = true;
    }

    IEnumerator IntitialSpawnDelayCoroutine(float initialSpawnDelay)
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        canSpawn = true;
    }
}
