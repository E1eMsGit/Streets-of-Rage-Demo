using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight;
    public float maxHeight;
    public int maxHealth = 10;
    public string playerName;
    public Sprite playerImage;
    public AudioClip collisionSound;
    public AudioClip jumpSound;
    public AudioClip healthItemSound;
    public Weapon weapon;

    private int _currentHealth;
    private float _currentSpeed;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Transform _groundCheck;
    private bool _onGround;
    private bool _isDead = false;
    private bool _isFacingRight = true;
    private bool _isJump = false;
    private AudioSource _audioSource;
    private bool _holdingWeapon = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _groundCheck = transform.Find("GroundCheck");
        _currentSpeed = maxSpeed;
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        _onGround = Physics.Linecast(transform.position, _groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _animator.SetBool("OnGround", _onGround);
        _animator.SetBool("Dead", _isDead);
        _animator.SetBool("Weapon", _holdingWeapon);

        if (Input.GetButtonDown("Jump") && _onGround)
        {
            _isJump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        if (!_isDead)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (!_onGround)
            {
                z = 0;
            }

            _rigidbody.velocity = new Vector3(x * _currentSpeed, _rigidbody.velocity.y, z * _currentSpeed);

            if (_onGround)
            {
                _animator.SetFloat("Speed", Mathf.Abs(_rigidbody.velocity.magnitude));
            }
            
            if (x > 0 && !_isFacingRight)
            {
                Flip();
            }
            else if (x < 0 && _isFacingRight)
            {
                Flip();
            }

            if (_isJump)
            {
                _isJump = false;
                _rigidbody.AddForce(Vector3.up * jumpForce);
                PlaySong(jumpSound);
            }

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            _rigidbody.position = new Vector3(
                Mathf.Clamp(_rigidbody.position.x, minWidth + 1, maxWidth - 1),
                _rigidbody.position.y, 
                Mathf.Clamp(_rigidbody.position.z, minHeight, maxHeight));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Health Item"))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Destroy(other.gameObject);
                _animator.SetTrigger("Catching");
                PlaySong(healthItemSound);
                _currentHealth = maxHealth;
                FindObjectOfType<UIManager>().UpdateHealth(_currentHealth);
            }
        }

        if (other.CompareTag("Weapon"))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                _animator.SetTrigger("Catching");
                _holdingWeapon = true;
                WeaponItem weaponItem = other.GetComponent<PickableWeapon>().weapon;
                weapon.ActivateWeapon(weaponItem.sprite, weaponItem.color, weaponItem.durability, weaponItem.damage);
                Destroy(other.gameObject);                
            }
        }
    }

    public void TookDamage(int damage)
    {
        if (!_isDead)
        {
            _currentHealth -= damage;
            _animator.SetTrigger("HitDamage");
            FindObjectOfType<UIManager>().UpdateHealth(_currentHealth);
            PlaySong(collisionSound);

            if (_currentHealth <= 0)
            {
                _isDead = true;
                FindObjectOfType<GameManager>().lives--;

                if (_isFacingRight)
                {
                    _rigidbody.AddForce(new Vector3(-3, 5, 0), ForceMode.Impulse);
                }
                else
                {
                    _rigidbody.AddForce(new Vector3(3, 5, 0), ForceMode.Impulse);
                }
            }
        }
    }

    public void PlaySong(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void SetHoldingWeaponToFalse()
    {
        _holdingWeapon = false;
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ZeroSpeed()
    {
        _currentSpeed = 0;
    }

    private void ResetSpeed()
    {
        _currentSpeed = maxSpeed;
    }

    private void PlayerRespawn()
    {
        if (FindObjectOfType<GameManager>().lives > 0)
        {
            _isDead = false;
            FindObjectOfType<UIManager>().UpdateLives();
            _currentHealth = maxHealth;
            FindObjectOfType<UIManager>().UpdateHealth(_currentHealth);
            _animator.Rebind();
            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            transform.position = new Vector3(minWidth, 10, -4);
        }
        else
        {
            FindObjectOfType<UIManager>().UpdateDisplayMessage("Game Over");
            Destroy(FindObjectOfType<GameManager>().gameObject);
            Invoke("LoadScene", 2f);
        }
    }   
}
