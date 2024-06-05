using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private int _currentIndex = 1;
    private const int _maxIndex = 3;
    private readonly float _amountToMove = 3f;
    [SerializeField] private float _moveSpeed = 22.5f; // Adjust this value to control the movement speed
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private Menu _menu;

    void Awake()
    {
        _targetPosition = transform.position;
        _menu = FindObjectOfType<Menu>();
    }

    void Update()
    {
        if (!_isMoving)
        {
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && _currentIndex < _maxIndex && _menu.IsGameStarted)
            {
                _currentIndex++;
                _targetPosition += Vector3.right * _amountToMove;
                StartCoroutine(MoveToTarget());
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && _currentIndex > 0 && _menu.IsGameStarted)
            {
                _currentIndex--;
                _targetPosition += Vector3.left * _amountToMove;
                StartCoroutine(MoveToTarget());
            }
        }
    }

    private IEnumerator MoveToTarget()
    {
        _isMoving = true;
        while (Vector3.Distance(transform.position, _targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _moveSpeed);
            yield return null;
        }
        transform.position = _targetPosition; // Ensure the player exactly reaches the target position
        _isMoving = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Car"))
        {
            Destroy(gameObject);
            FindObjectOfType<Menu>().EndGame();
        }
    }
}
