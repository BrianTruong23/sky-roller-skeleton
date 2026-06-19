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

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement == null)
        {
            return;
        }

        hasLaunched = true;
        movement.LaunchUp(upwardSpeed, forwardBoost, boostDuration);
    }
}
