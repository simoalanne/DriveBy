using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private float _menuFadeOutSpeed = 0.75f;
    private bool _isGameStarted = false;
    public bool IsGameStarted => _isGameStarted;

    void Awake()
    {
        _menuCanvas.enabled = true;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameTransition());
    }

    #if UNITY_EDITOR
        public void QuitGame()
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    #else
        public void QuitGame()
        {
            Application.Quit();
        }
    #endif

    private IEnumerator StartGameTransition()
    {
        while (_menuCanvas.GetComponent<CanvasGroup>().alpha > 0)
        {
            _menuCanvas.GetComponent<CanvasGroup>().alpha -= Time.unscaledDeltaTime * _menuFadeOutSpeed;
            yield return null;
        }
        _menuCanvas.enabled = false;
        _isGameStarted = true;
    }
}
