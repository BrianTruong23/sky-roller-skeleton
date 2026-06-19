using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    [SerializeField] GameOverUI gameOverUI;

    bool hasLost;

    void OnTriggerEnter(Collider other)
    {
        if (hasLost || !other.CompareTag("Player"))
        {
            return;
        }

        hasLost = true;
        gameOverUI.showGameOver();
        Time.timeScale = 0f;
    }
}
