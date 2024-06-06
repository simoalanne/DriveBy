using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    // Awake is called before the Start method
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            GetComponent<AudioSource>().volume = 0.1f;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
