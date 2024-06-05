using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;
    [SerializeField] private float _initialSpawnRate = 2.0f;
    [SerializeField] private float _spawnRateDecrease = 0.05f;
    [SerializeField] private float _fastestSpawnRate = 0.3f;
    [SerializeField] private Vector2[] _spawnPoints;
    [SerializeField] private int _minLanesToSpawnCarsAtOnce = 2;
    [SerializeField] private int _maxLanesToSpawnCarsAtOnce = 3;
    private Sprite[] _cars;
    private readonly List<int> _usedSpawnPoints = new();
    [SerializeField] private float _carSpeedMultiplier = 1.0f;
    [SerializeField] private float _carSpeedIncrease = 0.075f;
    public float CarSpeedMultiplier => _carSpeedMultiplier;
    public Vector2[] SpawnPoints => _spawnPoints;
    private bool _isMaxDifficultyReached = false;
    private int _currentScore = 0;
    private DisplayScore _displayScore;
    private PlayerMovement _playerMovement;


    void Awake()
    {
        _carPrefab.transform.localScale = new Vector3(_carPrefab.transform.localScale.x, -1, _carPrefab.transform.localScale.z); // Flip car sprite to face the player controlled car
        _cars = new Sprite[FindObjectOfType<Menu>().CarSprites.Length];
        _displayScore = FindObjectOfType<DisplayScore>();
        _playerMovement = FindObjectOfType<PlayerMovement>();

        for (int i = 0; i < _cars.Length; i++)
        {
            _cars[i] = FindObjectOfType<Menu>().CarSprites[i]; // Get car sprites from Menu so they dont have to be referened in the inspector again.
        }

        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        var menu = FindObjectOfType<Menu>();
        if (!menu.IsGameStarted)
        {
            yield return new WaitUntil(() => menu.IsGameStarted);
        }

        while (true)
        {
            int amountToSpawnAtOnce = Random.Range(_minLanesToSpawnCarsAtOnce, _maxLanesToSpawnCarsAtOnce + 1);
            for (int i = 0; i < amountToSpawnAtOnce; i++)
            {
                Instantiate(_carPrefab, GetValidSpawnPoint(),
                Quaternion.identity).GetComponent<SpriteRenderer>().sprite = _cars[Random.Range(0, _cars.Length)]; // Randomly select a car sprite from the array.
            }

            _usedSpawnPoints.Clear(); // Reset used spawn points once done spawning cars.
            _currentScore += amountToSpawnAtOnce;
            _displayScore.UpdateScore(_currentScore);
            yield return new WaitForSeconds(_initialSpawnRate);
            if (_initialSpawnRate > _fastestSpawnRate)
            {
                _initialSpawnRate -= _spawnRateDecrease; // Increase spawn rate every time pair of cars are spawned.
                _carSpeedMultiplier += _carSpeedIncrease; // Also increase the car speed multiplier to make up for the increased spawn rate.
            }
            else if (_isMaxDifficultyReached == false)
            {
                Debug.Log("Max difficulty reached!");
                // If the max difficulty is reached, set the initial spawn rate to the fastest spawn rate and spawn cars at all but one lane.
                _isMaxDifficultyReached = true;
                _initialSpawnRate = _fastestSpawnRate;
                _minLanesToSpawnCarsAtOnce = _spawnPoints.Length - 1;
                if (_playerMovement != null)
                {
                    _playerMovement.MoveSpeed += 15f; // Increase player movement once max difficulty is reached to make it possible to dodge cars.
                }
            }
        }
    }

    private Vector2 GetValidSpawnPoint()
    {
        int tempIndex = Random.Range(0, _spawnPoints.Length); // Randomly select a spawn point.
        while (_usedSpawnPoints.Contains(tempIndex)) // Check if the spawn point has already been used.
        {
            tempIndex = Random.Range(0, _spawnPoints.Length); // Randomly select a spawn point again if the previous one was already used.
        }

        _usedSpawnPoints.Add(tempIndex); // Once unused spawn point is found, add it to the list of used spawn points.
        return _spawnPoints[tempIndex]; // Return the selected spawn point as a Vector2.
    }
}
