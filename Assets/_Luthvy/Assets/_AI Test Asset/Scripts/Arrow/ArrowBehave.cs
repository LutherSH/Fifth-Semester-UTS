using UnityEngine;

public class ArrowBehave : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

    public int damageAmount = 10;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        {
            if (other.CompareTag("Player"))
            {
                PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
                if (player != null)
                {
                    player.PlayerTakeDmg(damageAmount);
                    Debug.Log("Player took " + damageAmount + " damage. Current health: " +
                    GameManager.gameManager._playerHealth.Health + "/" +
                    GameManager.gameManager._playerHealth.MaxHealth);
                }   
                // Damage to Player
            }
        }
    }
}