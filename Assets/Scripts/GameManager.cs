using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lives;
    public int characterIndex;

    private static GameManager _gameManager;

    private void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = this;
        }
        else if (_gameManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
