using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : TheState
{
///////////////////////////////////////////////////////////////////////
/// PROPERTIES OF STATE
    private EnemyAnimalAI enemy;
    private Transform player;
    public AttackState(EnemyAnimalAI enemyAI)//, Transform playerTrasform) // REGISTER STATE
    {
        enemy = enemyAI;
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE ENTER
    public void Enter()
    {
        Debug.Log("ATTAAAACCCCKKK");
        enemy.nAgent.isStopped = true;
        enemy.fov = 359f;
    }

    ///////////////////////////////////////////////////////////////////////
    /// STATE UPDATE

    public void Update()
    {
    // Face the player
    Vector3 direction = (enemy.player.position - enemy.transform.position).normalized;
    direction.y = 0; // keep upright, no tilting

    if (direction.magnitude > 0.01f)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            lookRotation,
            Time.deltaTime * 5f // turn speed
        );
    }

        if (enemy.playerInAttackRange)
        {
            Debug.LogError("Attacked Launched");
            FireAtPlayer();
        }

        else
        {
            enemy.SwitchState(new ChaseState(enemy));
        }
        
    }

public void FireAtPlayer()
{
    if (Time.time < enemy.nextFireTime) return;
    enemy.nextFireTime = Time.time + enemy.fireCooldown;

    // Spawn position (use firePoint if available)
    Vector3 spawnPos = enemy.firePoint != null ? enemy.firePoint.position : enemy.transform.position + enemy.transform.forward * 1.0f;

    // Direction to player (use player's current position)
    Vector3 dir = (enemy.player.position - spawnPos);
    if (dir.sqrMagnitude < 0.0001f)
    {
        dir = enemy.firePoint != null ? enemy.firePoint.forward : enemy.transform.forward;
    }
    dir.Normalize();

    // Rotation that looks along 'dir'
    Quaternion rot = Quaternion.LookRotation(dir);

    // If your bullet model's "nose" isn't on +Z, tweak here:
    // rot *= Quaternion.Euler(0f, 90f, 0f); // <- tweak if needed

    // Instantiate with the computed rotation
    GameObject bullet = Object.Instantiate(enemy.arrowPrevab, spawnPos, rot);

    // Give it velocity if it has a Rigidbody
    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.velocity = dir * enemy.arrowSpeed;
    }
    else
    {
        // fallback for non-Rigidbody bullets: orient then move in Update()
        bullet.transform.forward = dir;
    }

    Debug.Log("Fired bullet at player (dir: " + dir + ")");
}


///////////////////////////////////////////////////////////////////////
    /// STATE EXIT

    public void Exit()
    {
        Debug.Log("Exiting Attack");
    }
}
