using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer1;
    [SerializeField] LineRenderer lineRenderer2;
    [SerializeField] LineRenderer lineRenderer3;
    [SerializeField] LineRenderer lineRenderer4;
    //[SerializeField] GameObject muzzle;
    [SerializeField] Transform spawn;
    [SerializeField] float shotDistance = 10;
    [SerializeField] float turnOffTime = 0.65f;
    Vector3 spawnVector;
   
    public void ShootTheGun()
    {
        //I could use loop, I know. 
        spawnVector = spawn.forward;
        //RaycastHit hit;
        //RaycastHit hit_1;
        //RaycastHit hit_2;
        //RaycastHit hit_3;
        Ray ray1 = new Ray(spawn.position, spawnVector*shotDistance);
        Ray ray2 = new Ray(spawn.position, (spawnVector + new Vector3(-.2f, 0f, 0f))*shotDistance);
        Ray ray3 = new Ray(spawn.position, (spawnVector + new Vector3(0f, .1f, 0f))*shotDistance);
        Ray ray4 = new Ray(spawn.position, (spawnVector + new Vector3(0f, -.1f, 0f))*shotDistance);
        StartCoroutine(RenderTracer(lineRenderer1, ray1.direction*shotDistance));
        StartCoroutine(RenderTracer(lineRenderer2, ray2.direction*shotDistance));
        StartCoroutine(RenderTracer(lineRenderer3, ray3.direction*shotDistance));
        StartCoroutine(RenderTracer(lineRenderer4, ray4.direction*shotDistance));
    }
    IEnumerator RenderTracer(LineRenderer tracer,Vector3 _hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, spawn.position);        
        tracer.SetPosition(1, spawn.position + _hitPoint);
        yield return new WaitForSeconds(turnOffTime);
        tracer.enabled = false; 
    }
}
