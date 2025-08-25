using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;
    private Transform player;
    public AttackState(EnemyAnimalAI enemyAI, Transform playerTrasform) // REGISTER STATE
    {
        enemy = enemyAI;
        player = playerTrasform;
    }

///////////////////////////////////////////////////////////////////////
/// STATE ENTER
    public void Enter()
    {
        Debug.Log("ATTAAAACCCCKKK");
    }

///////////////////////////////////////////////////////////////////////
/// STATE UPDATE

    public void Update()
    {
        Debug.LogError("Attacked Launched");
        enemy.SwitchState(new ChaseState(enemy, player));
    }

///////////////////////////////////////////////////////////////////////
/// STATE EXIT

    public void Exit()
    {
        Debug.Log("Exiting Attack");
    }
}
