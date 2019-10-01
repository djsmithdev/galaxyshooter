using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;

    [SerializeField]
    private int _scorePerEnemy = 10;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private AudioClip _explosionAudioClip;

    private Player _player;

    private Animator _animator;

    private AudioSource _audioSource;

    private bool _alive = true;


    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();

        GameObject _playerObject = GameObject.Find("Player");



        if (_playerObject != null)
        {
            _player = _playerObject.GetComponent<Player>();
        }

        _direction = new Vector3(Random.Range(-0.5f, 0.5f), -1, 0);

        StartCoroutine(StartShooting());
    }

    // Update is called once per frame
    void Update()
    {
        // move the enemy down 4m/ps
        // move enemy down
        MoveEnemy(_direction);
        
        
        CheckEnemyBounds();
    }

    IEnumerator StartShooting()
    {
        while (_alive == true && _player)
        {
            Vector3 heading = _player.transform.position - _laserPrefab.transform.position;

            _laserPrefab.GetComponent<Laser>().setDirection(new Vector3(Random.Range(-0.5f, 0.5f), -1, 0));
            FireProjectile(_laserPrefab);
            yield return new WaitForSeconds(0.25f);
        }

    }



    void FireProjectile(GameObject projectilePrefab)
    {
        Vector3 newLocation = new Vector3(transform.position.x, transform.position.y - 0.75f + transform.position.z);
        GameObject projectile = Instantiate(projectilePrefab, newLocation, Quaternion.identity);
    }

    void MoveEnemy(Vector3 direction)
    {
        
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    void CheckEnemyBounds()
    {
        // if bottom of screen, respawn at top with a new random x position
        if (transform.position.y < -4.0f)
        {
            transform.position = new Vector3(Random.Range(-9.3f,9.3f), 6.2f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_alive == true)
        {
            if (other.tag == "Laser")
            {
                // if other is laser, destroy laser then self
                _player.addScore(_scorePerEnemy);
                _animator.SetTrigger("OnEnemyDeath");
                _speed = _speed / 1.5f;
                _alive = false;

                // play explosion sound
                _audioSource.PlayOneShot(_explosionAudioClip);
                Destroy(GetComponent<Collider2D>());
                Destroy(other.gameObject);
                Destroy(this.gameObject, 2.8f);
            }
            else if (other.tag == "Player")
            {
                _player.Damage(25);
                _animator.SetTrigger("OnEnemyDeath");
                _speed = _speed / 1.5f;
                _alive = false;

                // play explosion sound
                _audioSource.PlayOneShot(_explosionAudioClip);

                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);

            }
        }
    }
}
