using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    void Awake()
    {
        _scoreText.text = "";
    }

    public void UpdateScore(int score)
    {
        if (score <= 0)
        {
            _scoreText.text = ""; // Hide score if it's less than or equal to 0
            return;
        }

        _scoreText.text = $"Score: {score}";
    }
}
