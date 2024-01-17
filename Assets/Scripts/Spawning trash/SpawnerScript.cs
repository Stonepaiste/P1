using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawning : MonoBehaviour
{
    public GameObject[] trash;              //Liste med trash objekter
    public Vector3 spawnValues;             //Område hvor trash kan spawne
    public float spawnMostWait;
    public float spawnLeastWait;

    private void Start()
    {
        StartCoroutine(StartTrash());           
    }

    // Start is called before the first frame update
    IEnumerator StartTrash()
    {
        while (true)
        {
            SpawnTrash();       //Kalder spawntrash methoden
            yield return new WaitForSeconds(Random.Range(spawnLeastWait, spawnMostWait));           //Venter random antal sekunder
        }
    }

    private void SpawnTrash()
    {
        int randTrash = Random.Range(0, trash.Length);      //Vælger et tilfædligt stykke skrald
        //LAver en random spawnposition
        Vector3 randomSpawnPosition = new Vector3(Random.Range(transform.position.x -spawnValues.x,transform.position.x + spawnValues.x), transform.position.y + spawnValues.y, 1);
        Instantiate(trash[randTrash], randomSpawnPosition, Quaternion.identity);     //Spawner en et stykke skrald på den random position
    }
}