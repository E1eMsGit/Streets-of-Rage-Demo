using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMenu : MonoBehaviour
{
    public Image adamImage;
    public Image axelImage;
    public Image blazeImage;
    public Animator adamAnimator;
    public Animator axelAnimator;
    public Animator blazeAnimator;

    private Color _defaultColor;
    private int _characterIndex;
    private AudioSource _audioSource;

    public int CharacterIndex
    {
        get => _characterIndex;
        set
        {
            _characterIndex = value;
            if (_characterIndex < 1)
            {
                _characterIndex = 1;
            }
            else if (_characterIndex > 3)
            {
                _characterIndex = 3;
            }
        }
    }

    private void Start()
    {
        CharacterIndex = 1;
        _audioSource = GetComponent<AudioSource>();
        _defaultColor = axelImage.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CharacterIndex--;
            PlaySound();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CharacterIndex++;
            PlaySound();
                
        }

        switch (CharacterIndex)
        {
            case 1:
                adamImage.color = Color.yellow;
                adamAnimator.SetBool("Attack", true);
                axelImage.color = _defaultColor;
                axelAnimator.SetBool("Attack", false);
                blazeImage.color = _defaultColor;
                blazeAnimator.SetBool("Attack", false);
                break;
            case 2:
                axelImage.color = Color.blue;
                axelAnimator.SetBool("Attack", true);
                adamImage.color = _defaultColor;
                adamAnimator.SetBool("Attack", false);
                blazeImage.color = _defaultColor;
                blazeAnimator.SetBool("Attack", false);
                break;
            case 3:
                blazeImage.color = Color.red;
                blazeAnimator.SetBool("Attack", true);
                axelImage.color = _defaultColor;
                axelAnimator.SetBool("Attack", false);
                adamImage.color = _defaultColor;
                adamAnimator.SetBool("Attack", false);
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            FindObjectOfType<GameManager>().characterIndex = _characterIndex;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void PlaySound()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
