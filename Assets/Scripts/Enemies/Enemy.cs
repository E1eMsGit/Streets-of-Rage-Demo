using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxSpeed;
    public float minHeight;
    public float maxHeight;
    public float damageTime = 0.5f;
    public int maxHealth;
    public float attackRate = 1f;
    public string enemyName;
    public Sprite enemyImage;
    public AudioClip collisionSound;
    public AudioClip deathSound;

    private int _currentHealth;
    private float _currentSpeed;
    private Rigidbody _rigidbody;
    protected Animator _animator;
    private Transform _groundCheck;
    private bool _onGround;
    protected bool _isFacingRight = true;
    protected bool _isDead = false;
    private Transform _target;
    private float _zForce;
    private float _walkTimer;
    private bool _damaged;
    private float _damageTimer;
    private float _nextAttack;
    private AudioSource _audioSource;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _groundCheck = transform.Find("GroundCheck");
        _target = FindObjectOfType<Player>().transform;
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        _onGround = Physics.Linecast(transform.position, _groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _animator.SetBool("Grounded", _onGround);
        _animator.SetBool("Dead", _isDead);

        if (!_isDead)
        {
            _isFacingRight = (_target.position.x < transform.position.x) ? false : true;

            if (_isFacingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        
        if (_damaged && !_isDead)
        {
            _damageTimer += Time.deltaTime;
            if (_damageTimer >= damageTime)
            {
                _damaged = false;
                _damageTimer = 0;
            }
        }

        _walkTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!_isDead)
        {
            Vector3 targetDistance = _target.position - transform.position;
            float hForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            if (_walkTimer >= Random.Range(1f, 2f))
            {
                _zForce = Random.Range(-1, 2);
                _walkTimer = 0;
            }

            if (Mathf.Abs(targetDistance.x) < 1.5f)
            {
                hForce = 0;
            }

            if (!_damaged)
            {
                _rigidbody.velocity = new Vector3(hForce * _currentSpeed, 0, _zForce * _currentSpeed);
            }

            _animator.SetFloat("Speed", Mathf.Abs(_currentSpeed));

            if (Mathf.Abs(targetDistance.x) < 1.5f && Mathf.Abs(targetDistance.z) < 1.5f && Time.time > _nextAttack)
            {
                _animator.SetTrigger("Attack");
                _currentSpeed = 0;
                _nextAttack = Time.time + attackRate;
            }          
        }

        _rigidbody.position = new Vector3(
                _rigidbody.position.x,
                _rigidbody.position.y,
                Mathf.Clamp(_rigidbody.position.z, minHeight, maxHeight));
    }

    public void TookDamage(int damage)
    {
        if (!_isDead)
        {
            _damaged = true;
            _currentHealth -= damage;
            _animator.SetTrigger("HitDamage");
            PlaySong(collisionSound);

            FindObjectOfType<UIManager>().UpdateEnemyUI(maxHealth, _currentHealth, enemyName, enemyImage);

            if (_currentHealth <= 0)
            {
                _isDead = true;
                _rigidbody.AddRelativeForce(new Vector3(3, 5, 0), ForceMode.Impulse);
                PlaySong(deathSound);
            }
        }
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    public void PlaySong(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void ResetSpeed()
    {
        _currentSpeed = maxSpeed;
    }
}
