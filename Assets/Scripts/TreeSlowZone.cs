using UnityEngine;

public class TreeSlowZone : MonoBehaviour
{
    [SerializeField] float stopDuration = 0.5f;
    [SerializeField] float forwardMultiplier = 0.4f;
    [SerializeField] float sideMultiplier = 0.4f;

    bool hasStoppedPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement == null)
        {
            return;
        }

        if (!hasStoppedPlayer)
        {
            hasStoppedPlayer = true;
            movement.StopBriefly(stopDuration);
        }

        movement.SetMovementMultipliers(forwardMultiplier, sideMultiplier);
    }

    void OnTriggerStay(Collider other)
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

        hasStoppedPlayer = false;
    }
}
