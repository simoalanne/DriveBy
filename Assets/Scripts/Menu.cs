using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private Canvas _carSelectionCanvas;
    [SerializeField] private Canvas _gameEndMenu;
    [SerializeField] private float _menuFadeOutSpeed = 0.75f;
    [SerializeField] private Sprite[] _carSprites;
    [SerializeField] private GameObject _playerCar;
    [SerializeField] private Button _previousCarButton;
    [SerializeField] private Button _nextCarButton;
    [SerializeField] private TMP_Text _carSelectionText;
    [SerializeField] private float _gameEndMenuActive = 4f;
    [SerializeField] private TMP_Text _gameEndMenuText;
    private bool _isGameStarted = false;
    public bool IsGameStarted => _isGameStarted;
    public Sprite[] CarSprites => _carSprites;
    private int _selectedCarIndex;

    void Awake()
    {
        _menuCanvas.enabled = true;
        _carSelectionCanvas.enabled = false;
        _selectedCarIndex = PlayerPrefs.GetInt("SelectedCar", (_carSprites.Length - 1) / 2); // Get player's selected car index or set to the middle of the array
        UpdateCarSelection(); // Update car sprite
        ValidateCarSelectionIndex(); // Disable corresponding button if at the end or beginning of the array
    }

    void Update()
    {
        if (_carSelectionCanvas.enabled && Input.GetKeyDown(KeyCode.Space))
        {
            DisableCarSelection();
        }

        if (_carSelectionCanvas.enabled && Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextCar();
        }

        if (_carSelectionCanvas.enabled && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousCar();
        }
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

    public void EnableCarSelection()
    {
        _menuCanvas.enabled = false;
        _carSelectionCanvas.enabled = true;
    }

    public void DisableCarSelection()
    {
        _menuCanvas.enabled = true;
        _carSelectionCanvas.enabled = false;
    }

    private IEnumerator StartGameTransition()
    {
        while (_menuCanvas.GetComponent<CanvasGroup>().alpha > 0)
        {
            _menuCanvas.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * _menuFadeOutSpeed;
            yield return null;
        }
        _menuCanvas.enabled = false;
        _isGameStarted = true;
    }

    public void SelectNextCar()
    {
        if (_previousCarButton.interactable == false)
        {
            _previousCarButton.interactable = true; // Can safely enable the button now
        }

        _selectedCarIndex++;
        ValidateCarSelectionIndex();
        UpdateCarSelection();
        PlayerPrefs.SetInt("SelectedCar", _selectedCarIndex); // Save index
    }

    public void SelectPreviousCar()
    {
        if (_nextCarButton.interactable == false)
        {
            _nextCarButton.interactable = true; // Can safely enable the button now
        }

        _selectedCarIndex--;
        ValidateCarSelectionIndex();
        UpdateCarSelection();
        PlayerPrefs.SetInt("SelectedCar", _selectedCarIndex); // Save index
    }

    private void ValidateCarSelectionIndex()
    {
        if (_selectedCarIndex == 0)
        {
            _previousCarButton.interactable = false;
        }
        else if (_selectedCarIndex >= _carSprites.Length - 1)
        {
            _nextCarButton.interactable = false;
        }
    }

    private void UpdateCarSelection()
    {
        if (_selectedCarIndex < 0)
        {
            _selectedCarIndex = 0;
            return;
        }
        else if (_selectedCarIndex >= _carSprites.Length)
        {
            _selectedCarIndex = _carSprites.Length - 1;
            return;
        }

        _playerCar.GetComponent<SpriteRenderer>().sprite = _carSprites[_selectedCarIndex];
        _carSelectionText.text = "Current Car:\n" + _carSprites[_selectedCarIndex].name;
        Debug.Log("Selected car: " + _carSprites[_selectedCarIndex].name);
        Debug.Log("Selected car index: " + _selectedCarIndex + " out of " + (_carSprites.Length - 1));
    }

    public void EndGame()
    {
        _gameEndMenu.enabled = true;
        InvokeRepeating("UpdateGameEndMenu", 0, 1.1f);
    }

    private void UpdateGameEndMenu()
    {
        _gameEndMenuActive -= 1f;
        _gameEndMenuText.text = "you crashed!\nrestarting in " + _gameEndMenuActive + " seconds";
        if (_gameEndMenuActive <= 0)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
