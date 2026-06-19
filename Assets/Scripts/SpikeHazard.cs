using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    [SerializeField] GameOverUI gameOverUI;

    bool hasLost;

    void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
            col.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TryKill(other);
    }

    void OnTriggerStay(Collider other)
    {
        TryKill(other);
    }

    void TryKill(Collider other)
    {
        if (hasLost)
        {
            return;
        }

        if (!other.CompareTag("Player") || other.attachedRigidbody == null)
        {
            return;
        }

        if (gameOverUI == null)
        {
            return;
        }

        hasLost = true;
        EffectNotificationUI.Show("You hit the spikes!");
        gameOverUI.showGameOver();
        Time.timeScale = 0f;
    }
}
