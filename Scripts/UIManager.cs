using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _waveText;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Text _healthText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Text _gameOverTextObject;

    private Player _player;
    private GM _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("GM").GetComponent<GM>();
    }

    // Update is called once per frame
    void Update()
    {
        int _lives = _player.getLives();
        _scoreText.text = "Score: " + _player.getScore();
        _healthText.text = "Health: " + _player.getHealth();
        _waveText.text = "Wave: " + _player.getWave();

        _livesImage.GetComponent<Image>().sprite = _livesSprites[_lives]; 
        
        if (_lives == 0)
        {
            GameOverSequence();
        }

    }

    private void GameOverSequence()
    {          
        _gameManager.SetGameOver();
        StartCoroutine(GameOver());
    }


    IEnumerator GameOver()
    {
        while (true)
        {
            _restartText.text = "Press [SPACE] or [Jump] to Begin";
            _gameOverTextObject.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverTextObject.text = "";
            yield return new WaitForSeconds(0.5f);
        }        
    }
}
