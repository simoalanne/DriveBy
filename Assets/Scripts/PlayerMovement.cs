using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private int _currentIndex = 1;
    private const int _maxIndex = 3;
    private readonly float _amountToMove = 3f;
    [SerializeField] private float _moveSpeed = 22.5f; // Adjust this value to control the movement speed
    private Vector3 _targetPosition;
    private bool _isMoving = false;

    void Start()
    {
        _targetPosition = transform.position;
    }

    void Update()
    {
        if (!_isMoving)
        {
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && _currentIndex < _maxIndex && FindObjectOfType<Menu>().IsGameStarted)
            {
                _currentIndex++;
                _targetPosition += Vector3.right * _amountToMove;
                Debug.Log(_currentIndex);
                StartCoroutine(MoveToTarget());
            }
            else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && _currentIndex > 0 && FindObjectOfType<Menu>().IsGameStarted)
            {
                _currentIndex--;
                _targetPosition += Vector3.left * _amountToMove;
                Debug.Log(_currentIndex);
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
}
