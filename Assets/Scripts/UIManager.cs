using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _liveSprite;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private GameObject _gameOver;
    [SerializeField]
    private GameObject _restartLevel;
    private GameManager _gameManager;
    [SerializeField]
    private Text _ammoCounter;
    [SerializeField]
    private Slider _fuelBar;


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _ammoCounter.text = "Ammo: " + 15;
        if (_gameManager == null)
        {
            Debug.LogError("Game_Manager Failed to Call");
        }
    }

    void Update()
    {

    }
    public void GetScore(int _score)
    {
        _scoreText.text = "Score: " + _score;
    }
    public void GetAmmo(int _ammo)
    {
        _ammoCounter.text = "Ammo: " + _ammo;
        if (_ammo == 0)
        {
            _ammoCounter.color = Color.red;
        }
        else if (_ammo > 0)
        {
            _ammoCounter.color = Color.white;
        }
    }
    public void UpdateLives(int CurrentLives)
    {
        _livesImage.sprite = _liveSprite[CurrentLives];
        if (CurrentLives < 1)

        {
            GameOverHandler();
        }
    }
    public void UpdateFuelBar(float _fuel)
    {
        _fuelBar.value = _fuel;
    }
    private void GameOverHandler()
    {
        _gameManager.GameOver();
        _restartLevel.SetActive(true);
        StartCoroutine("GameOverBlinker");
    }
    IEnumerator GameOverBlinker()
    {
        while(true)
        {
            _gameOver.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
