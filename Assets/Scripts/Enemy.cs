using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 3.5f;
    private int points;
    private Player _player;
    private Animator _animation;
    private AudioSource _explosionSound;
    private BoxCollider2D _boxCollider2d;
    [SerializeField]
    private GameObject _enemyLaser;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player Not Called");
        }
        _animation = GetComponent<Animator>();
        if(_animation == null)
        {
            Debug.LogError("Animator Not Called");
        }
        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        if(_explosionSound == null)
        {
            Debug.LogError("Explosion Audio Clip Not Called");
        }
        _boxCollider2d = GetComponent<BoxCollider2D>();
        StartCoroutine("_randomFire");

    }
    

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -3.8f)
        {
            transform.position = new Vector3(Random.Range(-10f, 10f), 8f, 0);
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _animation.SetTrigger("OnEnemyDeath");
            _boxCollider2d.enabled = false;
            _explosionSound.Play();
            _speed = 0;
            Destroy(gameObject, 2.4f);

        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
           if(_player != null)
            {
                _player.ScoreUpdate(10);
                
            }
            _animation.SetTrigger("OnEnemyDeath");
            _boxCollider2d.enabled = false;
            _explosionSound.Play();
            _speed = 0;
            Destroy(gameObject, 2.4f);
        }

    }

    IEnumerator _randomFire()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        }
    }
}
