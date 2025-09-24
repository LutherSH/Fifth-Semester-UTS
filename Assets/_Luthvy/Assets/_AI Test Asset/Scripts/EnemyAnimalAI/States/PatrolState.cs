using UnityEngine;
using UnityEngine.AI;

public class PatrolState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;
    private const float arriveTolerance = 0.5f;
    private const int sampleAttempts = 8;

    public PatrolState(EnemyAnimalAI enemyAI) // REGISTER STATE
    {
        enemy = enemyAI;
    }

///////////////////////////////////////////////////////////////////////
/// STATE ENTER
    public void Enter()
    {
        Debug.Log("Entering Patrol");

        if (enemy.nAgent == null) enemy.nAgent = enemy.GetComponent<NavMeshAgent>();

        enemy.nAgent.isStopped = false;
        enemy.nAgent.updateRotation = false;

        if (!enemy.setAWalkPoint)
        {
            TryFindAndSetWalkPoint();
        }
        else
        {
            enemy.nAgent.SetDestination(enemy.walkPoint);
        }
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE UPDATE
    public void Update()
    {
        if (enemy.nAgent == null) return;

        if (!enemy.setAWalkPoint)
        {
            TryFindAndSetWalkPoint();
            return;
        }

        if (!enemy.nAgent.hasPath || Vector3.Distance(enemy.nAgent.destination, enemy.walkPoint) > 0.2f)
        {
            enemy.nAgent.SetDestination(enemy.walkPoint);
        }

        if (enemy.nAgent.pathPending) return;

        if (enemy.nAgent.remainingDistance <= Mathf.Max(enemy.nAgent.stoppingDistance, arriveTolerance))
        {
            enemy.setAWalkPoint = false;
            enemy.SwitchState(new IdleState(enemy));
        }
        
        // Face Direction
        Vector3 velocity = enemy.nAgent.desiredVelocity;
        if (velocity.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(velocity.normalized);
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            lookRotation,
            Time.deltaTime * 5f
            );
        }

    }
    
///////////////////////////////////////////////////////////////////////
/// STATE EXIT
    public void Exit()
    {
        Debug.Log("Exiting Patrol");
    }

///////////////////////////////////////////////////////////////////////
/// FIND AND SET WALKPOINT
    private void TryFindAndSetWalkPoint()
    {
        if (enemy == null) return;

        float range = Mathf.Max(0.1f, enemy.rangeOfWalkpoint);

        for (int i = 0; i < sampleAttempts; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-range, range),
                0f,
                Random.Range(-range, range)
            );

            Vector3 samplePos = enemy.transform.position + randomOffset;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(samplePos, out hit, 2.0f, NavMesh.AllAreas))
            {
                enemy.walkPoint = hit.position;
                enemy.setAWalkPoint = true;

                if (enemy.nAgent != null)
                {
                    enemy.nAgent.SetDestination(enemy.walkPoint);
                }

                return;
            }
        }

        enemy.setAWalkPoint = false;
    }
}