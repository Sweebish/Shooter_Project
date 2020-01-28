using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshot;
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _tripleShotActive;
    [SerializeField]
    private bool _speedUpActive;
    [SerializeField]
    private bool _shieldActive;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    private Renderer _playerSprite;
    [SerializeField]
    private Renderer _rightEngine;
    [SerializeField]
    private Renderer _leftEngine;
    [SerializeField]
    private Renderer _thruster;
    [SerializeField]
    private AudioSource _laserSound;
    private AudioSource _powerUpSound;

    

    void PlayerMovement()
    {
        if(_speedUpActive == false)
        {
            _speed = 10f;
        }
        else if(_speedUpActive == true)
        {
            _speed = 15f;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * _speed * Time.deltaTime * v);
        transform.Translate(Vector3.right * _speed * Time.deltaTime * h);

    }
    void PlayerBounds()
    {
        // Limit vertial movement and enable screen wrap on hroizontal axes
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        Vector3 offset = new Vector3(0, 1.05f, 0);
        {
            _canFire = Time.time + _fireRate;
            if (_tripleShotActive == true)
            {
                Instantiate(_tripleshot, transform.position, Quaternion.identity);
            }
            else if (_tripleShotActive == false)
            {
                Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            }
        }
    }
    public void Damage()
    {
        if(_shieldActive == true)
        {
            _playerShield.SetActive(false);
            _shieldActive = false;
            return;
        }
        _lives -= 1;
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager.OnplayerDeath();
            _leftEngine.enabled = false;
            _rightEngine.enabled = false;
            _playerSprite.enabled = false;
            _thruster.enabled = false;
        }
        switch(_lives)
        {
            case 2:
                _rightEngine.enabled = true;
                break;
            case 1:
                _leftEngine.enabled = true;
                break;
        }
    }
    public void TripleShotActive()
    {
        _tripleShotActive = true;
        _powerUpSound.Play();
        StartCoroutine("_TripleShotTimer");
    }
    public void SpeedUpActive()
    {
        _speedUpActive = true;
        _powerUpSound.Play();
        StartCoroutine("_SpeedUpTimer");
    }
    public void ShieldActive()
    {
        _playerShield.SetActive(true);
        _powerUpSound.Play();
        _shieldActive = true;
    }
    public void ScoreUpdate(int points)
    {
        _score += points;
        _uiManager.GetScore(_score);
    }
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _powerUpSound = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager Failed to Call");
        }
        if(_uiManager == null)
        {
            Debug.LogError("UIManager Faild to call");
        }
        _playerSprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerBounds();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
            _laserSound.Play();
        }

    }
    IEnumerator _TripleShotTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;

    }
    IEnumerator _SpeedUpTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _speedUpActive = false;
    }
}
