using UnityEngine;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScores";
    private const int MaxHighScores = 3;

    private List<float> _highScores;
    public List<float> HighScores => _highScores;

    private static HighScoreManager _instance;
    public static HighScoreManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _highScores = LoadHighScores();
        _highScores.Sort((a, b) => b.CompareTo(a)); // Sort in descending order
    }

    public void CheckForHighScore(float score)
    {
        if (_highScores.Count < MaxHighScores || _highScores[_highScores.Count - 1] < score)
        {
            AddHighScore(score);
        }
    }

    private void AddHighScore(float score)
    {
        _highScores.Add(score);
        _highScores.Sort((a, b) => b.CompareTo(a)); // Sort in descending order

        // Trim the list to the top 3 scores
        if (_highScores.Count > MaxHighScores)
        {
            _highScores.RemoveAt(_highScores.Count - 1);
        }

        SaveHighScores(_highScores);
    }

    private void SaveHighScores(List<float> highScores)
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetFloat(HighScoreKey + i, highScores[i]);
        }

        PlayerPrefs.SetInt(HighScoreKey + "_Count", highScores.Count);
        PlayerPrefs.Save();
    }

    private List<float> LoadHighScores()
    {
        List<float> highScores = new List<float>();
        int count = PlayerPrefs.GetInt(HighScoreKey + "_Count", 0);

        for (int i = 0; i < count; i++)
        {
            float score = PlayerPrefs.GetFloat(HighScoreKey + i, 0);
            highScores.Add(score);
        }

        return highScores;
    }
}