using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private float _spawnTimer = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _asteroidPrefab;
    private bool _asteroidAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        Asteroid();
        
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnTimer);
        }
    }
    
    IEnumerator SpawnTripleShot()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            int randomPU = Random.Range(0, 3);
            Instantiate (_powerups[randomPU], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }
    public void OnplayerDeath()
    {
        _stopSpawning = true;
    }
    public void Asteroid()
    {
            Vector3 posToSpawn = new Vector3(0, 4, 0);
            GameObject asteroid = Instantiate(_asteroidPrefab, posToSpawn, Quaternion.identity);
    }
    public void AsteroidDeath()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnTripleShot());
    }
}
