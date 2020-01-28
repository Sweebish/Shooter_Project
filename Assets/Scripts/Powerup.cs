using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    // 0 = triple shot 1 = speed up 2 = shields
    [SerializeField]
    private int powerupID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        Debug.Log("Triple Shot!");
                        player.TripleShotActive();
                    break;

                    case 1:
                        Debug.Log("Speed Up!");
                        player.SpeedUpActive();
                    break;

                    case 2:
                        Debug.Log("Shields!");
                        player.ShieldActive();
                    break;
                }
                
            }
            Destroy(this.gameObject);
        }
       
    }
}
