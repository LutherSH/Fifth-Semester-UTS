using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Gun gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform fpsController, weaponHolder, mainCamera;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;
}
