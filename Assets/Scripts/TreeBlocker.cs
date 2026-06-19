using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TreeBlocker : MonoBehaviour
{
    [SerializeField] string notificationMessage = "Blocked by the tree — swerve left or right!";

    bool playerNotified;

    void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerMovement movement = GetMovement(collision);
        if (movement == null)
        {
            return;
        }

        if (!playerNotified)
        {
            playerNotified = true;
            EffectNotificationUI.Show(notificationMessage);
        }

        movement.AddForwardBlock();
    }

    void OnCollisionStay(Collision collision)
    {
        PlayerMovement movement = GetMovement(collision);
        if (movement == null)
        {
            return;
        }

        Rigidbody playerRb = collision.rigidbody;
        Vector3 velocity = playerRb.linearVelocity;
        if (velocity.z > 0f)
        {
            playerRb.linearVelocity = new Vector3(velocity.x, velocity.y, 0f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        PlayerMovement movement = GetMovement(collision);
        if (movement == null)
        {
            return;
        }

        playerNotified = false;
        movement.RemoveForwardBlock();
    }

    static PlayerMovement GetMovement(Collision collision)
    {
        if (!collision.collider.CompareTag("Player") || collision.rigidbody == null)
        {
            return null;
        }

        return collision.rigidbody.GetComponent<PlayerMovement>();
    }
}
