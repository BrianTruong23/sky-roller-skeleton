using UnityEngine;

public class SlowZone : MonoBehaviour
{
    [SerializeField] float forwardMultiplier = 0.12f;
    [SerializeField] float sideMultiplier = 0.25f;
    [SerializeField] string notificationMessage = "You have been slowed down.";

    bool playerInside;

    void OnTriggerEnter(Collider other)
    {
        PlayerMovement movement = GetPlayerMovement(other);
        if (movement != null)
        {
            movement.EnterSlowZone(forwardMultiplier, sideMultiplier);
        }

        if (GetPlayerMovement(other) != null && !playerInside)
        {
            playerInside = true;
            EffectNotificationUI.Show(notificationMessage);
        }
    }

    void OnTriggerStay(Collider other)
    {
        ApplySlow(other);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerMovement movement = GetPlayerMovement(other);
        if (movement != null)
        {
            playerInside = false;
            movement.ExitSlowZone();
        }
    }

    void ApplySlow(Collider other)
    {
        PlayerMovement movement = GetPlayerMovement(other);
        if (movement != null)
        {
            movement.ApplySlowEffect(forwardMultiplier, sideMultiplier);
        }
    }

    static PlayerMovement GetPlayerMovement(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return null;
        }

        Rigidbody rb = other.attachedRigidbody;
        return rb != null ? rb.GetComponent<PlayerMovement>() : null;
    }
}
