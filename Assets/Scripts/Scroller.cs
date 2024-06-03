using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    private RawImage _image;
    [SerializeField] private float _speed = 0.2f;

    void Start()
    {
        _image = GetComponent<RawImage>();
    }

    void Update()
    {
        float newY = _image.uvRect.y + Time.deltaTime * _speed;
        newY = newY >= 1 ? 0 : newY;
        _image.uvRect = new Rect(_image.uvRect.x, newY, _image.uvRect.width, _image.uvRect.height);
    }
}