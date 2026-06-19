using UnityEngine;

public class SlowZone : MonoBehaviour
{
    [SerializeField] float forwardMultiplier = 0.5f;
    [SerializeField] float sideMultiplier = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        ApplySlow(other);
    }

    void OnTriggerStay(Collider other)
    {
        ApplySlow(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.ResetMovementMultipliers();
        }
    }

    void ApplySlow(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.SetMovementMultipliers(forwardMultiplier, sideMultiplier);
        }
    }
}
