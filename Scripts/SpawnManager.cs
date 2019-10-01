using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _asteroidPrefab;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _powerupContainer;

    [SerializeField]
    private float _spawnEnemyDelay = 2.5f;

    [SerializeField]
    private float _spawnAsteroidDelay = 15.0f;

    [SerializeField]
    private float _spawnPowerupTripleShotDelay = 3.5f;

    [SerializeField]
    private float _spawnRandomRangeFactor = 0.5f;

    [SerializeField]
    private bool _enemySpawner = false;

    [SerializeField]
    private bool _powerupSpawner = false;

    [SerializeField]
    private bool _asteroidSpawner = false;

    [SerializeField]
    private bool _waveCounter = false;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {


        _player = GameObject.Find("Player").GetComponent<Player>();
        StartSpawning();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.getLives() == 0)
        {
            _enemySpawner = false;
            _powerupSpawner = false;
            _asteroidSpawner = false;
            _waveCounter = false;
        }
    }

    public void StartSpawning()
    {
        _waveCounter = true;
        StartCoroutine(WaveCountdown());
        StartCoroutine(SpawnDelay());
    }

    private void BeginSpawning()
    {
        _enemySpawner = true;
        _powerupSpawner = true;
        _asteroidSpawner = true;

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnAsteroidRoutine());
        StartCoroutine(SpawnPowerupRoutine());        
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(3.0f);
        BeginSpawning();
    }

    IEnumerator SpawnPowerupRoutine()
    {
        bool _spawning = false;
        while (_powerupSpawner == true)
        {
            if (_spawning == true)
            {
                GameObject newPowerup = Instantiate(_powerups[Random.Range(0, _powerups.Length)], new Vector3(Random.Range(-9.3f, 9.3f), 6.2f, 0), Quaternion.identity);
                newPowerup.transform.parent = _powerupContainer.transform;                              
            }
            _spawning = true;
            yield return new WaitForSeconds(Random.Range(_spawnPowerupTripleShotDelay * _spawnRandomRangeFactor, _spawnPowerupTripleShotDelay * _spawnRandomRangeFactor));
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_enemySpawner == true)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9.3f, 9.3f), 6.2f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnEnemyDelay / _player.getWave());
        }
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while (_asteroidSpawner == true)
        {
            GameObject newAsteroid = Instantiate(_asteroidPrefab, new Vector3(Random.Range(-9.3f, 14.3f), 6.2f, 0), Quaternion.identity);
            newAsteroid.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_spawnAsteroidDelay / _player.getWave());
        }
    }

    IEnumerator WaveCountdown()
    {
        while (_waveCounter == true)
        {
            Debug.Log("Count Waves!");
            _player.setWave(_player.getWave() + 1);
            yield return new WaitForSeconds(15.0f);
        }
    }

    public void StopEnemySpawning()
    {
        _enemySpawner = false;
    }

    public void StopAsteroidSpawning()
    {
        _asteroidSpawner = false;
    }

    public void StopWaves()
    {
        _waveCounter = false;
    }

    public void StopAllSpawning()
    {
        StopEnemySpawning();
        StopAsteroidSpawning();
        StopWaves();
    }
}
