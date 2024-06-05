using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 6.0f;

    void Awake()
    {
        _movementSpeed += FindObjectOfType<Spawner>().CarSpeedMultiplier;
    }

    void Update()
    {
        gameObject.transform.position += Vector3.down * (_movementSpeed * Time.deltaTime);
    }
}
