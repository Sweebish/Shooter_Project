using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Animator _anim;
    [SerializeField]
    private SpawnManager _spawnManager;
    private AudioSource _explosionSound;
    private CircleCollider2D _collider2d;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();
        _collider2d = GetComponent<CircleCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, (-10 * Time.deltaTime));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                _anim.SetTrigger("OnAsteroidDeath");
                Destroy(gameObject, 2.4f);
            }
        }
        if (other.tag == "Laser")
            {
            Destroy(other.gameObject);
            _explosionSound.Play();
            _anim.SetTrigger("OnAsteroidDeath");
            _collider2d.enabled = false;
            _spawnManager.AsteroidDeath();
            Destroy(gameObject, 2.4f);
            }
    }
}
