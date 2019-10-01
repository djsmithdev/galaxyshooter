using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _laserTriplePrefab;
    [SerializeField]
    private GameObject _ProjectileContainer;
    [SerializeField]
    private float _fireRate = 0.25f;
    [SerializeField]
    private GameObject _imageShield;

    [SerializeField]
    private GameObject _thruster;

    [SerializeField]
    private GameObject[] _fire;

    [SerializeField]
    private int _score;

    private SpawnManager _spawnManager;
    private bool _tripleShot = false;
    private bool _speedBoost = false;
    private bool _shieldsUp = false;

    [SerializeField]
    private float _speedBoostMultiplier = 1.85f;

    private int _lives = 3;
    private int _health = 100;
    private int _wave = 0;
    private bool _isAlive = true;

    private float _nextFire = 0.0f;

    [SerializeField]
    private AudioClip _explosionAudioClip;

    private GameObject _player;


    private AudioSource _audioSource;
 
    public Bounds playerBoundary = new Bounds(6.0f, 9.3f, -4f, -9.3f);

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _player = GameObject.Find("Player");
        ResetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive == true)
        {
            DisplayFires();
            CalculateMovement();
            CheckInput();
        }
    }

    public int getWave()
    {
        return _wave;
    }
    
    public void setWave(int wave)
    {
        _wave = wave;
    }

    private void DisplayFires()
    {
        if (_health < 100)
        {
            // show first fire
            _fire[0].SetActive(true);
        }
        else
        {
            // disable first fire
            _fire[0].SetActive(false);
        }

        if (_health < 66)
        {
            // show second fire
            _fire[1].SetActive(true);
        }
        else
        {
            // disable second fire
            _fire[1].SetActive(false);
        }

        if (_health < 33)
        {
            // show third fire
            _fire[2].SetActive(true);
        }
        else
        {
            // disable third fire
            _fire[2].SetActive(false);
        }

        
    }

    void ResetPlayer()
    {
        _isAlive = true;
        _health = 100;
        _animator.ResetTrigger("OnPlayerDeath");
        _animator.SetTrigger("OnPlayerReset");

        this.GetComponent<BoxCollider2D>().enabled = true;

        // zero the player position
        transform.position = new Vector3(0, -2.3f, 0);
    }

    void CheckInput()
    {
        // if space key is pressed
        bool firing = Input.GetButton("Fire1");

        if (firing && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            FireOne();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // if moving vertically upward, fire thruster
        if (verticalInput > 0)
        {
            _thruster.SetActive(true);
        } else
        {
            _thruster.SetActive(false);
        }

        if (horizontalInput > 0)
        { 
            _animator.SetBool("isMovingRight", true);
        }

        else if (horizontalInput < 0) {
            _animator.SetBool("isMovingLeft", true);
        }
        else
        {
            _animator.SetBool("isMovingLeft", false);
            _animator.SetBool("isMovingRight", false);

        }

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x,
                                                playerBoundary.Left,
                                                playerBoundary.Right),
                                         Mathf.Clamp(transform.position.y,
                                                playerBoundary.Bottom, 
                                                playerBoundary.Top),
                                         transform.position.z);

       if (transform.position.x <= playerBoundary.Left)
       {
            transform.position = new Vector3(playerBoundary.Right, transform.position.y, transform.position.z);        
       }
       else if (transform.position.x >= playerBoundary.Right)
       {
            transform.position = new Vector3(playerBoundary.Left, transform.position.y, transform.position.z);
       }

       float _finalSpeed = _speed;
       if (_speedBoost)
       {
        _finalSpeed = _speed * _speedBoostMultiplier;
       }

       transform.Translate(direction * _finalSpeed * Time.deltaTime);       
    }

    void FireOne()
    {
        // if tripleshot is active, fire three
        // else fire a single shot       

        if (_tripleShot == true)
        {
            FireTripleShot();
        }
        else
        {
            FireSingleShot();
        }

        // play sound
        _audioSource.PlayOneShot(_laserAudioClip);
    }

    void FireSingleShot()
    {
        FireProjectile(_laserPrefab);
    }

    void FireTripleShot()
    {
        FireProjectile(_laserTriplePrefab);
    }



    void FireProjectile(GameObject projectilePrefab)
    {

        

        Vector3 newLocation = new Vector3(transform.position.x, transform.position.y + 1.0f + transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab, newLocation, Quaternion.identity);


        Laser[] _lasers = projectile.gameObject.GetComponentsInChildren<Laser>();
        foreach (Laser laser in _lasers)
        {
            laser.setDirection(Vector3.up);
        }

        projectile.transform.parent = _ProjectileContainer.transform;
    }


    public void Damage(int value)
    {
        if (_shieldsUp != true)
        {
            _health = _health - value;
            Debug.Log("Player Damaged. Health: " + _health);
            if (_health <= 0)
            {
                _animator.SetTrigger("OnPlayerDeath");
                _animator.ResetTrigger("OnPlayerReset");
                Debug.Log("Player Died.");

                // reset player position
                DestroyShip();
                StartCoroutine(DelayReset(4.0f));

                _isAlive = false;
                          
                _lives--;  
                
                if (_lives < 0)
                {
                    _lives = 0;
                }
                
                if (_lives > 0)
                {
                    StartCoroutine(DelayReset(4.0f));
                }
            }
        }
    }


    public void EnableTripleShot(float value)
    {
        _tripleShot = true;
        StartCoroutine(StartTripleShot(value)); // start coroutine
    }


    public void EnableSpeedBoost(float value)
    {
        _speedBoost = true;
        StartCoroutine(StartSpeedBoost(value)); // start coroutine
    }

    public void EnableShields(float value)
    {
        _shieldsUp = true;
        _imageShield.SetActive(true);
        StartCoroutine(StartShieldsUp(value)); // start coroutine
    }

    public void addScore(int value)
    {
        _score = _score + value;
    }

    public int getScore()
    {
        return _score;
    }

    public int getLives()
    {
        return _lives;
    }

    public int getHealth()
    {
        return _health;
    }

    private void DestroyShip()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;

        _animator.SetTrigger("OnPlayerDeath");
        _animator.ResetTrigger("OnPlayerReset");
        _fire[0].SetActive(false);
        _fire[1].SetActive(false);
        _fire[2].SetActive(false);
        _thruster.SetActive(false);

        // play explosion sound
        _audioSource.clip = _explosionAudioClip;
        _audioSource.Play();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAlive == true)
        {
            if (other.tag == "Laser")
            {
                Damage(10);
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator StartShieldsUp(float timerLength)
    {
        yield return new WaitForSeconds(timerLength);
        _imageShield.SetActive(false);
        _shieldsUp = false;
    }

    IEnumerator StartTripleShot(float timerLength)
    {     
        yield return new WaitForSeconds(timerLength);
        _tripleShot = false;
    }

    IEnumerator StartSpeedBoost(float timerLength)
    {
        yield return new WaitForSeconds(timerLength);
        _speedBoost = false;
    }

    IEnumerator DelayReset(float timerLength)
    {        
        yield return new WaitForSeconds(2.8f);
        ResetPlayer();
    }
}




public class Bounds
{
    public float Top { get; set; }
    public float Right { get; set; }
    public float Bottom { get; set; }
    public float Left { get; set; }

    public Bounds(float top, float right, float bottom, float left)
    {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
    }
    // Other properties, methods, events...
}