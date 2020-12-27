using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAI 
{
    void Attack();
    void Chase();
    void Calm();
    void LostSight();
    void StateChange();
    void TickBehavior();
    void AIMove();
    //EnemyStat GetEnemyStat();
}
