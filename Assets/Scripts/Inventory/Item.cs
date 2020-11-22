using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulItem
{
    public class Item : ScriptableObject
    {
        [Header("Info")]
        public Sprite itemIcon;
        public string itemName;
    }
}
