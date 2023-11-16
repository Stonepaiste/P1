using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawning : MonoBehaviour
{
    public GameObject[] trash;
    public Vector3 spawnValues;
    public float spawnMostWait;
    public float spawnLeastWait;

    int randTrash;
    private void Start()
    {
        StartCoroutine(StartTrash());
    }

    // Start is called before the first frame update
    IEnumerator StartTrash()
    {
            while (true)
            {
                SpawnTrash();
                yield return new WaitForSeconds(Random.Range(spawnLeastWait, spawnMostWait));
            }
        
    }

    private void SpawnTrash()
    {
        int randTrash = Random.Range(0, trash.Length);
        Vector3 randomSpawnPosition = new Vector3(Random.Range(transform.position.x -spawnValues.x,transform.position.x + spawnValues.x), transform.position.y + spawnValues.y, 1);
        Instantiate(trash[randTrash], randomSpawnPosition, Quaternion.identity);
    }
}