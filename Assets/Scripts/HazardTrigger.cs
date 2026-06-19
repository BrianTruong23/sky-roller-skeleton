using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HazardTrigger : MonoBehaviour
{
    public enum HazardType
    {
        Slow,
        TreeSlow,
        SpikeKill
    }

    [SerializeField] HazardType hazardType;
    [SerializeField] GameOverUI gameOverUI;
    [SerializeField] float forwardMultiplier = 0.35f;
    [SerializeField] float sideMultiplier = 0.35f;
    [SerializeField] string slowMessage = "You have been slowed down.";
    [SerializeField] string spikeMessage = "You hit the spikes!";

    bool spikeTriggered;
    bool playerInside;

    void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerMovement movement = GetPlayerMovement(other);
        if (movement == null)
        {
            return;
        }

        playerInside = true;

        switch (hazardType)
        {
            case HazardType.Slow:
                movement.EnterSlowZone(forwardMultiplier, sideMultiplier);
                EffectNotificationUI.Show(slowMessage);
                break;
            case HazardType.SpikeKill:
                EffectNotificationUI.Show(spikeMessage);
                TriggerDeath();
                break;
        }
    }

    void OnTriggerStay(Collider other)
    {
        PlayerMovement movement = GetPlayerMovement(other);
        if (movement == null || hazardType != HazardType.Slow)
        {
            return;
        }

        movement.ApplySlowEffect(forwardMultiplier, sideMultiplier);
    }

    void OnTriggerExit(Collider other)
    {
        PlayerMovement movement = GetPlayerMovement(other);
        if (movement == null)
        {
            return;
        }

        playerInside = false;

        if (hazardType == HazardType.Slow)
        {
            movement.ExitSlowZone();
        }
    }

    void TriggerDeath()
    {
        if (spikeTriggered || gameOverUI == null)
        {
            return;
        }

        spikeTriggered = true;
        gameOverUI.showGameOver();
        Time.timeScale = 0f;
    }

    static PlayerMovement GetPlayerMovement(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return null;
        }

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null)
        {
            return null;
        }

        return rb.GetComponent<PlayerMovement>();
    }
}
