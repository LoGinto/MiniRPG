using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulItem
{    
    public class Interactible : MonoBehaviour
    {
        public float radius = 2f;
        public string interactibleText;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,radius);
        }
        public virtual void Interact()
        {
            Debug.Log("I picked up");
        }
    }
}
