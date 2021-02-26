using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour   
{
    [SerializeField] float missleSpeed = 5f;
    [SerializeField] float waitTime = 2f;
    public Transform upperTransform;
    Transform player;
    bool flyStarted = false;
    Vector3 targetPosition;
    private void Awake()
    {
        player = FindObjectOfType<Movement>().transform;
        StartCoroutine(FlyTowards());
    }
    IEnumerator FlyTowards()
    {
        yield return new WaitForSeconds(waitTime);
        targetPosition = player.position - transform.position;
        flyStarted = true;
    }
    private void FixedUpdate()
    {
        if(flyStarted == true)
        {        
            transform.Translate(targetPosition * missleSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (upperTransform != null)
            {
                transform.Translate(upperTransform.position * missleSpeed * Time.fixedDeltaTime);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 1 << 6 || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy"))
        {
            Debug.Log($"{collision.collider.name} hit by {gameObject}");
        }
        if (collision.collider.tag != "EnemyProjectile")
        {
            Destroy(gameObject);
        }
    }
}
