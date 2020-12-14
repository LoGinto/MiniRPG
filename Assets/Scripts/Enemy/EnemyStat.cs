using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyStuff/EnemyStatObject")]
public class EnemyStat : ScriptableObject
{
    public int level;

    [Header("Attack animations")]
    public float attackCoolDown = 2f;
    public string attackAnim1;
    public string attackAnim2;
    public string attackAnim3;
    public string attackAnim4;
    [Header("Combo animations")]
    public float comboChance = 0.25f;
    public string combo1;
    public string combo2;
}
