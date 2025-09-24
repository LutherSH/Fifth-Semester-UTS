using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyEvent : MonoBehaviour
{
    [Header("Settings")]
    public string enemyTag = "Enemy";
    public bool triggerOnce = true;

    private List<GameObject> enemiesInArea = new List<GameObject>();
    private bool triggered = false;

    private void Start()
    {
        // Automatically find all enemies inside the trigger at Start
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                enemiesInArea.Add(hit.gameObject);
            }
        }
    }

    private void Update()
    {
        if (triggered && triggerOnce) return;

        // Clean up null/destroyed enemies from the list
        enemiesInArea.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (enemiesInArea.Count == 0)
        {
            triggered = true;
            OnAllEnemiesDead();
        }
    }

    private void OnAllEnemiesDead()
    {
        Debug.LogWarning("All enemies inside this area are gone!");
        // Place your trigger code here (open door, spawn loot, etc.)
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
