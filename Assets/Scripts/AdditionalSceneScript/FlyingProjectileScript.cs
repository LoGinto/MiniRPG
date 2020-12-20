using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingProjectileScript : MonoBehaviour
{
    public Transform target;
    public float firingAngle = 45f;
    public float gravity = 9.8f;
    Transform projectile;
    Transform myTransform;

    private void Awake()
    {
        projectile = this.gameObject.transform;
        myTransform = transform;
        if (FindObjectOfType<LockOn>().lookAtTransform != null)
        {
            target = FindObjectOfType<LockOn>().lookAtTransform;
        }
        else
        {
            target = FindObjectOfType<BetterFighter>().emptryThrowTarget;
        }
        this.transform.parent = null;
        StartCoroutine(Launch());
    }

    public void LaunchProjectile()
    {
       
            this.transform.parent = null;
            StartCoroutine(Launch());
        
    }
    IEnumerator Launch()
    {
        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(0);

        // Move projectile to the position of throwing object + add some offset if needed.
        projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(projectile.position, target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        projectile.rotation = Quaternion.LookRotation(target.position -projectile.position);

        float elapse_time = 0;  

        while (elapse_time < flightDuration)
        {
            projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            if (collision.collider.name != "Player" || FindObjectOfType<BetterFighter>().weaponObject.GetInstance())
            {
                Destroy(gameObject);
            }
        }
        catch
        {
            if(collision.collider.name != "Player")
            {
                Destroy(gameObject);
            }
        }
    }
}
