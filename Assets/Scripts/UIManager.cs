using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthUI;
    public Image playerImage;
    public Text playerName;
    public Text livesText;
    public Text displayMessage;

    public GameObject enemyUI;
    public Slider enemySlider;
    public Image enemyImage;
    public Text enemyName;

    public float enemyUITimer = 4f;

    private float enemyTimer;
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        healthUI.maxValue = _player.maxHealth;
        healthUI.value = healthUI.maxValue;
        playerName.text = _player.playerName;
        playerImage.sprite = _player.playerImage;
        UpdateLives();
    }

    private void Update()
    {
        enemyTimer += Time.deltaTime;

        if (enemyTimer >= enemyUITimer)
        {
            enemyUI.SetActive(false);
            enemyTimer = 0;
        }
    }

    public void UpdateHealth(int amount)
    {
        healthUI.value = amount;
    }

    public void UpdateEnemyUI(int maxHealth, int currentHealth, string name, Sprite image)
    {
        enemySlider.maxValue = maxHealth;
        enemySlider.value = currentHealth;
        enemyName.text = name;
        enemyImage.sprite = image;
        enemyTimer = 0;
        enemyUI.SetActive(true);
    }

    public void UpdateLives()
    {
        livesText.text = $"x {FindObjectOfType<GameManager>().lives}";
    }

    public void UpdateDisplayMessage(string message)
    {
        displayMessage.text = message;
    }
}
