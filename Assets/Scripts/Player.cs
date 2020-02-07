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
    private int _shieldActive;
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
    private Renderer _thrusterAnim;
    [SerializeField]
    private AudioSource _laserSound;
    private AudioSource _powerUpSound;
    private SpriteRenderer _playerShieldColor;
    [SerializeField]
    private int _ammoCounter = 15;
    [SerializeField]
    private GameObject _healthPowerup;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _fuel = 3f;
    private bool _thrusters;

    

    void PlayerMovement()
    {

        if (_speedUpActive == false)
        {
            _speed = 10f;
        }
        else if(_speedUpActive == true)
        {
            _speed = 15f;
        }
        if (Input.GetKey(KeyCode.LeftShift) && _fuel > 0f)
        {
            _thrusters = true;
            ThrustersActive();

        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * _speed * Time.deltaTime * v);
        transform.Translate(Vector3.right * _speed * Time.deltaTime * h);

    }
    void ThrustersActive()
    {
        if(_thrusters == true)
        {
            _speed = _speed * 2;
            StartCoroutine("_ThrusterFuel");
        }
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
            _ammoCounter--;
            _uiManager.GetAmmo(_ammoCounter);
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
    public void CameraShake()
    {
        StartCoroutine("_Shaker");
    }
    public void Damage()
    {
        if(_shieldActive > 0)
        {
            switch(_shieldActive)
            { 
            case 3:
                _shieldActive--;
                    _playerShieldColor.color = Color.green;
                break;
            case 2:
                _shieldActive--;
                _playerShieldColor.color = Color.red;
                break;
            case 1:
                _shieldActive--;
                _playerShield.SetActive(false);
                break;
            }
            return;
        }

        _lives -= 1;
        CameraShake();
        //_gameManager.ShakeCamera();
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnplayerDeath();
            _leftEngine.enabled = false;
            _rightEngine.enabled = false;
            _playerSprite.enabled = false;
            _thrusterAnim.enabled = false;
        }
        switch(_lives)
        {
            case 2:
                _rightEngine.enabled = true;
                break;
            case 1:
                _leftEngine.enabled = true;
                _rightEngine.enabled = true;
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
        _playerShieldColor.color = Color.white;
        _powerUpSound.Play();
        _shieldActive = 3;
    }
    public void ScoreUpdate(int points)
    {
        _score += points;
        _uiManager.GetScore(_score);
    }
    public void AmmoRefill()
    {
        _ammoCounter += 5;
        _powerUpSound.Play();
        _uiManager.GetAmmo(_ammoCounter);
    }
    public void RefillHealth()
    {
        if(_lives < 3)
        {
            _lives++;
            _powerUpSound.Play();
            _uiManager.UpdateLives(_lives);
        }
        if(_rightEngine.enabled == true && _lives == 3)
        {
            _rightEngine.enabled = false;
        }
        if(_leftEngine.enabled == true && _lives == 2)
        {
            _leftEngine.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _powerUpSound = GetComponent<AudioSource>();
        _playerShieldColor = _playerShield.GetComponent<SpriteRenderer>();
        
        if(_playerShieldColor == null)
        {
            Debug.LogError("_playerShieldColor failed to call");
        }
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCounter > 0)
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
    IEnumerator _Shaker()
    {
        float i;
        Vector3 _originalPos = new Vector3(0, 0, -10);

        for (i = 5; i > 0f; i--)
        {
            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(-0.5f, 0.5f);
            float z = _camera.transform.position.z;
            _camera.transform.position = new Vector3(x, y, z);
            yield return new WaitForSeconds(0.1f);
            Debug.Log("frame" + i + " X=" + _camera.transform.position.x + " Y=" + _camera.transform.position.y);
        }
        _camera.transform.position = _originalPos;
        Debug.Log("return to center");
    }
    IEnumerator _ThrusterFuel()
    {
        for (_fuel = 3f; _fuel >= 0f; _fuel--)
        {
            _uiManager.UpdateFuelBar(_fuel);
            yield return new WaitForSeconds(1);
        }
        if(_fuel <= 0f)
        {
            _fuel = 0;
            _thrusters = false;
            yield return new WaitForSeconds(10);
            _fuel = 3f;
            _uiManager.UpdateFuelBar(_fuel);
        }
    }
}
