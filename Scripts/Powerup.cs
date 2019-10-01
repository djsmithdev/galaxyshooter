using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // ID's for Powerups
    // 1: Triple Shot
    // 2: Speed
    // 3: Shields
    [SerializeField]
    private int powerupID;

    [SerializeField]
    private float _speed = 1.0f;

    [SerializeField]
    private float _powerupLife = 10.0f;

    [SerializeField]
    private AudioClip _powerupAudioClip;

    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // move down at a speed of 3
        MovePowerup(Vector3.down);
        Destroy(this.gameObject, _powerupLife);

    }

    void MovePowerup(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);
    }


    // check for collisisons
    // check for player
    // collectable by player only  hint: use tags
    // on collected, destroy
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Laser")
        {
            // if other is laser, destroy laser then self
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            // ID's for Powerups
            // 1: Triple Shot
            // 2: Speed
            // 3: Shields
            switch(powerupID)
            {
                case 0:
                    other.transform.GetComponent<Player>().EnableTripleShot(5.0f);
                    break;
                case 1:
                    other.transform.GetComponent<Player>().EnableSpeedBoost(5.0f);
                    break;
                case 2:
                    other.transform.GetComponent<Player>().EnableShields(5.0f);
                    break;

                default:
                    Debug.Log("Error in powerupID");
                    break;
            }

            // play sound
            _audioSource.PlayOneShot(_powerupAudioClip);

            Destroy(this.gameObject);
        }
    }
}
