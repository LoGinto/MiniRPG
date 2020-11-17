using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulItem
{
    [CreateAssetMenu(menuName ="Item/WeaponItem")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One handed")]
        public string On_Light_Attack_1;
        public string On_Heavy_Attack_1;
    }
}