using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] Transform player;
    Vector3 playerPosition;
    Vector3 targetPosition;
    // Start is called before the first frame update
    void Awake()
    {
        if(player != null)
        {
            playerPosition = player.position;
        }
        else
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetPosition = playerPosition - transform.position;
        transform.Translate(targetPosition * projectileSpeed * Time.fixedDeltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 1<<6||collision.collider.CompareTag("Player")|| collision.collider.CompareTag("Enemy"))
        {
            Debug.Log($"{collision.collider.name} hit by {gameObject}");
        }
        Destroy(gameObject);
    }
}
