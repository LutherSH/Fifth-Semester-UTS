using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseEnemyEvent : MonoBehaviour
{
/////////////////////////////////////////////////////////////////////
/// 
    [Header("Settings")]
    public string enemyTag = "Enemy";
    public bool triggerOnce = true;
    public bool lastEvent;
    private GameObject theEventGameobject;
/////////////////////////////////////////////////////////////////////
/// 
    [Header("Rewards")]
    public GameObject objectReward;
    public GameObject activateNextEvent;
    public SceneManagerTG sceneManager;
/////////////////////////////////////////////////////////////////////
    /// 
    private List<GameObject> enemiesInArea = new List<GameObject>();
    private bool triggered = false;

/////////////////////////////////////////////////////////////////////

    private void Start()
    {
        theEventGameobject = gameObject;
        objectReward.SetActive(true);
        activateNextEvent.SetActive(false);

        // FIND ENEMY
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(enemyTag))
            {
                enemiesInArea.Add(hit.gameObject);
            }
        }
    }

    /////////////////////////////////////////////////////////////////////
    /// 
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

        if (!lastEvent && triggered)
        {
            activateNextEvent.SetActive(true);
        }

        if (lastEvent && triggered)
        {
            sceneManager.TemporaryGameWin();
        }
    }
    /////////////////////////////////////////////////////////////////////
    /// 
    private void OnAllEnemiesDead()
    {
        Debug.LogWarning("All enemies inside this area are gone!");
        objectReward.SetActive(false);
        theEventGameobject.SetActive(false);
    }
    
/////////////////////////////////////////////////////////////////////
    /// 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
