using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private int _timeToLive = 2; // seconds
    [SerializeField]
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move laser up
        MoveLaser(_direction);

        // check laser
        CheckLaser();
    }

    public void setDirection(Vector3 direction)
    {
        _direction = direction;
    }

    void MoveLaser(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    void CheckLaser()
    {
        Destroy(this.gameObject, _timeToLive);
    }

    private void OnDestroy()
    {
        if (transform.parent != null && transform.parent.name != "Projectiles") // if object has a parent
        {
            if (transform.childCount <= 1) // if this object is the last child
            {
                Destroy(transform.parent.gameObject); // destroy parent a few frames later
            }
        }
    }
}
