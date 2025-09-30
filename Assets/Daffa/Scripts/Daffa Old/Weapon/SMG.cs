using System.Collections;
using UnityEngine;

public class SMG : Gun
{
    public override void Update()
    {
        base.Update();

        if (Input.GetButton("Fire1"))
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }
     public override void Shoot()
    {
        RaycastHit hit;
        Vector3 target = Vector3.zero;

        if (Physics.Raycast(cameraTransform.position, gunMuzzle.forward, out hit, gunData.shootingRange/*, gunData.targetLayerMask*/))
        {
            //Debug.Log(gunData.gunName + " hit " + hit.collider.name);

            ApplyDamageToTarget(hit.collider, gunData.damage);
            
            target = hit.point;
        }
        else
        {
            target = cameraTransform.position + cameraTransform.forward * gunData.shootingRange;
        }
        
        StartCoroutine(BulletFire(target, hit));
    }

    private void ApplyDamageToTarget(Collider targetCollider, float damage)
    {
        int damageAmount = Mathf.RoundToInt(damage);
        
        if (targetCollider.CompareTag("Player"))
        {
            // Damage to Player
            PlayerBehaviour player = targetCollider.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.PlayerTakeDmg(damageAmount);
            }
        }
        else if (targetCollider.CompareTag("Enemy"))
        {
            // Damage to Enemy yang spesifik
            EnemyBehaviour enemy = targetCollider.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damageAmount);
            }
        }
        else
        {
            //Debug.Log("Hit " + targetCollider.name + " but no specific health system applied");
        }
    }

    private IEnumerator BulletFire(Vector3 target, RaycastHit hit)
    {
        GameObject bulletTrail = Instantiate(gunData.bulletTrailPrefab, gunMuzzle.position, Quaternion.identity);

        while (bulletTrail != null && Vector3.Distance(bulletTrail.transform.position, target) > 0.1f)
        {
            bulletTrail.transform.position = Vector3.MoveTowards(bulletTrail.transform.position, target, Time.deltaTime * gunData.bulletSpeed);
            yield return null;
        }

        Destroy(bulletTrail);

        if (hit.collider != null)
        {
            BulletHitFX(hit);
        }
    }

    private void BulletHitFX(RaycastHit hit)
    {
        Vector3 hitPosition = hit.point + hit.normal * 0.01f;

        GameObject bulletHole = Instantiate(bulletHolePrefab, hitPosition, Quaternion.LookRotation(hit.normal));
        //GameObject hitParticle = Instantiate(bulletHitParticlePrefab, hitPosition, Quaternion.LookRotation(hit.normal));

        bulletHole.transform.parent = hit.collider.transform;
        //hitParticle.transform.parent = hit.collider.transform;

        Destroy(bulletHole, 5f);
        //Destroy(hitParticle, 5f);
    }
}
