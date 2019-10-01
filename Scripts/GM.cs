using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool restart = Input.GetButton("Jump");

        if (restart == true && _isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }

    public void SetGameOver()
    {
        _isGameOver = true;
        _spawnManager.StopAllSpawning();
        Destroy(GameObject.Find("Player"));
    }

    public bool GetGameOver()
    {
        return _isGameOver;
    }
}
