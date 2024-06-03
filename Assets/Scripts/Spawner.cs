using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _spawnRate = 2.0f;
    [SerializeField] private Vector2[] _spawnPoints;

    void Start()
    {
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
            Instantiate(_prefab, _spawnPoints[Random.Range(0, _spawnPoints.Length)], Quaternion.identity);
            yield return new WaitForSeconds(_spawnRate);
        }
    }
}
