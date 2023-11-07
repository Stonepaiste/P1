using System.Collections;
using UnityEngine;

public class spawning : MonoBehaviour
{
    public GameObject[] trash;
    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;

    int randTrash;

    // Start is called before the first frame update
    private IEnumerator Start
    {
        get
        {
            yield return new WaitForSeconds(Random.Range(0, spawnWait));

            while (true)
            {
                SpawnTrash();
                yield return new WaitForSeconds(Random.Range(spawnLeastWait, spawnMostWait));
            }
        }
    }

    private void SpawnTrash()
    {
        int randTrash = Random.Range(0, trash.Length);
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));
        Instantiate(trash[randTrash], randomSpawnPosition, Quaternion.identity);
    }
}