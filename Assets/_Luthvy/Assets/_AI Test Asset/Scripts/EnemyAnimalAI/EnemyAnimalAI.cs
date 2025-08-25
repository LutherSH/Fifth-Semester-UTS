using UnityEngine;

public class EnemyAnimalAI : MonoBehaviour
{
    private TheState currentState;

///////////////////////////////////////////////////////////////////////
///// Property For Patrol

    [Header("Patrol Settings")] 
    public Transform[] waypoints;
    [HideInInspector] public int currentWaypointIndex = 0;
    public float speed = 2f;

///////////////////////////////////////////////////////////////////////
//// Property For Idle

    [Header("Idle Settings")] 
    public float idleDuration = 2f;

///////////////////////////////////////////////////////////////////////
///// Property For Chase

    [Header("Chase Settings")] 
    public Transform player;
    public float chaseSpeed = 3.5f;
    public float chaseRange = 5f;         
    public float chaseStopRange = 7f;  
    public float rotationSpeed = 1f;

///////////////////////////////////////////////////////////////////////
//// Property For Attack

    [Header("Attack Settings")]
    public float launchAttackRange = 3f;

///////////////////////////////////////////////////////////////////////
/// START

    void Start()
    {
        SwitchState(new IdleState(this)); // IDLE
    }
///////////////////////////////////////////////////////////////////////
/// UPDTAE
    void Update()
    {
        if (player != null) // PLAYER CHECK
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // CHECK RANGE
            if (distanceToPlayer < chaseRange)
            {
                // MOVE TO PLAYER
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // RAYCAST SHOT
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, chaseRange))
                {
                    Debug.DrawLine(transform.position + Vector3.up, hit.point, Color.red);

                    // SEE PLAYER ? CHASE PLAYER .
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (!(currentState is ChaseState))
                        {
                            SwitchState(new ChaseState(this, player));
                        }
                    }

                    // CANT SEE PLAYER ? DONT CHASE PLAYER
                    else
                    {
                        SwitchState(new PatrolState(this));
                    }
                }
             }
            // else
            // { 
            //     if (currentState is ChaseState)
            //     {
            //         SwitchState(new IdleState(this));
            //     }
            // }
        }

        // STATE CHECK
        if (currentState != null)
        {
            currentState.Update();
        }
    }

///////////////////////////////////////////////////////////////////////
/// STATE SWITCHER
    public void SwitchState(TheState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

///////////////////////////////////////////////////////////////////////
/// DRAW GIZMIOZ
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseStopRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, launchAttackRange);
        
    }
}