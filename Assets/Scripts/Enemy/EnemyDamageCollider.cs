using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCollider : MonoBehaviour
{
    Collider enemyWeaponCollider = null;
    // Start is called before the first frame update
    void Start()
    {
        //yield return new WaitForSeconds(1.5f);
        try
        {
            AssignWeaponCollider();
            Debug.Log("executed");
        }
        catch { }
    }
    private void Update()
    {
        this.enabled = true; 
        AssignWeaponCollider();
    }
     void AssignWeaponCollider()
    {
        if(enemyWeaponCollider == null)
        {
            if(GetComponent<Enemy>().weaponObject != null)
            {
                try
                {
                    enemyWeaponCollider = GetComponent<Enemy>().weaponObject.GetInstance().GetComponent<Collider>();
                }
                catch
                {
                    enemyWeaponCollider = GameObject.Find(GetComponent<Enemy>().weaponObject.GetInstance().name).GetComponent<Collider>();
                }              
            }
        }
    }
    public  void TurnOnColliderAnim()
    {
        if (enemyWeaponCollider != null)
        {
            enemyWeaponCollider.enabled = true; 
            Debug.Log("Weapon enabled");
        }
    }
    public  void TurnOffColliderAnim()
    {
        if (enemyWeaponCollider != null)
        {
            enemyWeaponCollider.enabled = false;
            Debug.Log("Weapon disabled");
        }
    }
    public Collider GetCollider()
    {
        return enemyWeaponCollider;
    }
}
