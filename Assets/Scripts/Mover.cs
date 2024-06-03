using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 6.0f;

    void Update()
    {
        gameObject.transform.position += Vector3.down * _movementSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            // Add game over logic here
        }
    }
}
