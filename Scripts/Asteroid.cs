using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{


    [SerializeField]
    private float _speed = 1.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private AudioClip _explosionAudioClip;

    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private float _randomSize;
    private float _rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();

        _randomSize = Random.Range(0.2f, 1f);
        this.transform.localScale = new Vector3(1f, 1f, 1f) * _randomSize;

        _rotationSpeed = Random.Range(-10.0f,10.0f);
        if (_spawnManager == null)
        {
            Debug.Log("Error, SpawnManager not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // rotate object on the z axis
        Vector3 _direction = new Vector3(Random.Range(-0.75f, 0.75f), _speed, 0);
        transform.Rotate(new Vector3(0, 0, _rotationSpeed) * Time.deltaTime);
        transform.Translate(_direction * -_speed * Time.deltaTime);

    }

    //check for collision detection
    // of laser
    // instantiate explosion at position of asteroid
    // destroy the explosion after finished playing

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Laser")
        {
            // load in explosion at astroid location

            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(explosion.gameObject, 1.55f);
            

            // play explosion sound
            _audioSource.PlayOneShot(_explosionAudioClip);
            explosion.transform.localScale = new Vector3(1f, 1f, 1f) * _randomSize;

            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy")
        {
            // load in explosion at astroid location

            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<Animator>().SetTrigger("OnEnemyDeath");
            Destroy(other.gameObject, 2.8f);
            Destroy(explosion.gameObject, 1.55f);

            // play explosion sound
            _audioSource.PlayOneShot(_explosionAudioClip);
            explosion.transform.localScale = new Vector3(1f, 1f, 1f) * _randomSize;

            Destroy(this.gameObject);
        }
        else if (other.tag == "Asteroid")
        {
            // load in explosion at astroid location

            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            GameObject explosionOther = Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);

            Destroy(other.gameObject, 1.55f);
            Destroy(explosion.gameObject, 1.55f);

            // play explosion sound
            _audioSource.PlayOneShot(_explosionAudioClip);
            explosion.transform.localScale = new Vector3(1f, 1f, 1f) * _randomSize;

            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            // load in explosion at astroid location
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<Player>().Damage(50);
            Destroy(explosion.gameObject, 1.55f);
            Destroy(this.gameObject, 0.5f);
            _audioSource.PlayOneShot(_explosionAudioClip);
            explosion.transform.localScale = new Vector3(1f, 1f, 1f) * _randomSize;
        }


    }

}
