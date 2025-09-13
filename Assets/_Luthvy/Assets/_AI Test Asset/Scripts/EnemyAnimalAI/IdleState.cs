using UnityEngine;

public class IdleState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;
    private float idleTimer;
    private float idleDuration;

    public IdleState(EnemyAnimalAI enemyAI) // REGISTER STATE
    {
        enemy = enemyAI;
        idleDuration = enemy.idleDuration;
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE ENTER
    public void Enter()
    {
        Debug.Log("Entering Idle");
        idleTimer = 0f;
        if (enemy.nAgent != null)
        {
            enemy.nAgent.isStopped = true;
        }
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE UPDATE
    public void Update()
    {
        if (enemy.playerInSightRange && !enemy.playerInAttackRange)
        {
            enemy.nAgent.isStopped = false;
            enemy.SwitchState(new ChaseState(enemy));
            return;
        }

        else if (enemy.playerInSightRange && enemy.playerInAttackRange)
        {
            enemy.nAgent.isStopped = false;
            enemy.SwitchState(new AttackState(enemy));
            return;
        }

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            if (enemy.nAgent != null)
            {
                enemy.nAgent.isStopped = false;
            }

            enemy.SwitchState(new PatrolState(enemy));
        }
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE EXIT
    public void Exit()
    {
        Debug.Log("Exiting Idle");
        if (enemy.nAgent != null)
        {
            enemy.nAgent.isStopped = false;
        }
    }
}