using UnityEngine;

public class CoasterLaunchZone : MonoBehaviour
{
    [SerializeField] float upwardSpeed = 18f;
    [SerializeField] float forwardBoost = 3f;
    [SerializeField] float boostDuration = 2f;

    bool hasLaunched;

    void OnTriggerEnter(Collider other)
    {
        if (hasLaunched || !other.CompareTag("Player"))
        {
            return;
        }

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null)
        {
            return;
        }

        PlayerMovement movement = rb.GetComponent<PlayerMovement>();
        if (movement == null)
        {
            return;
        }

        hasLaunched = true;
        EffectNotificationUI.Show("Launched upward!");
        movement.LaunchUp(upwardSpeed, forwardBoost, boostDuration);
    }
}
