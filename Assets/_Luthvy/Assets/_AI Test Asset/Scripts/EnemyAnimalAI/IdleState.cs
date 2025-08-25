using UnityEngine;

public class IdleState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;
    private float timer;

    public IdleState(EnemyAnimalAI enemyAI) // REGISTER STATE
    {
        enemy = enemyAI;
    }

///////////////////////////////////////////////////////////////////////
/// STATE ENTER
    public void Enter()
    {
        timer = enemy.idleDuration;
        Debug.Log("Entering Idle");
    }
    
///////////////////////////////////////////////////////////////////////
/// STATE UPDATE
    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            enemy.SwitchState(new PatrolState(enemy));
        }
    }
    
///////////////////////////////////////////////////////////////////////
/// STATE EXIT
    public void Exit()
    {
        Debug.Log("Exiting Idle");
    }
}