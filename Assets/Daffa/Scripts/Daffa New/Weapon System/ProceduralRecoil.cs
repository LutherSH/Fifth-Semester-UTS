// using UnityEngine;

// public class ProceduralRecoil : MonoBehaviour
// {
//     // References
//     private WeaponConfig weaponConfig;

//     private Vector2 currentRotation;
//     private bool randomizeRecoil;

//     [Header("Random Recoil")]
//     public Vector2 randomRecoilConstraints;

//     [Header("Pattern Recoil")]
//     public Vector2[] recoilPattern;

//     public void DetermineRecoil()
//     {
//         transform.localPosition = Vector3.forward * 0.01f;

//         if (randomizeRecoil)
//         {
//             float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
//             float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

//             Vector2 recoil = new Vector2(xRecoil, yRecoil);

//             currentRotation += recoil;
//         }
//         else
//         {
//             int currentStep = weaponConfig.maxReserveAmmo + 1 - weaponConfig.magazineSize;
//             currentStep = Mathf.Clamp(currentStep, 0, recoilPattern.Length - 1);
//         }
//     }
// }
