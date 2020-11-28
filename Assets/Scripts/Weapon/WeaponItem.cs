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
        [Header("Stamina drain")]
        public float baseStaminaDrain;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
        [Header("One handed")]
        public string On_Light_Attack_1;
        public string On_Heavy_Attack_1;
        [Header("Secondary one handed attacks")]
        public string On_Light_Attack_2;
        public string On_Heavy_Attack_2;
        [Header("Two handed Attack")]
        public string On_Light_THA_01;
        public string On_Heavy_THA_01;
        [Header("Two handed secondary attack")]
        public string On_Light_THA_02;
        public string On_Heavy_THA_02;
        [Header("Idle")]
        public string right_hand_idle;
        public string left_hand_idle;
        public string th_idle; 

    }
}