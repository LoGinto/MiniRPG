using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulItem
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOvveride;
        public bool isLeftSlot;
        public bool isRightHandSlot;
        public bool isBackSlot;
        public GameObject currentWeaponModel;
        public WeaponItem currentWeapon;

        public  void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }
        public void UnloadAndDestroy()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }
        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadAndDestroy();
            if(weaponItem == null)
            {
                UnloadWeapon();
                return;
            }
            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if(model != null)
            {
                if(parentOvveride != null)
                {
                    model.transform.parent = parentOvveride;
                }
                else
                {
                    model.transform.parent = transform;
                }
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }
            currentWeaponModel = model; 
        }
    }
}
