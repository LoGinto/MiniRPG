using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTransformGetter : MonoBehaviour
{
    [SerializeField] Transform lockToTransform;
    public float lockResetDist = 10f;
    private void Start()
    {
        if(lockToTransform != null)
        {
            lockToTransform.gameObject.isStatic = true;
        }
    }
    public Transform GetLockToTransform()
    {
        if(lockToTransform != null)
        {
            return lockToTransform;
        }
        else
        {
            return this.transform;
        }
    }
}
