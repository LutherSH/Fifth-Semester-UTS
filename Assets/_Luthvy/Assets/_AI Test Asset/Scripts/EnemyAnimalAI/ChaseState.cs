using UnityEngine;

public class ChaseState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;
    private Transform player;

    public ChaseState(EnemyAnimalAI enemyAI, Transform playerTransform) // REGISTER STATE AND THE PROPERTIES
    {
        enemy = enemyAI;
        player = playerTransform;
    }
    
///////////////////////////////////////////////////////////////////////
/// STATE ENTER
    public void Enter()
    {
        Debug.Log("Entering Chase");
    }
    
///////////////////////////////////////////////////////////////////////
/// STATE UPDATE
    public void Update()
    {
        if (player == null) return;

        // DIRECTION TO PLAYER
        Vector3 direction = (player.position - enemy.transform.position).normalized;

        // FACE TO PLAYER
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            lookRotation,
            Time.deltaTime * enemy.rotationSpeed);
        }

        // MOVING TO PLAYER
        enemy.transform.position += direction * enemy.chaseSpeed * Time.deltaTime;

        // PLAYER FLEE ? GO BACK TO PATROL
        if (Vector3.Distance(enemy.transform.position, player.position) > enemy.chaseStopRange)
        {
            enemy.SwitchState(new PatrolState(enemy));
        }
    }
    
///////////////////////////////////////////////////////////////////////
/// STATE EXIT
    public void Exit()
    {
        Debug.Log("Exiting Chase");
    }
}