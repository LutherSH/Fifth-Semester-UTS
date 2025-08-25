using UnityEngine;

public class PatrolState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;

    public PatrolState(EnemyAnimalAI enemyAI) // REGISTER STATE
    {
        enemy = enemyAI;
    }

///////////////////////////////////////////////////////////////////////
/// STATE ENTER
    public void Enter()
    {
        Debug.Log("Entering Patrol");
    }

///////////////////////////////////////////////////////////////////////
/// STATE UPDATE
    public void Update()
    {
        if (enemy.waypoints.Length == 0) return;

        Transform target = enemy.waypoints[enemy.currentWaypointIndex];
        enemy.transform.position = Vector3.MoveTowards(
            enemy.transform.position,
            target.position,
            enemy.speed * Time.deltaTime);

        // Check if reached waypoint
        if (Vector3.Distance(enemy.transform.position, target.position) < 0.1f)
        {
            enemy.currentWaypointIndex = (enemy.currentWaypointIndex + 1) % enemy.waypoints.Length;
            enemy.SwitchState(new IdleState(enemy));
        }
    }

///////////////////////////////////////////////////////////////////////
/// STATE EXIT
    public void Exit()
    {
        Debug.Log("Exiting Patrol");
    }
}