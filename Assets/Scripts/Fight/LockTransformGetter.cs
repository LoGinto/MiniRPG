using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTransformGetter : MonoBehaviour
{
    [SerializeField] Transform lockToTransform;
    public float lockResetDist = 10f;
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
