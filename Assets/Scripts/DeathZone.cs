using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{

    [SerializeField] GameOverUI gameOverUI;

    bool hasLost;
    private void OnTriggerEnter(Collider collider)
    {

        if (hasLost || !collider.CompareTag("Player"))
        {
            return;
        }

        hasLost = true;
        gameOverUI.showGameOver();
        Time.timeScale = 0f;
    }


}
