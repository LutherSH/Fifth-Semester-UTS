using UnityEngine;

public class ChaseState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;
    //private Transform player;

    public ChaseState(EnemyAnimalAI enemyAI) //Transform playerTransform) // REGISTER STATE AND THE PROPERTIES
    {
        enemy = enemyAI;
        //player = playerTransform;
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE ENTER
    public void Enter()
    {
        Debug.Log("Entering Chase");
        if (enemy.nAgent != null)
        {
            enemy.nAgent.isStopped = false;
        }
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE UPDATE
    public void Update()
    {
        if (enemy.player == null)
            return;

        enemy.nAgent.SetDestination(enemy.player.position);

        if (!enemy.playerInSightRange)
        {
            enemy.SwitchState(new IdleState(enemy));
        }

        else if (enemy.playerInAttackRange)
        {
            enemy.SwitchState(new AttackState(enemy));
        }
    }
    
///////////////////////////////////////////////////////////////////////
/// STATE EXIT
    public void Exit()
    {
        Debug.Log("Exiting Chase");
    }
}